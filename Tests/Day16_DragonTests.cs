using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Tests
{
    [TestClass]
    public class Day16_DragonTests
    {
        [TestMethod]
        // 1 becomes 100.
        public void Test_Process_1()
        {
            //Setup
            string input = "1";
            string expected = "100";

            //Action
            string result = Day16_Dragon.ProcessDragonExtensionAsString(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        // 0 becomes 001.
        public void Test_Process_0()
        {
            //Setup
            string input = "0";
            string expected = "001";

            //Action
            string result = Day16_Dragon.ProcessDragonExtensionAsString(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        // 11111 becomes 11111000000.
        public void Test_Process_11111()
        {
            //Setup
            string input = "11111";
            string expected = "11111000000";

            //Action
            string result = Day16_Dragon.ProcessDragonExtensionAsString(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        // 111100001010 becomes 1111000010100101011110000.
        public void Test_Process_111100001010()
        {
            //Setup
            string input = "111100001010";
            string expected = "1111000010100101011110000";

            //Action
            string result = Day16_Dragon.ProcessDragonExtensionAsString(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_Checksum()
        {
            //Setup
            string input = "110010110100";
            string expectedResult = "100";

            //Action
            string checksum = Day16_Dragon.Checksum(input);

            //Assert
            Assert.AreEqual(expectedResult, checksum);
        }

        [TestMethod]
        public void Test_GetChecksumForLength()
        {
            //Setup
            string input = "10000";
            int length = 20;
            string expectedResult = "01100";

            //Action
            string checksum = Day16_Dragon.GetChecksumForLength(input, length);

            //Assert
            Assert.AreEqual(expectedResult, checksum);
        }

        [TestMethod]
        public void Answer1_GetChecksumForLength()
        {
            //Setup
            string input = "11101000110010100";
            int length = 272;
            string expectedResult = "10100101010101101";

            //Action
            string checksum = Day16_Dragon.GetChecksumForLength(input, length);

            //Assert
            Assert.AreEqual(expectedResult, checksum);
        }

        [TestMethod]
        public void Answer2_GetChecksumForLength()
        {
            //Setup
            string input = "11101000110010100";
            int length = 35651584;
            string expectedResult = "01100001101101001";

            //Action
            string checksum = Day16_Dragon.GetChecksumForLength(input, length);

            //Assert
            Assert.AreEqual(expectedResult, checksum);
        }
    }
}