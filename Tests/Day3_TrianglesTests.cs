using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass]
    public class Day3_TrianglesTests
    {
        [TestMethod]
        public void Test_IsValidTriangle_354()
        {
            //Setup
            int[] triangle = { 3, 5, 4 };

            //Action
            bool isValid = Day3_Triangles.IsValidTriangle(triangle);

            //Assert
            Assert.IsTrue(isValid, $"{triangle} should be a triangle");
        }

        [TestMethod]
        public void Test_IsInvalidTriangle_51025()
        {
            //Setup
            int[] triangle = { 5, 10, 25 };

            //Action
            bool isValid = Day3_Triangles.IsValidTriangle(triangle);

            //Assert
            Assert.IsFalse(isValid, $"{triangle} shouldn't be a triangle");
        }

        [TestMethod]
        public void Test_IsInvalidTriangle_510()
        {
            //Setup
            int[] triangle = { 5, 10, 0 };

            //Action
            bool isValid = Day3_Triangles.IsValidTriangle(triangle);

            //Assert
            Assert.IsFalse(isValid, $"{triangle} shouldn't be a triangle");
        }

        [TestMethod]
        public void Test_IsInvalidTriangle_15105()
        {
            //Setup
            int[] triangle = { 15, 10, 5 };

            //Action
            bool isValid = Day3_Triangles.IsValidTriangle(triangle);

            //Assert
            Assert.IsFalse(isValid, $"{triangle} shouldn't be a triangle");
        }

        [TestMethod]
        public void Test_CountValidTriangles()
        {
            //Setup
            string[] input = {
                "  541  588  421",
                "  3    5    4",
                "15 10 5",
                "6 8 10"
            };

            //Action
            int validCount = Day3_Triangles.CountValidTriangles(input);

            //Assert
            Assert.AreEqual(3, validCount, "mismatch count");
        }

        [TestMethod]
        public void Answer_CountValidTriangles()
        {
            //Setup
            string[] input = Resources.Day3_Input.Split('\n');

            //Action
            int validCount = Day3_Triangles.CountValidTriangles(input);

            //Assert
            Console.WriteLine($"There are [{validCount}] valid triangles on the walls.");
            Assert.AreEqual(993, validCount, "mismatch count");
        }

        [TestMethod]
        public void Test_CountValidVerticalTriangles()
        {
            //Setup
            string[] input = {
                "101 301 501",
                "102 302 502",
                "103 303 503",
                "201 401 601",
                "202 402 602",
                "203 403 603"
            };

            //Action
            int validCount = Day3_Triangles.CountValidVerticalTriangles(input);

            //Assert
            Assert.AreEqual(6, validCount, "mismatch count");
        }

        [TestMethod]
        public void Answer_CountValidVerticalTriangles()
        {
            //Setup
            string[] input = Resources.Day3_Input.Split('\n');

            //Action
            int validCount = Day3_Triangles.CountValidVerticalTriangles(input);

            //Assert
            Console.WriteLine($"There are [{validCount}] valid triangles on the walls.");
            Assert.AreEqual(1849, validCount, "mismatch count");
        }
    }
}