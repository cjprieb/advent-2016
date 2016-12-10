using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day6_Jamming
    {
        public enum RepetitionCodeType
        {
            MostLikely,
            LeastLikely
        }

        public static string ExtractErrorFreeMessage(string[] messages, RepetitionCodeType codeType = RepetitionCodeType.MostLikely)
        {
            //Count characters in each column
            List<Dictionary<char, int>> columnCounts = new List<Dictionary<char, int>>();
            foreach (string message in messages.Select(str => str.Trim()))
            {
                for (int i = 0; i < message.Length; i++)
                {
                    Dictionary<char, int> characterCounts = null;
                    if (i == columnCounts.Count())
                    {
                        characterCounts = new Dictionary<char, int>();
                        columnCounts.Add(characterCounts);
                    }
                    else
                    {
                        characterCounts = columnCounts[i];
                    }

                    char c = message[i];
                    if (characterCounts.ContainsKey(c))
                    {
                        characterCounts[c] += 1;
                    } 
                    else
                    {
                        characterCounts[c] = 1;
                    }
                }
            }

            //Build into message
            char[] charArray = columnCounts
                .Select(dict =>
                {
                    IEnumerable<KeyValuePair<char, int>> order = null;
                    switch (codeType)
                    {
                        case RepetitionCodeType.MostLikely:
                            order = dict.OrderByDescending(kvp => kvp.Value);
                            break;

                        case RepetitionCodeType.LeastLikely:
                            order = dict.OrderBy(kvp => kvp.Value);
                            break;
                    }
                    return order != null ? order.First().Key : '_';
                })
                .ToArray();
            return new string(charArray); ;
        }
    }
}
