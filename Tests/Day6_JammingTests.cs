using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;
using static Advent.Day6_Jamming;

namespace Advent.Tests
{
    [TestClass]
    public class Day6_JammingTests
    {
        [TestMethod]
        public void Test_ExtractErrorFreeMessage()
        {
            //Setup
            string[] input = {
                "eedadn",
                "drvtee",
                "eandsr",
                "raavrd",
                "atevrs",
                "tsrnev",
                "sdttsa",
                "rasrtv",
                "nssdts",
                "ntnada",
                "svetve",
                "tesnvt",
                "vntsnd",
                "vrdear",
                "dvrsen",
                "enarar"
            };

            //Action
            string message = Day6_Jamming.ExtractErrorFreeMessage(input);

            //Assert
            Assert.AreEqual("easter", message, "message mismatch");
        }

        [TestMethod]
        public void Answer1_ExtractErrorFreeMessage()
        {
            //Setup
            string[] input = Resources.Day6_Input.Split('\n');

            //Action
            string message = Day6_Jamming.ExtractErrorFreeMessage(input);

            //Assert
            Console.WriteLine($"Decoded message from Santa is [{message}]");
            Assert.AreEqual("asvcbhvg", message, "message mismatch");
        }
        [TestMethod]
        public void Test_ExtractErrorFreeMessage_Modified()
        {
            //Setup
            string[] input = {
                "eedadn",
                "drvtee",
                "eandsr",
                "raavrd",
                "atevrs",
                "tsrnev",
                "sdttsa",
                "rasrtv",
                "nssdts",
                "ntnada",
                "svetve",
                "tesnvt",
                "vntsnd",
                "vrdear",
                "dvrsen",
                "enarar"
            };

            //Action
            string message = Day6_Jamming.ExtractErrorFreeMessage(input, RepetitionCodeType.LeastLikely);

            //Assert
            Assert.AreEqual("advent", message, "message mismatch");
        }

        [TestMethod]
        public void Answer2_ExtractErrorFreeMessage_Modified()
        {
            //Setup
            string[] input = Resources.Day6_Input.Split('\n');

            //Action
            string message = Day6_Jamming.ExtractErrorFreeMessage(input, RepetitionCodeType.LeastLikely);

            //Assert
            Console.WriteLine($"Decoded message from Santa is [{message}]");
            Assert.AreEqual("odqnikqv", message, "message mismatch");
        }
    }
}