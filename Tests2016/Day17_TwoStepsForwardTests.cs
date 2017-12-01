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
    public class Day17_TwoStepsForwardTests
    {
        [TestMethod]
        public void Test_PathToVault_ihgpwlah()
        {
            //Setup
            string input = "ihgpwlah";
            string expectedResult = "DDRRRD";

            //Action
            string result = Day17_TwoStepsForward.PathToVault(input);

            //Assert
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }

        [TestMethod]
        public void Test_PathToVault_kglvqrro()
        {
            //Setup
            string input = "kglvqrro";
            string expectedResult = "DDUDRLRRUDRD";

            //Action
            string result = Day17_TwoStepsForward.PathToVault(input);

            //Assert
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }

        [TestMethod]
        public void Test_PathToVault_ulqzkmiv()
        {
            //Setup
            string input = "ulqzkmiv";
            string expectedResult = "DRURDRUDDLLDLUURRDULRLDUUDDDRR";

            //Action
            string result = Day17_TwoStepsForward.PathToVault(input);

            //Assert
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }

        [TestMethod]
        public void Answer1_PathToVault_pxxbnzuo()
        {
            //Setup
            string input = "pxxbnzuo";
            string expectedResult = "RDULRDDRRD";

            //Action
            string result = Day17_TwoStepsForward.PathToVault(input);

            //Assert
            Console.WriteLine($"The path to reach the value is [{result}].");
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }
        [TestMethod]
        public void Test_LongestPathToVault_ihgpwlah()
        {
            //Setup
            string input = "ihgpwlah";
            int expectedResult = 370;

            //Action
            int result = Day17_TwoStepsForward.LongestPathToVault(input);

            //Assert
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }

        [TestMethod]
        public void Test_LongestPathToVault_kglvqrro()
        {
            //Setup
            string input = "kglvqrro";
            int expectedResult = 492;

            //Action
            int result = Day17_TwoStepsForward.LongestPathToVault(input);

            //Assert
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }

        [TestMethod]
        public void Test_LongestPathToVault_ulqzkmiv()
        {
            //Setup
            string input = "ulqzkmiv";
            int expectedResult = 830;

            //Action
            int result = Day17_TwoStepsForward.LongestPathToVault(input);

            //Assert
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }

        [TestMethod]
        public void Answer2_LongestPathToVault_pxxbnzuo()
        {
            //Setup
            string input = "pxxbnzuo";
            int expectedResult = 752;

            //Action
            int result = Day17_TwoStepsForward.LongestPathToVault(input);

            //Assert
            Console.WriteLine($"The path to reach the value is [{result}].");
            Assert.AreEqual(expectedResult, result, $"Path for {input} was wrong");
        }
    }
}