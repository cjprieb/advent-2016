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
    public class Day02_BathroomCodeTests
    {
        [TestMethod]
        public void Test_ParseInstructions()
        {
            //Setup
            string[] instructions =
            {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };

            //Action
            string code = Day02_BathroomCode.ParseInstructions(instructions);

            //Assert
            Assert.AreEqual("1985", code, "Code mismatch");
        }

        [TestMethod]
        public void Answer1_ParseInstructions()
        {
            //Setup
            string[] instructions = ParseFile(Resources.Day02_Input);

            //Action
            string code = Day02_BathroomCode.ParseInstructions(instructions);

            //Assert
            Console.WriteLine($"The code to the bathroom is [{code}].");
            Assert.AreEqual("73597", code, "Code mismatch");
        }
        [TestMethod]
        public void Test_ParseInstructions_Part2()
        {
            //Setup
            string[] instructions =
            {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            };

            //Action
            string code = Day02_BathroomCode.ParseInstructions_Part2(instructions);

            //Assert
            Assert.AreEqual("5DB3", code, "Code mismatch");
        }

        [TestMethod]
        public void Answer2_ParseInstructions()
        {
            //Setup
            string[] instructions = ParseFile(Resources.Day02_Input);

            //Action
            string code = Day02_BathroomCode.ParseInstructions_Part2(instructions);

            //Assert
            Console.WriteLine($"The real code to the bathroom with the improved code pad is [{code}].");
            Assert.AreEqual("A47DA", code, "Code mismatch");
        }

        private string[] ParseFile(string fileString)
        {
            return fileString.Split('\n')
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToArray();
        }
    }
}