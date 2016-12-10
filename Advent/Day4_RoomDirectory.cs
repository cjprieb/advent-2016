using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
    public class Day4_RoomDirectory
    {
        public static int SumOfValidRoomIds(string[] input)
        {
            return GetValidRooms(input).Sum(room => room.SectorId);
        }

        public static List<Room> GetValidRooms(string[] input)
        {
            List<Room> list = new List<Room>();
            Regex roomPattern = new Regex(@"([\w\-]+)-(\d+)\[(\w+)\]");
            foreach (var room in input)
            {
                Match match = roomPattern.Match(room);
                if (match.Success)
                {
                    string roomName = match.Groups[1].Value;
                    string idString = match.Groups[2].Value;
                    string checksum = match.Groups[3].Value;

                    if (IsValidRoom(roomName, checksum))
                    {
                        list.Add(new Room()
                        {
                            EncryptedRoomName = roomName,
                            CheckSum = checksum,
                            SectorId = int.Parse(idString)
                        });
                    }
                }
            }
            return list;
        }

        public static List<Room> FindRooms(string[] input, string searchString)
        { 
            searchString = searchString.Replace(" ", "");
            return GetValidRooms(input)
                .Where(room =>
                {
                    string decryptedName = DecryptName(room.EncryptedRoomName, room.SectorId);
                    return decryptedName.Replace(" ", "").Contains(searchString);
                })
                .ToList();
        }

        public static bool IsValidRoom(string encryptedName, string checksum)
        {
            Dictionary<char, int> letterCount = new Dictionary<char, int>();
            foreach ( char c in encryptedName )
            {
                if (c != '-')
                {
                    int count;
                    letterCount[c] = letterCount.TryGetValue(c, out count) ? count + 1 : 1;
                }
            }

            List<char> mostCommonLetters = letterCount
                .OrderBy(kvp => kvp, new KeyValueComparer())
                .Take(5)
                .Select(kvp => kvp.Key)
                .ToList();

            bool isValid = true;
            foreach ( char c in checksum )
            {
                if ( !mostCommonLetters.Contains(c) )
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        public static string DecryptName(string encryptedName, int rotation)
        {
            rotation = rotation % 26;
            int reverseRotation = 26 - rotation;

            StringBuilder decryptedName = new StringBuilder();
            foreach ( char c in encryptedName )
            {
                if ( c == '-' )
                {
                    decryptedName.Append(" ");
                }
                else
                {
                    int decryptedChar = (c + rotation) > 'z' ? c - reverseRotation : c + rotation;
                    decryptedName.Append((char)decryptedChar);
                }
            }

            return decryptedName.ToString();
        }

        class KeyValueComparer : IComparer<KeyValuePair<char, int>>
        {
            public int Compare(KeyValuePair<char, int> x, KeyValuePair<char, int> y)
            {
                if ( x.Value == y.Value )
                {
                    return x.Key.CompareTo(y.Key);
                }
                else
                {
                    return y.Value.CompareTo(x.Value);
                }
            }
        }

        public class Room
        {
            public string EncryptedRoomName { get; set; }
            public string CheckSum { get; set; }
            public int SectorId { get; set; }
        }
    }
}
