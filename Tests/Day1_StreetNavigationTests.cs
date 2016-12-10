using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;
using static Advent.Day1_StreetNavigation;

namespace Advent.Tests
{
    [TestClass]
    public class Day1_StreetNavigationTests
    {
        [TestMethod]
        public void Test_BlocksAway_R2L3()
        {
            //Setup
            string input = "R2, L3";
            string[] inputAsList = ParseInput(input);

            //Action
            BlockPosition position = GetLocation(inputAsList);

            //Assert
            Assert.AreEqual(5, position.TotalBlocks, "Total block count mismatch");
        }

        [TestMethod]
        public void Test_BlocksAway_R2R2R2()
        {
            //Setup
            string input = "R2, R2, R2";
            string[] inputAsList = ParseInput(input);

            //Action
            BlockPosition position = GetLocation(inputAsList);

            //Assert
            Assert.AreEqual(2, position.TotalBlocks, "Total block count mismatch");
        }

        [TestMethod]
        public void Test_BlocksAway_R5L5R5R3()
        {
            //Setup
            string input = "R5, L5, R5, R3";
            string[] inputAsList = ParseInput(input);

            //Action
            BlockPosition position = GetLocation(inputAsList);

            //Assert
            Assert.AreEqual(12, position.TotalBlocks, "Total block count mismatch");
        }

        [TestMethod]
        public void Answer1_BlocksAway()
        {
            //Setup
            string input = Resources.Day1_Input;
            string[] inputAsList = ParseInput(input);

            //Action
            BlockPosition position = GetLocation(inputAsList);

            Console.WriteLine($"Headquarters is [{position.TotalBlocks}] blocks away.");
            Assert.AreEqual(307, position.TotalBlocks, "Total block count mismatch");
        }

        [TestMethod]
        public void Test_PlacesVisited_R8R4R4R8()
        {
            //Setup
            string input = "R8, R4, R4, R8";
            string[] inputAsList = ParseInput(input);

            //Action
            Point position = GetFirstLocationVisitedTwice(inputAsList);

            //Assert
            Assert.IsNotNull(position, "no place found");
            Assert.AreEqual(4, position.BlockDistanceFromCenter, "Total block count mismatch");
        }

        [TestMethod]
        public void Answer2_PlacesVisited()
        {
            //Setup
            string input = Resources.Day1_Input;
            string[] inputAsList = ParseInput(input);

            //Action
            Point position = GetFirstLocationVisitedTwice(inputAsList);

            Console.WriteLine($"Headquarters is [{position.BlockDistanceFromCenter}] blocks away.");
            Assert.AreEqual(165, position.BlockDistanceFromCenter, "Total block count mismatch");
        }

        private string[] ParseInput(string str)
        {
            return str.Replace(" ", "").Split(',');
        }
    }
}