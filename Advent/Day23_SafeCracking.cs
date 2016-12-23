using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day23_SafeCracking
    {
        public static int CodeForTheSafe(string[] input, int startingValue = 7)
        {
            Dictionary<char, int> startingValues = new Dictionary<char, int>
            {
                { 'a', startingValue }
            };
            AssemBunny computer = new AssemBunny(startingValues);
            computer.RunInstructions(input);
            return computer.Registers['a'];
        }
    }
}
