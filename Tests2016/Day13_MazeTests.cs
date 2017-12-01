using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent.Day13_Maze;

namespace Advent.Tests
{
    [TestClass]
    public class Day13_MazeTests
    {
        [TestMethod]
        public void Test_CreateMaze()
        {
            //Setup
            int width = 10;
            int height = 7;
            int favoriteNumber = 10;

            //Action
            Maze maze = Day13_Maze.CreateMaze(favoriteNumber);
            maze.Build(width, height);

            //Assert
            string expectedOutput = ".#.####.##" + Environment.NewLine + 
                                    "..#..#...#" + Environment.NewLine +
                                    "#....##..." + Environment.NewLine +
                                    "###.#.###." + Environment.NewLine +
                                    ".##..#..#." + Environment.NewLine +
                                    "..##....#." + Environment.NewLine +
                                    "#...##.###";
            Console.WriteLine(maze);
            Assert.AreEqual(expectedOutput, maze.ToString());
        }


        [TestMethod]
        public void Test_CreateMaze_Actual()
        {
            //Setup
            int width = 50;
            int height = 50;
            int favoriteNumber = 1350;

            //Action
            Maze maze = Day13_Maze.CreateMaze(favoriteNumber);
            maze.Build(width, height);

            //Assert
            Console.WriteLine(maze);
            //Assert.AreEqual(expectedOutput, maze.ToString());
        }

        [TestMethod]
        public void Test_NavigateMaze()
        {
            //Setup
            int favoriteNumber = 10;
            Coordinate goal = new Coordinate(7,4);

            //Action
            Maze maze = Day13_Maze.CreateMaze(favoriteNumber);
            List<Coordinate> path = maze.NavigateToGoal(goal);
            int steps = path.Count();

            //Assert
            Console.WriteLine(maze.GetMazeAsString(path));
            Assert.AreEqual(11, steps);
        }

        [TestMethod]
        public void Answer_NavigateMaze()
        {
            //Setup
            int favoriteNumber = 1350;
            Coordinate goal = new Coordinate(31, 39);

            //Action
            Maze maze = Day13_Maze.CreateMaze(favoriteNumber);
            List<Coordinate> path = maze.NavigateToGoal(goal);
            int steps = path.Count();

            //Assert
            Console.WriteLine($"It took {steps} steps to get to {goal}");
            Console.WriteLine(maze.GetMazeAsString(path));
            Assert.AreEqual(92, steps);
        }

        [TestMethod]
        public void Answer_NavigateDistance()
        {
            //Setup
            int favoriteNumber = 1350;
            int distance = 50;

            //Action
            Maze maze = Day13_Maze.CreateMaze(favoriteNumber);
            List<Coordinate> points = maze.PointsWithin(distance);
            int count = points.Count();

            //Assert
            Console.WriteLine($"{count} places could be reached in {distance} moves");
            Assert.AreEqual(124, count);
        }
    }
}