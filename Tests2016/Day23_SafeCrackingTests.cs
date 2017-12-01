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
    [TestClass()]
    public class Day23_SafeCrackingTests
    {
        private string[] SampleInput =
        {
            "cpy 2 a",
            "tgl a",
            "tgl a",
            "tgl a",
            "cpy 1 a",
            "dec a",
            "dec a"
        };

        [TestMethod]
        public void Test_CodeForTheSafe()
        {
            //Setup
            string[] input = SampleInput;
            int expected = 3;

            //Action
            int result = Day23_SafeCracking.CodeForTheSafe(input, 2);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer1_CodeForTheSafe()
        {
            //Setup
            string[] input = Resources.Day23_Input.Split('\n');
            int expected = 13050;

            //Action
            int result = Day23_SafeCracking.CodeForTheSafe(input);

            //Assert
            Console.WriteLine($"The code sent to the safe is [{result}]");
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer2_CodeForTheSafe()
        {
            //Setup
            string[] input = Resources.Day23_Input.Split('\n');
            int expected = 479009610; // takes about 10 minutes unoptimized.

            //Action
            int result = Day23_SafeCracking.CodeForTheSafe(input, 12);

            //Assert
            Console.WriteLine($"The code sent to the safe is [{result}]");
            Assert.AreEqual(expected, result);
        }
    }
}