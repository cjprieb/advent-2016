using Advent2018.Day09;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018.Tests
{
    [TestClass]
    public class Day09Tests
    {
        [TestMethod]
        public void Solve1_9_Players()
        {
            var day = new Day();

            int players = 9;
            int max = 25;
            long expected = 32;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve1_10_Players()
        {
            var day = new Day();

            int players = 10;
            int max = 1618;
            long expected = 8317;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve1_13_Players()
        {
            var day = new Day();

            int players = 13;
            int max = 7999;
            long expected = 146373;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve1_17_Players()
        {
            var day = new Day();

            int players = 17;
            int max = 1104;
            long expected = 2764;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve1_21_Players()
        {
            var day = new Day();

            int players = 21;
            int max = 6111;
            long expected = 54718;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve1_30_Players()
        {
            var day = new Day();

            int players = 30;
            int max = 5807;
            long expected = 37305;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve1_Input()
        {
            var day = new Day();

            int players = 419;
            int max = 72164;
            long expected = 423717;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Solve2_Input()
        {
            var day = new Day();

            int players = 419;
            long max = 72164 * 100;
            //int expected = 741859099; // too  low
            long expected = 3553108197;

            long actual = day.solve1(players, max);
            Assert.AreEqual(expected, actual);
        }
    }
}
