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
    public class Day19_ElephantTests
    {
        [TestMethod]
        public void Test_ElfWhoGetsPresents()
        {
            //Setup
            int input = 5;
            int expected = 3;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        [TestMethod]
        public void Answer1_ElfWhoGetsPresents()
        {
            //Setup
            int input = 3012210;
            int expected = 1830117;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        //[TestMethod]
        public void Test_ElfWhoGetsPresents_Variation()
        {
            //Setup
            int input = 5;
            int expected = 3;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents_SideVariation(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        [TestMethod]
        public void Answer1_ElfWhoGetsPresents_Variation()
        {
            //Setup
            int input = 3012210;
            int expected = 1830117;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents_SideVariation(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        [TestMethod]
        public void Test_ElfWhoGetsPresents_Variation_5()
        {
            //Setup
            int input = 5;
            int expected = 2;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents_AcrossVariation(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        [TestMethod]
        public void Test_ElfWhoGetsPresents_Variation_6()
        {
            //Setup
            int input = 6;
            int expected = 3;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents_AcrossVariation(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        [TestMethod]
        public void Test_ElfWhoGetsPresents_Variation_20()
        {
            //Setup
            int input = 20;
            int expected = 13;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents_AcrossVariation(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }

        [TestMethod]
        public void Answer2_ElfWhoGetsPresents_Variation()
        {
            //Setup
            int input = 3012210;
            int expected = 1417887;

            //Action
            int result = Day19_Elephant.ElfWhoGetsPresents_AcrossVariation(input);

            //Assert
            Assert.AreEqual(expected, result, $"Wrong output with {input} elves");
        }
    }
}