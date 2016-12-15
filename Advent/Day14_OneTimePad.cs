using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent
{
    public class Day14_OneTimePad
    {
        public static List<int> GenerateKeys(string salt, int repeats = 0)
        {
            List<int> keys = new List<int>();
            KeyGenerator generator = new KeyGenerator(salt);
            int index = 0;
            while (keys.Count() < 64)
            {
                generator.GeneratorHashForIndex(index, repeats);
                string threeChars = generator.GetThreeOfAKind(index);
                if (threeChars != null)
                {
                    string fiveChars = new string(threeChars[0], 5);
                    int max = index + 1000;
                    for(int i = index+1; i <= max; i++)
                    {
                        generator.GeneratorHashForIndex(i, repeats);
                        if (generator.DoesHashContainString(i, fiveChars))
                        {
                            keys.Add(index);
                            break;
                        }
                    }
                }

                index++;
            }

            Console.WriteLine($"{generator.TotalHashes} hashes were performed");
            return keys;
        }

        class KeyGenerator
        {
            private MD5 _Md5 = null;
            private string _Salt = null;
            private Dictionary<int, string> _ThreeOfAKind = new Dictionary<int, string>();
            private Dictionary<int, string> _FiveOfAKind = new Dictionary<int, string>();
            private int _MaxIndexChecked = -1;
            private long _TotalHashes = 0;
            private Regex _ThreeOfAKindPattern = new Regex(@"(\w)\1{2}");
            private Regex _FiveOfAKindPattern = new Regex(@"(\w)\1{4}");

            public long TotalHashes
            {
                get { return _TotalHashes; }
            }

            public KeyGenerator(string salt)
            {
                _Md5 = MD5.Create();
                _Salt = salt;
            }

            public void GeneratorHashForIndex(int index, int repeats = 0)
            {
                if (_MaxIndexChecked < index)
                {
                    string input = _Salt + index;
                    string hash = input;
                    for (int i = 0; i <= repeats; i++)
                    {
                        hash = GetHashString(hash);
                    }

                    Match match = _ThreeOfAKindPattern.Match(hash);
                    if (match.Success)
                    {
                        _ThreeOfAKind[index] = match.Groups[0].Value;

                        if (_FiveOfAKindPattern.IsMatch(hash))
                        {
                            _FiveOfAKind[index] = hash;
                        }
                    }

                    _MaxIndexChecked = index;
                }
            }

            private string GetHashString(string input)
            {
                _TotalHashes++;

                // Convert the input string to a byte array and compute the hash.
                byte[] data = _Md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

            internal bool DoesHashContainString(int index, string fiveOfAKind)
            {
                if (_FiveOfAKind.ContainsKey(index))
                {
                    return _FiveOfAKind[index].Contains(fiveOfAKind);
                }
                else
                {
                    return false;
                }
            }

            internal string GetThreeOfAKind(int index)
            {
                if (_ThreeOfAKind.ContainsKey(index))
                {
                    return _ThreeOfAKind[index];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
