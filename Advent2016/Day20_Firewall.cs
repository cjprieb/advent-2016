using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Advent
{
    public class Day20_Firewall
    {
        public static long LowestUnblockedFirewall(string[] blockedRanges, long max)
        {
            long minAddress = -1;
            long minUncheckedIndex = 0;
            SortedList<long, Range> ranges = Parse(blockedRanges);
            foreach ( var range in ranges )
            {
                if (range.Key > minUncheckedIndex)
                {
                    minAddress = minUncheckedIndex;
                    break;
                }
                else
                {
                    minUncheckedIndex = range.Value.End + 1;
                }
            }
            return minAddress;
        }

        public static long IpAddressesAllowed(string[] blockedRanges, long max)
        {
            long validIpAddressCount = 0;
            long minUncheckedIndex = 0;
            SortedList<long, Range> ranges = Parse(blockedRanges);
            foreach (var range in ranges)
            {
                if (range.Key > minUncheckedIndex)
                {
                    validIpAddressCount += (range.Key - minUncheckedIndex);
                }

                if (range.Value.End >= minUncheckedIndex)
                {
                    minUncheckedIndex = (range.Value.End + 1);
                }
            }
            if (minUncheckedIndex <= max)
            {
                validIpAddressCount += (max - minUncheckedIndex + 1);
            }
            return validIpAddressCount;
        }

        private static SortedList<long, Range> Parse(string[] blockedRanges)
        {
            Regex rangePattern = new Regex(@"(\d+)-(\d+)");
            SortedList<long, Range> ranges = new SortedList<long, Range>();
            foreach (var str in blockedRanges)
            {
                Match match = rangePattern.Match(str);
                if (match.Success)
                {
                    long start = long.Parse(match.Groups[1].Value);
                    long end = long.Parse(match.Groups[2].Value);
                    if (ranges.ContainsKey(start))
                    {
                        throw new Exception($"list of ranges already contains {start}: {ranges[start]}");
                    }
                    ranges.Add(start, new Range(start, end));
                }
                else
                {
                    throw new Exception($"{str} does not match {rangePattern}");
                }
            }
            return ranges;
        }



        #region class Range
        public struct Range
        {
            public long Start;
            public long End;

            public Range(long value)
            {
                Start = value;
                End = value;
            }

            public Range(long start, long end)
            {
                Start = start;
                End = end;
            }

            public override bool Equals(object obj)
            {
                if (obj is Range)
                {
                    Range range = (Range)obj;
                    return range.Start == Start && range.End == End;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return $"{Start}-{End}".GetHashCode();
            }

            public override string ToString()
            {
                return $"{Start}-{End}";
            }
        }
        #endregion class Range
    }
}
