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
    public class Day18_RogueTests
    {
        string[] SmallSample =
        {
            "..^^.",
            ".^^^^",
            "^^..^"
        };

        string[] LargeSample =
        {
            ".^^.^.^^^^",
            "^^^...^..^",
            "^.^^.^.^^.",
            "..^^...^^^",
            ".^^^^.^^.^",
            "^^..^.^^..",
            "^^^^..^^^.",
            "^..^^^^.^^",
            ".^^^..^.^^",
            "^^.^^^..^^"
        };

        [TestMethod]
        public void Test_NextRow_Small_0()
        {
            //Setup
            string input = SmallSample[0];
            string expected = SmallSample[1];

            //Action
            string result = Day18_Rogue.NextRow(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong row after {input}");
        }

        [TestMethod]
        public void Test_NextRow_Small_1()
        {
            //Setup
            string input = SmallSample[1];
            string expected = SmallSample[2];

            //Action
            string result = Day18_Rogue.NextRow(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong row after {input}");
        }

        [TestMethod]
        public void Test_NextRow_Large_0()
        {
            //Setup
            string input = LargeSample[0];
            string expected = LargeSample[1];

            //Action
            string result = Day18_Rogue.NextRow(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong row after {input}");
        }

        [TestMethod]
        public void Test_NextRow_Large_1()
        {
            //Setup
            string input = LargeSample[1];
            string expected = LargeSample[2];

            //Action
            string result = Day18_Rogue.NextRow(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong row after {input}");
        }

        [TestMethod]
        public void Test_NextRow_Large_4()
        {
            //Setup
            string input = LargeSample[4];
            string expected = LargeSample[5];

            //Action
            string result = Day18_Rogue.NextRow(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong row after {input}");
        }

        [TestMethod]
        public void Test_NextRow_Large_All()
        {
            //Setup
            string input = LargeSample[0];
            int numberOfRows = 10;

            //Action
            var result = Day18_Rogue.GetTilesOnFloor(input, numberOfRows);

            for (int i = 0; i < result.Count(); i++)
            {
                //Assert
                Assert.AreEqual(LargeSample[i], result[i], $"Input of row {i} produced the wrong output");
            }
        }

        [TestMethod]
        public void Test_NumberOfSafeTiles_LargeSample()
        {
            //Setup
            string input = LargeSample[0];
            int numberOfRows = 10;
            int expected = 38;

            //Action
            int result = Day18_Rogue.NumberOfSafeTiles(input, numberOfRows);

            //Assert
            Console.WriteLine(LargeSample.Sum(row => row.Count(c => c == '.')));
            Assert.AreEqual(expected, result, $"Wrong number of safe tiles for {input}");
        }

        [TestMethod]
        public void Answer1_NumberOfSafeTiles()
        {
            //Setup
            string input = Resources.Day18_Input.Trim();
            int numberOfRows = 40;
            int expected = 1913;

            //Action
            int result = Day18_Rogue.NumberOfSafeTiles(input, numberOfRows);

            //Assert
            Console.WriteLine($"There are {result} safe tiles in the room with {numberOfRows} rows");
            Assert.AreEqual(expected, result, $"Wrong number of safe tiles for {input}");
        }

        [TestMethod]
        public void Answer2_NumberOfSafeTiles()
        {
            //Setup
            string input = Resources.Day18_Input.Trim();
            int numberOfRows = 400000;
            int expected = 19993564;

            //Action
            int result = Day18_Rogue.NumberOfSafeTiles(input, numberOfRows);

            //Assert
            Console.WriteLine($"There are {result} safe tiles in the room with {numberOfRows} rows");
            Assert.AreEqual(expected, result, $"Wrong number of safe tiles for {input}");
        }
    }
}