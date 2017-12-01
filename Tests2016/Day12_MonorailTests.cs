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
    public class Day12_MonorailTests
    {
        [TestMethod]
        public void Test_ProcessInstructions()
        {
            //Setup
            string[] input =
            {
                "cpy 41 a",
                "inc a",
                "inc a",
                "dec a",
                "jnz a 2",
                "dec a"
            };

            //Action
            AssemBunny assembler = Day12_Monorail.ProcessInstructions(input);

            //Assert
            Console.WriteLine(assembler);
            Assert.AreEqual(42, assembler.Registers['a']);
        }

        [TestMethod]
        public void Answer1_ProcessInstructions()
        {
            //Setup
            string[] input = Resources.Day12_Input.Split('\n');

            //Action
            AssemBunny assembler = Day12_Monorail.ProcessInstructions(input);

            //Assert
            Console.WriteLine(assembler);
            Assert.AreEqual(318007, assembler.Registers['a']);
        }

        [TestMethod]
        public void Answer2_ProcessInstructions()
        {
            //Setup
            string[] input = Resources.Day12_Input.Split('\n');
            Dictionary<char, int> startingValues = new Dictionary<char, int>()
            {
                { 'c', 1 }
            };

            //Action
            AssemBunny assembler = Day12_Monorail.ProcessInstructions(input, startingValues);

            //Assert
            Console.WriteLine(assembler);
            Assert.AreEqual(9227661, assembler.Registers['a']);
        }
    }
}