using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Properties;
using static Advent.Day21_Scrambling;

namespace Advent.Tests
{
    [TestClass()]
    public class Day21_ScramblingTests
    {
        string[] SampleInput =
        {
            "swap position 4 with position 0", 
            "swap letter d with letter b",
            "reverse positions 0 through 4",
            "rotate left 1 step",
            "move position 1 to position 4",
            "move position 3 to position 0",
            "rotate based on position of letter b",
            "rotate based on position of letter d",
        };

        string[] Passwords =
        {
            "abcde",
            "ebcda",
            "edcba",
            "abcde",
            "bcdea",
            "bdeac",
            "abdec",
            "ecabd",
            "decab",
        };

        [TestMethod]
        public void Test_ScramblePassword_1()
        {
            //Setup
            int index = 1;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_2()
        {
            //Setup
            int index = 2;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_3()
        {
            //Setup
            int index = 3;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_4()
        {
            //Setup
            int index = 4;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_5()
        {
            //Setup
            int index = 5;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_6()
        {
            //Setup
            int index = 6;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_7()
        {
            //Setup
            int index = 7;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_8()
        {
            //Setup
            int index = 8;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index - 1];
            string expected = Passwords[index];

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_ScramblePassword_All()
        {
            //Setup
            string[] action = SampleInput;
            string password = Passwords.First();
            string expected = Passwords.Last();

            //Action
            string result = Day21_Scrambling.ScramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"All actions should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_1()
        {
            //Setup
            int index = 1;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_2()
        {
            //Setup
            int index = 2;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_3()
        {
            //Setup
            int index = 3;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_4()
        {
            //Setup
            int index = 4;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_5()
        {
            //Setup
            int index = 5;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_6()
        {
            //Setup
            int index = 6;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_7()
        {
            //Setup
            int index = 7;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_8()
        {
            //Setup
            int index = 8;
            string[] action = SampleInput.Skip(index - 1).Take(1).ToArray();
            string password = Passwords[index];
            string expected = Passwords[index - 1];

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"{action[0]} should convert {password} to {expected}");
        }

        [TestMethod]
        public void Test_DescramblePassword_All()
        {
            //Setup
            string[] action = SampleInput;
            string password = Passwords.Last();
            string expected = Passwords.First();

            //Action
            string result = Day21_Scrambling.DescramblePassword(action, password);

            //Assert
            Assert.AreEqual(expected, result, $"All actions should convert {password} to {expected}");
        }

        [TestMethod]
        public void Reverse_RotateBasedOnPosition_0()
        {
            //Setup
            string input = "habcdefg";
            string expected = "abcdefgh";
            Descrambler descrambler = new Descrambler(input, reverse: true);

            //Action
            descrambler.RotateBasedOnPosition(expected[0]);
            string result = descrambler.Password;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Reverse_RotateBasedOnPosition_4()
        {
            //Setup
            string input = "cdefghab";
            string expected = "abcdefgh";
            Descrambler descrambler = new Descrambler(input, reverse: true);

            //Action
            descrambler.RotateBasedOnPosition(expected[4]);
            string result = descrambler.Password;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Reverse_RotateBasedOnPosition_7()
        {
            //Setup
            string input = "habcdefg";
            string expected = "abcdefgh";
            Descrambler descrambler = new Descrambler(input, reverse: true);

            //Action
            descrambler.RotateBasedOnPosition(expected[7]);
            string result = descrambler.Password;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Answer1_ScramblePassword()
        {
            //Setup
            string[] actions = Resources.Day21_Input.Split('\n');
            string password = "abcdefgh";
            string expected = "bfheacgd";

            //Action
            string result = Day21_Scrambling.ScramblePassword(actions, password);

            //Assert
            Console.WriteLine($"{password} unscrambles to [{result}]");
            Assert.AreEqual(expected, result, $"Actions should convert {password} to {expected}");
        }

        [TestMethod]
        public void Answer2_DescramblePassword()
        {
            //Setup
            string[] actions = Resources.Day21_Input.Split('\n');
            string password = "fbgdceah";
            string expected = "gcehdbfa";

            //Action
            string result = Day21_Scrambling.DescramblePassword(actions, password);

            //Assert
            Console.WriteLine($"{password} unscrambles to [{result}]");
            Assert.AreEqual(expected, result, $"Actions should convert {password} to {expected}");
        }
    }
}