using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent.Day10_BalanceBots;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass]
    public class Day10_BalanceBotsTests
    {
        static string[] _SampleInstructions =
        {
            "value 5 goes to bot 2",
            "bot 2 gives low to bot 1 and high to bot 0",
            "value 3 goes to bot 1",
            "bot 1 gives low to output 1 and high to bot 0",
            "bot 0 gives low to output 2 and high to output 0",
            "value 2 goes to bot 2"
        };

        [TestMethod]
        public void Test_ParseInstructions()
        {
            //Action
            BalanceBots bots = ProcessInstructions(_SampleInstructions);

            //Assert
            Assert.AreEqual(1, bots.GetBin("0").ChipValues.Count, "output 0 chip count");
            Assert.AreEqual(1, bots.GetBin("1").ChipValues.Count, "output 1 chip count");
            Assert.AreEqual(1, bots.GetBin("2").ChipValues.Count, "output 2 chip count");
            
            Assert.AreEqual(5, bots.GetBin("0").ChipValues[0], "output 0 chip value");
            Assert.AreEqual(2, bots.GetBin("1").ChipValues[0], "output 1 chip value");
            Assert.AreEqual(3, bots.GetBin("2").ChipValues[0], "output 2 chip value");
        }

        [TestMethod]
        public void Answer_ParseInstructions()
        {
            //Setup
            int low = 17;
            int high = 61;

            //Action
            BalanceBots bots = ProcessInstructions(Resources.Day10_Input.Split('\n'), new int[] { low, high });

            //Assert
            Assert.IsTrue(bots.BotsWhoDidImportantComparison.Count > 0);

            Bot bot = bots.BotsWhoDidImportantComparison[0];
            Console.WriteLine($"Bot {bot.BotId} compared value-{low} microchip with value-{high} microchip.");
            Assert.AreEqual("86", bot.BotId);

            Bot[] outputs = {
                bots.GetBin("0"),
                bots.GetBin("1"),
                bots.GetBin("2")
            };
            Console.WriteLine($"Output bin 0: {ToString(outputs[0].ChipValues)}");
            Console.WriteLine($"Output bin 1: {ToString(outputs[1].ChipValues)}");
            Console.WriteLine($"Output bin 2: {ToString(outputs[2].ChipValues)}");
            int answer = outputs.Select(bin => bin.ChipValues[0]).Aggregate(1, (total, next) => total * next);
            Console.WriteLine($"Values in output bins 0, 1, 2 multiplied together: {answer}");
        }

        private static string ToString(IEnumerable<int> values)
        {
            return "[" + string.Join(",", values.Select(i => i.ToString())) + "]";
        }
    }
}