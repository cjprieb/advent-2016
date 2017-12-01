using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent
{
    public class Day09_Decompression
    {
        static Regex _MarkerPattern = new Regex(@"\((\d+)x(\d+)\)");

        public static string Decompress(string input)
        {
            StringBuilder result = new StringBuilder();
            
            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c != '(')
                {
                    result.Append(c);
                    continue;
                }

                int closingParenthesis = input.IndexOf(')', i);
                if (closingParenthesis > 0)
                {
                    string marker = input.Substring(i, closingParenthesis - i + 1);
                    Match match = _MarkerPattern.Match(marker);
                    if (match.Success)
                    {
                        int startIndex = i + marker.Length;
                        int lengthToRepeat = int.Parse(match.Groups[1].Value);
                        int timesToRepeat = int.Parse(match.Groups[2].Value);

                        if ((lengthToRepeat + startIndex) > input.Length)
                        {
                            lengthToRepeat = input.Length - startIndex;
                        }

                        string repeatingSection = input.Substring(startIndex, lengthToRepeat);
                        while (timesToRepeat > 0)
                        {
                            result.Append(repeatingSection);
                            timesToRepeat--;
                        }

                        i += marker.Length + lengthToRepeat - 1;
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
            }
            
            return result.ToString();
        }

        public static long DecompressedLength(string input)
        {
            long length = 0;
            MatchCollection matches = _MarkerPattern.Matches(input);
            if (matches.Count > 0)
            {
                int minValidMatchIndex = 0;
                foreach (Match match in matches)
                {
                    if (match.Index < minValidMatchIndex)
                    {
                        continue; // will be processed by reccursion
                    }
                    else if (match.Index > minValidMatchIndex)
                    {
                        length += match.Index - minValidMatchIndex;
                    }

                    int lengthToRepeat = int.Parse(match.Groups[1].Value);
                    int timesToRepeat = int.Parse(match.Groups[2].Value);
                    int startingIndex = match.Index + match.Groups[0].Value.Length;

                    string substringToProcess = input.Substring(startingIndex, lengthToRepeat);
                    minValidMatchIndex = startingIndex + lengthToRepeat;
                    length += timesToRepeat * DecompressedLength(substringToProcess);
                }
                if (minValidMatchIndex < input.Length)
                {
                    length += input.Length - minValidMatchIndex;
                }
            }
            else
            {
                length += input.Length;
            }
            return length;
        }
    }
}
