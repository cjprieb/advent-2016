using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent.Properties;
using static Advent.Day13_Maze;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            Day22();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void Day22()
        {
            int count = Day22_GridComputing.NumberOfCommandsToMoveData(Resources.Day22_Input.Split('\n'));
            Console.WriteLine($"It took {count} moves to get the data");
        }

        static void Day13()
        {
            //Setup
            int favoriteNumber = 1350;

            //Action
            Maze maze = Day13_Maze.CreateMaze(favoriteNumber);
            maze.NavigateToGoal(new Coordinate(31, 39));
            //List<Coordinate> points = maze.PointsWithin(distance);
            //int count = points.Count();

            //Assert
            //Console.WriteLine($"{count} places could be reached in {distance} moves");
        }
    }
}
