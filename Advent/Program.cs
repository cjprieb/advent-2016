using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent.Day13_Maze;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            Day13();

            Console.ReadKey();
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
