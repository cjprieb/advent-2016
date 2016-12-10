using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Advent
{
    public class Day5_Md5Hash
    {
        public class PasswordGenerator
        {
            private const string _Zeros = "00000";
            private const char _EmptyChar = '_';

            char[] _Password = null;
            private MD5 _Md5 = null;
            private string _DoorId = null;

            public PasswordGenerator(string doorId)
            {
                _Md5 = MD5.Create();
                _DoorId = doorId;
                _Password = new string(_EmptyChar, 8).ToArray();
            }

            public void GeneratePassword(int startingIndex = 0, int offset = 1)
            {
                int passwordIndex = 0;
                for (int i = startingIndex; ; i += offset)
                {
                    string hashStringOuput = GetHashString(_Md5, $"{_DoorId}{i}");
                    if (hashStringOuput.StartsWith(_Zeros))
                    {
                        _Password[passwordIndex] = hashStringOuput[5];
                        passwordIndex++;

                        if (HasPassword())
                        {
                            break;
                        }
                    }
                }
            }

            public void GeneratePositionalPassword(int startingIndex = 0, int offset = 1)
            {
                for (int i = startingIndex; ; i += offset)
                {
                    string hashStringOuput = GetHashString(_Md5, $"{_DoorId}{i}");
                    if (hashStringOuput.StartsWith(_Zeros))
                    {
                        int position = hashStringOuput[5] - '0';

                        if (position >= 0 && position < 8 && _Password[position] == _EmptyChar)
                        {
                            _Password[position] = hashStringOuput[6];
                            if (HasPassword())
                            {
                                break;
                            }
                        }
                    }
                }
            }

            public bool HasPassword()
            {
                return _Password.All(c => c != _EmptyChar);
            }

            public string CreatePassword()
            {
                return new string(_Password);
            }
        }

        public static string GeneratePassword(string doorId)
        {
            Console.WriteLine($"Generating password for : {doorId}");
            PasswordGenerator generator = new PasswordGenerator(doorId);

            //int threads = 6;
            //List<int> indexList = new List<int>();
            //for(int i = 0; i < threads; i++)
            //{
            //    indexList.Add(i);
            //}
            //Parallel.ForEach(indexList, index => generator.GeneratePassword(index, threads));

            generator.GeneratePassword();

            string password = generator.CreatePassword();
            Console.WriteLine($"\nPassword found: " + password);
            return password;
        }

        public static string GeneratePositionalPassword(string doorId)
        {
            Console.WriteLine($"Generating password for : {doorId}");
            PasswordGenerator generator = new PasswordGenerator(doorId);

            //int threads = 6;
            //List<int> indexList = new List<int>();
            //for(int i = 0; i < threads; i++)
            //{
            //    indexList.Add(i);
            //}
            //Parallel.ForEach(indexList, index => generator.GeneratePassword(index, threads));

            generator.GeneratePositionalPassword();

            string password = generator.CreatePassword();
            Console.WriteLine($"\nPassword found: " + password);
            return password;
        }

        private static string GetHashString(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

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

        private static void WritePasswordToConsole(char nextChar)
        {
            Console.Write(nextChar);
        }
    }
}
