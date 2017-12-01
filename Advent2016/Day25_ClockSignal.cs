using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day25_ClockSignal
    {
        private const int MAX_VALUE = 10000;

        private const int MAX_TRANSMITTED = 100;

        public static int FindInitialValueForRegister(string[] input)
        {
            int result = -1;
            for (var i = 0; i < MAX_VALUE; i++)
            {
                var resultList = GetTransmittedValuesFor(input, i);
                if (IsValidClockResult(resultList))
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        public static IEnumerable<int> GetTransmittedValuesFor(string[] input, int value)
        {
            AssemBunny bunny = new AssemBunny(new Dictionary<char, int> { { 'a', value } }, MAX_TRANSMITTED);
            bunny.RunInstructions(input);
            return bunny.TransmittedValues;
        }

        private static bool IsValidClockResult(IEnumerable<int> resultList)
        {
            var isValid = resultList.Any();
            var previous = 1;
            foreach (var item in resultList)
            {
                if ((previous == 0 && item == 1) || (previous == 1 && item == 0))
                {
                    previous = item;
                }
                else
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}
