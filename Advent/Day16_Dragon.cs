using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day16_Dragon
    {
        private static int _repeats;

        public static string ProcessDragonExtensionAsString(string input)
        {
            var result = ProcessDragonExtension(ConvertToList(input));
            return new string(result.Select(b => b ? '1' : '0').ToArray());
        }

        public static string Checksum(string input)
        {
            var data = ConvertToList(input);
            var result = ChecksumBuilder(data, input.Length);
            return new string(data.Take(result).Select(b => b ? '1' : '0').ToArray());
        }

        /// <summary>
        /// The checksum for some given data is created by considering each 
        /// non-overlapping pair of characters in the input data. 
        /// If the two characters match (00 or 11), the next checksum character 
        /// is a 1. 
        /// If the characters do not match (01 or 10), the next checksum 
        /// character is a 0. 
        /// This should produce a new string which is exactly half as long as 
        /// the original. 
        /// If the length of the checksum is even, repeat the process until you 
        /// end up with a checksum with an odd length.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ChecksumBuilder(List<bool> input, int size)
        {
            int reduceIndex = 0;
            if (size % 2 == 0 && size > 0)
            {
                int max = size-1;

                for (int i = 0; i < max; i += 2)
                {
                    input[reduceIndex] = (input[i] == input[i + 1]);
                    reduceIndex++;
                }

                reduceIndex = ChecksumBuilder(input, reduceIndex);
            }
            else
            {
                reduceIndex = size;
            }
            return reduceIndex;
        }

        /// <summary>
        /// Call the data you have at this point "a".
        /// Make a copy of "a"; call this copy "b".
        /// Reverse the order of the characters in "b".
        /// In "b", replace all instances of 0 with 1 and all 1s with 0.
        /// The resulting data is "a", then a single 0, then "b".
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<bool> ProcessDragonExtension(List<bool> input)
        {
            int length = input.Count;
            input.Add(false);
            for (int i = length-1; i >= 0; i--)
            {
                input.Add(!input[i]);
            }
            return input;
        }

        public static string GetChecksumForLength(string input, int length)
        {
            var data = ConvertToList(input);
            while (data.Count < length)
            {
                data = ProcessDragonExtension(data);
            }

            //data = data.Take(length).ToList();
            Console.WriteLine($"data size is {data.Count}");
            var result = ChecksumBuilder(data, length);
            return new string(data.Take(result).Select(b => b ? '1' : '0').ToArray());
        }

        public static List<bool> ConvertToList(string input)
        {
            return input.Select(c => c == '1').ToList();
        }
    }
}
