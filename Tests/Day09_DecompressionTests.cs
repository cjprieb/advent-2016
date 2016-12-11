using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass]
    public class Day09_DecompressionTests
    {
        [TestMethod]
        public void Test_Decompress_ADVENT()
        {
            //Setup
            string input = "ADVENT";

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Assert.AreEqual("ADVENT", result);
        }

        [TestMethod]
        public void Test_Decompress_1x5()
        {
            //Setup
            string input = "A(1x5)BC";

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Assert.AreEqual("ABBBBBC", result);
        }

        [TestMethod]
        public void Test_Decompress_3x3()
        {
            //Setup
            string input = "(3x3)XYZ";

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Assert.AreEqual("XYZXYZXYZ", result);
        }

        [TestMethod]
        public void Test_Decompress_2x2_bcd_2x2()
        {
            //Setup
            string input = "A(2x2)BCD(2x2)EFG";

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Assert.AreEqual("ABCBCDEFEFG", result);
        }

        [TestMethod]
        public void Test_Decompress_6x1_1x3()
        {
            //Setup
            string input = "(6x1)(1x3)A";

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Assert.AreEqual("(1x3)A", result);
        }

        [TestMethod]
        public void Test_Decompress_8x2_3x3()
        {
            //Setup
            string input = "X(8x2)(3x3)ABCY";

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Assert.AreEqual("X(3x3)ABC(3x3)ABCY", result);
        }

        [TestMethod]
        public void Answer1_Decompress()
        {
            //Setup
            string input = Resources.Day09_Input.Trim();

            //Action
            string result = Day09_Decompression.Decompress(input);

            //Assert
            Console.WriteLine($"Length of decompressed string is [{result.Length}]");
            Console.WriteLine(result);
            Assert.AreEqual(99145, result.Length);
        }

        [TestMethod]
        public void Test_DecompressTwice_3x3()
        {
            //Setup
            string input = "(3x3)XYZ";

            //Action
            long size = Day09_Decompression.DecompressedLength(input);

            //Assert
            Assert.AreEqual("XYZXYZXYZ".Length, size);
        }

        [TestMethod]
        public void Test_DecompressTwice_ABCY()
        {
            //Setup
            string input = "X(8x2)(3x3)ABCY";

            //Action
            long size = Day09_Decompression.DecompressedLength(input);

            //Assert
            Assert.AreEqual("XABCABCABCABCABCABCY".Length, size);
        }

        [TestMethod]
        public void Test_DecompressTwice_5_markers()
        {
            //Setup
            string input = "(27x12)(20x12)(13x14)(7x10)(1x12)A";

            //Action
            long size = Day09_Decompression.DecompressedLength(input);

            //Assert
            Assert.AreEqual(241920, size);
        }

        [TestMethod]
        public void Test_DecompressTwice_SEVEN()
        {
            //Setup
            string input = "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN";

            //Action
            long size = Day09_Decompression.DecompressedLength(input);

            //Assert
            Assert.AreEqual(445, size);
        }

        [TestMethod]
        public void Answer2_Decompress()
        {
            //Setup
            string input = Resources.Day09_Input.Trim();

            //Action
            long size = Day09_Decompression.DecompressedLength(input);

            //Assert
            Console.WriteLine($"Length of decompressed string is [{size}]");
            Assert.AreEqual(10943094568, size);
        }
    }
}