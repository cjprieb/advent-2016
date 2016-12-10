using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass]
    public class Day7_IPAddressesTests
    {
        [TestMethod]
        public void Test_IsValidIpAddress_abba()
        {
            //Setup
            string input = "abba[mnop]qrst";

            //Action
            bool result = Day7_IPAddresses.HasTlsSupport(input);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_IsValidIpAddress_abcd()
        {
            //Setup
            string input = "abcd[bddb]xyyx";

            //Action
            bool result = Day7_IPAddresses.HasTlsSupport(input);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_IsValidIpAddress_aaaa()
        {
            //Setup
            string input = "aaaa[qwer]tyui";

            //Action
            bool result = Day7_IPAddresses.HasTlsSupport(input);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_IsValidIpAddress_ioxxoj()
        {
            //Setup
            string input = "ioxxoj[asdfgh]zxcvbn";

            //Action
            bool result = Day7_IPAddresses.HasTlsSupport(input);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Answer1_IsValidIpAddress()
        {
            //Setup
            string[] input = Resources.Day7_Input.Split('\n');

            //Action
            int count = Day7_IPAddresses.CountIpAddressesWithTslSupport(input);

            //Assert
            Console.WriteLine($"There are [{count}] IP addresses that support TSL in the list.");
            Assert.AreEqual(110, count, "count mismatch");
        }

        [TestMethod]
        public void Test_SupportsSsl_aba()
        {
            //Setup
            string input = "aba[bab]xyz";

            //Action
            bool result = Day7_IPAddresses.HasSslSupports(input);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_SupportsSsl_xyx()
        {
            //Setup
            string input = "xyx[xyx]xyx";

            //Action
            bool result = Day7_IPAddresses.HasSslSupports(input);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_SupportsSsl_aaa()
        {
            //Setup
            string input = "aaa[kek]eke";

            //Action
            bool result = Day7_IPAddresses.HasSslSupports(input);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_SupportsSsl_zazbz()
        {
            //Setup
            string input = "zazbz[bzb]cdb";

            //Action
            bool result = Day7_IPAddresses.HasSslSupports(input);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Answer2_SupportsSsl()
        {
            //Setup
            string[] input = Resources.Day7_Input.Split('\n');

            //Action
            int count = Day7_IPAddresses.CountIpAddressesThatSupportSsl(input);

            //Assert
            Console.WriteLine($"There are [{count}] IP addresses that support SSL in the list.");
            Assert.AreEqual(242, count, "count mismatch");
        }
    }
}