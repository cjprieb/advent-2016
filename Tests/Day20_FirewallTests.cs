using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass()]
    public class Day20_FirewallTests
    {
        private string[] SampleRanges =
        {
            "5-8", "0-2", "4-7"
        };

        [TestMethod]
        public void Test_LowestUnblockedFirewall()
        {
            //Setup
            string[] input = SampleRanges;
            long max = 9;
            long expected = 3;

            //Action
            long lowest = Day20_Firewall.LowestUnblockedFirewall(input, max);

            //Assert
            Assert.AreEqual(expected, lowest);
        }

        [TestMethod]
        public void Answer1_LowestUnblockedFirewall()
        {
            //Setup
            string[] input = Resources.Day20_Input.Split('\n');
            long max = 4294967295;
            long expected = 17348574;

            //Action
            long lowest = Day20_Firewall.LowestUnblockedFirewall(input, max);

            //Assert
            Assert.AreEqual(expected, lowest);
        }

        [TestMethod]
        public void Test_IpAddressesAllowed()
        {
            //Setup
            string[] input = SampleRanges;
            long max = 9;
            long expected = 2;

            //Action
            long count = Day20_Firewall.IpAddressesAllowed(input, max);

            //Assert
            Assert.AreEqual(expected, count);
        }

        [TestMethod]
        public void Answer2_IpAddressesAllowed()
        {
            //Setup
            string[] input = Resources.Day20_Input.Split('\n');
            long max = 4294967295;
            long expected = 104;

            //Action
            long count = Day20_Firewall.IpAddressesAllowed(input, max);

            //Assert
            Assert.AreEqual(expected, count);
        }
    }
}