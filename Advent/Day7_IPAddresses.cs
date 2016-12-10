using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent
{
    public class Day7_IPAddresses
    {
        public static int CountIpAddressesWithTslSupport(string[] input)
        {
            return input.Count(ipAddress => HasTlsSupport(ipAddress.Trim()));
        }

        public static int CountIpAddressesThatSupportSsl(string[] input)
        {
            return input.Count(ipAddress => HasSslSupports(ipAddress.Trim()));
        }

        public static bool HasTlsSupport(string ipAddress)
        {
            bool hasAbbaPattern = false;
            bool isInvalidAddress = false; // abba pattern found in brackets
            bool insideBrackets = false;

            for (int i = 0; i < ipAddress.Length; i++)
            {
                char c = ipAddress[i];
                if (c == '[')
                {
                    insideBrackets = true;
                }
                else if (c == ']')
                {
                    insideBrackets = false;
                }
                else if (i < ipAddress.Length - 3)
                {
                    char[] fourLetterSample = ipAddress.Skip(i).Take(4).ToArray();
                    if (!fourLetterSample.Contains('[') && 
                        !fourLetterSample.Contains('[') &&
                        fourLetterSample[0] == fourLetterSample[3] &&
                        fourLetterSample[1] == fourLetterSample[2] && 
                        fourLetterSample[0] != fourLetterSample[1])
                    {
                        hasAbbaPattern = true;
                        if (insideBrackets)
                        {
                            isInvalidAddress = true;
                            break;
                        }
                    }
                }
            }

            return hasAbbaPattern && !isInvalidAddress;
        }

        public static bool HasSslSupports(string ipAddress)
        {
            bool insideBrackets = false;

            List<string> abaPatterns = new List<string>();
            List<string> babPatterns = new List<string>();

            for (int i = 0; i < ipAddress.Length; i++)
            {
                char c = ipAddress[i];
                if (c == '[')
                {
                    insideBrackets = true;
                }
                else if (c == ']')
                {
                    insideBrackets = false;
                }
                else if (i < ipAddress.Length - 2)
                {
                    char[] threeLetterSample = ipAddress.Skip(i).Take(3).ToArray();
                    if (!threeLetterSample.Contains('[') && 
                        !threeLetterSample.Contains('[') &&
                        threeLetterSample[0] == threeLetterSample[2] &&
                        threeLetterSample[0] != threeLetterSample[1])
                    {
                        if (insideBrackets)
                        {
                            babPatterns.Add(new string(threeLetterSample));
                        }
                        else
                        {
                            abaPatterns.Add(new string(threeLetterSample));
                        }
                    }
                }
            }

            return abaPatterns.Any(aba => babPatterns.Any(bab => bab[0] == aba[1] && aba[0] == bab[1]));
        }
    }
}
