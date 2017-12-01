using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Properties;
using static Advent.Day15_Timing;

namespace Advent.Tests
{
    [TestClass()]
    public class Day15_TimingTests
    {
        static readonly string[] _SampleInput =
        {
                "Disc #1 has 5 positions; at time=0, it is at position 4.",
                "Disc #2 has 2 positions; at time=0, it is at position 1."
        };

        [TestMethod]
        public void Test_ParseInput()
        {
            //Setup
            string[] input = _SampleInput;

            //Action
            var discs = Day15_Timing.ParseInput(input);

            //Assert
            Assert.AreEqual(2, discs.Count());
            Assert.AreEqual(1, discs[0].Index, "disc 0 index");
            Assert.AreEqual(4, discs[0].CurrentPosition, "disc 0 current position");
            Assert.AreEqual(2, discs[1].Index, "disc 1 index");
            Assert.AreEqual(1, discs[1].CurrentPosition, "disc 1 current position");
        }

        [TestMethod]
        public void Test_TimeToTriggerCapsule()
        {
            //Setup
            string[] input = _SampleInput;

            //Action
            int time = Day15_Timing.TimeToTriggerCapsule(input);

            //Assert
            Assert.AreEqual(5, time);
        }

        [TestMethod]
        public void Answer1_TimeToTriggerCapsule()
        {
            //Setup
            string[] input = Resources.Day15_Input.Split('\n');


            //Action
            int time = Day15_Timing.TimeToTriggerCapsule(input);

            //Assert
            Assert.AreEqual(203660, time);
        }

        [TestMethod]
        public void Answer2_TimeToTriggerCapsule()
        {
            //Setup
            string[] input = Resources.Day15_Input.Split('\n');
            Disc extraDisc = new Disc(7, 11, 0);

            //Action
            int time = Day15_Timing.TimeToTriggerCapsule(input, extraDisc);

            //Assert
            Assert.AreEqual(2408135, time);
        }
    }
}