using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass]
    public class Day22_GridComputingTests
    {
        [TestMethod]
        public void Test_GridCount_TestInput()
        {
            //Setup
            string[] input = Resources.Day22_TestInput.Split('\n');
            int expected = 20;

            //Action
            int result = Day22_GridComputing.CountGridCells(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_GridCount_Input()
        {
            //Setup
            string[] input = Resources.Day22_Input.Split('\n');
            int expected = 875;

            //Action
            int result = Day22_GridComputing.CountGridCells(input);

            //Assert
            if (result == 1)
            {
                Console.WriteLine($"There is 1 viable node pair");
            }
            else
            {
                Console.WriteLine($"There are [{result}] viable node pairs");
            }
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Test_ViableNodePairs()
        {
            //Setup
            string[] input = Resources.Day22_TestInput.Split('\n');
            int expected = 41;

            //Action
            int result = Day22_GridComputing.ViableNodePairs(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer1_ViableNodePairs()
        {
            //Setup
            string[] input = Resources.Day22_Input.Split('\n');
            int expected = 860;

            //Action
            int result = Day22_GridComputing.ViableNodePairs(input);

            //Assert
            if (result == 1)
            {
                Console.WriteLine($"There is 1 viable node pair");
            }
            else
            {
                Console.WriteLine($"There are [{result}] viable node pairs");
            }
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_NumberOfCommandsToMoveData()
        {
            //Setup
            string[] input = Resources.Day22_Part2.Split('\n');
            int expected = 7;

            //Action
            int result = Day22_GridComputing.NumberOfCommandsToMoveData(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer2_NumberOfCommandsToMoveData()
        {
            //Setup
            string[] input = Resources.Day22_Input.Split('\n');
            int expected = 200;

            //Action
            int result = Day22_GridComputing.NumberOfCommandsToMoveData(input);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}