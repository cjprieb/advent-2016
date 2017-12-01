using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;
using static Advent.Day08_TwoFactorAuthentication;

namespace Advent.Tests
{
    [TestClass]
    public class Day08_TwoFactorAuthenticationTests
    {

        [TestMethod]
        public void Test_Rectangle_3x2()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string expected = "###....\n" +
                              "###....\n" +
                              ".......";

            //Action
            painter.Rectangle(3, 2);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_RotateColumn_1by1()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string expected = "#.#....\n" +
                              "###....\n" +
                              ".#.....";

            //Action
            painter.Rectangle(3, 2);
            painter.RotateColumn(1, 1);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_RotateRow_0by4()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string expected = "....#.#\n" +
                              "###....\n" +
                              ".#.....";

            //Action
            painter.Rectangle(3, 2);
            painter.RotateColumn(1, 1);
            painter.RotateRow(0, 4);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_RotateColumn_1by2()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string expected = ".#..#.#\n" +
                              "###....\n" +
                              ".......";

            //Action
            painter.Rectangle(3, 2);
            painter.RotateColumn(1, 1);
            painter.RotateRow(0, 4);
            painter.RotateColumn(1, 2);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_ParseInstructions_3x2()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string[] instructions = { "rect 3x2" };
            string expected = "###....\n" +
                              "###....\n" +
                              ".......";

            //Action
            painter.ParseInstructions(instructions);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_ParseInstructions_1by1()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string[] instructions = { "rect 3x2", "rotate column x=1 by 1" };
            string expected = "#.#....\n" +
                              "###....\n" +
                              ".#.....";

            //Action
            painter.ParseInstructions(instructions);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_ParseInstructions_0by4()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string[] instructions = { "rect 3x2", "rotate column x=1 by 1", "rotate row y=0 by 4" };
            string expected = "....#.#\n" +
                              "###....\n" +
                              ".#.....";

            //Action
            painter.ParseInstructions(instructions);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Test_ParseInstructions_1by2()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(7, 3);
            string[] instructions = { "rect 3x2", "rotate column x=1 by 1", "rotate row y=0 by 4", "rotate column x=1 by 2" };
            string expected = ".#..#.#\n" +
                              "###....\n" +
                              ".......";

            //Action
            painter.ParseInstructions(instructions);

            //Assert
            Assert.AreEqual(expected, painter.Screen);
        }

        [TestMethod]
        public void Answer1_ParseInstructions()
        {
            //Setup
            ScreenPainter painter = new ScreenPainter(50, 6);
            string[] instructions = Resources.Day08_Input.Split('\n');

            //Action
            painter.ParseInstructions(instructions);

            //Assert
            Console.WriteLine(painter.Screen);
            Console.WriteLine($"[{painter.LitPixelsCount}] pixels are lit.");
            Assert.AreEqual(110, painter.LitPixelsCount, "pixel mismatch");
            //Screen says: ZJHRKCPLYJ
        }
    }
}