using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Tests
{
    [TestClass]
    public class Day5_Md5HashTests
    {
        [TestMethod]
        public void Test_GeneratePassword()
        {
            //Setup
            string input = "abc";

            //Action
            string password = Day5_Md5Hash.GeneratePassword(input);

            //Assert
            Assert.AreEqual("18f47a30", password, "Password mismtach");
        }

        [TestMethod]
        public void Answer1_GeneratePassword()
        {
            //Setup
            string input = "ugkcyxxp";

            //Action
            string password = Day5_Md5Hash.GeneratePassword(input);

            //Assert
            Console.WriteLine($"Password is [{password}]");
            Assert.AreEqual("d4cd2ee1", password, "Password mismtach");
        }
        [TestMethod]
        public void Test_GeneratePositionalPassword()
        {
            //Setup
            string input = "abc";

            //Action
            string password = Day5_Md5Hash.GeneratePositionalPassword(input);

            //Assert
            Assert.AreEqual("05ace8e3", password, "Password mismtach");
        }

        [TestMethod]
        public void Answer2_GeneratePositionalPassword()
        {
            //Setup
            string input = "ugkcyxxp";

            //Action
            string password = Day5_Md5Hash.GeneratePositionalPassword(input);

            //Assert
            Console.WriteLine($"New password is [{password}]");
            Assert.AreEqual("f2c730e5", password, "Password mismtach");
        }
    }
}