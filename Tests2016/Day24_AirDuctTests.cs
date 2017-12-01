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
    public class Day24_AirDuctTests
    {

        string[] SampleInput =
        {
            "###########",
            "#0.1.....2#",
            "#.#######.#",
            "#4.......3#",
            "###########"
        };

        [TestMethod]
        public void Test_StepsThroughMaze()
        {
            //Setup
            string[] input = SampleInput;
            int expected = 14;

            //Action
            int result = Day24_AirDuct.StepsThroughMaze(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer1_StepsThroughMaze()
        {
            //Setup
            string[] input = Resources.Day24_Input.Split('\n');
            int expected = 474;

            //Action
            int result = Day24_AirDuct.StepsThroughMaze(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer2_StepsThroughMaze()
        {
            //Setup
            string[] input = Resources.Day24_Input.Split('\n');
            int expected = 696;

            //Action
            int result = Day24_AirDuct.StepsThroughMazeBackToBeginning(input);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}