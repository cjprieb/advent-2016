using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advent
{
    public class Day13_Maze
    {
        public static Maze CreateMaze(int favNumber)
        {
            return new Maze(favNumber);
        }

        public class Maze
        {
            private const char WALL = '#';
            private const char EMPTY = '.';

            #region Member Variables...
            private Dictionary<int, Dictionary<int, char>> _Layout = new Dictionary<int, Dictionary<int, char>>();
            private int _FavoriteNumber = 0;
            #endregion Member Variables...

            #region Constructors...

            #region Maze
            public Maze(int favoriteNumber)
            {
                _FavoriteNumber = favoriteNumber;
            }
            #endregion Maze

            #endregion Constructors...

            #region Methods...

            #region Build
            public void Build(int width, int height)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        GetCharAt(x, y);
                    }
                }
            }
            #endregion Build

            #region GetCharAt
            /// <summary>
            /// Find x*x + 3*x + 2*x* y + y + y* y.
            /// Add the office designer's favorite number (your puzzle input).
            /// Find the binary representation of that sum; count the number of bits that are 1.
            /// count the number of bits that are 1.
            /// If the number of bits that are 1 is even, it's an open space.
            /// If the number of bits that are 1 is odd, it's a wall.
            /// </summary>
            private char GetCharAt(int x, int y)
            {
                char result;
                if (_Layout.ContainsKey(x) && _Layout[x].ContainsKey(y))
                {
                    result = _Layout[x][y];
                }
                else
                {
                    long sum = (x * x) + (3 * x) + (2 * x * y) + (y) + (y * y) + (_FavoriteNumber);
                    int bitCount = Convert.ToString(sum, 2).Count(c => c == '1');
                    result = (bitCount % 2 == 0) ? EMPTY : WALL;
                    if (!_Layout.ContainsKey(x))
                    {
                        _Layout[x] = new Dictionary<int, char>();
                    }
                    _Layout[x][y] = result;
                }
                return result;
            }
            #endregion GetCharAt

            #region GetMazeAsString
            public string GetMazeAsString(List<Coordinate> path)
            {
                int width = _Layout.Any() ? _Layout.Keys.Max() : 0;
                int height = 0;
                if (_Layout.Any() && _Layout.Values.Any(dict => dict.Any()))
                {
                    height = _Layout.Values.Where(dict => dict.Any()).Max(dict => dict.Keys.Max());
                }
                StringBuilder builder = new StringBuilder();
                for (int y = 0; y <= height; y++)
                {
                    if (y != 0)
                    {
                        builder.Append(Environment.NewLine);
                    }
                    for (int x = 0; x <= width; x++)
                    {
                        Coordinate coord = new Coordinate(x, y);
                        if (path.Contains(coord))
                        {
                            builder.Append("O");
                        }
                        else
                        {
                            builder.Append(GetCharAt(x, y));
                        }
                    }
                }
                return builder.ToString();
            }
            #endregion GetMazeAsString

            #region NavigateToGoal
            public List<Coordinate> NavigateToGoal(Coordinate goal)
            {
                Dictionary<Coordinate, List<Coordinate>> distances = NavigateTo(goal:goal);                
                return distances.ContainsKey(goal) ? distances[goal] : null;
            }
            #endregion NavigateToGoal

            #region PointsWithin
            public List<Coordinate> PointsWithin(int distance)
            {
                Dictionary<Coordinate, List<Coordinate>> distances = NavigateTo(distance:distance);
                return distances
                    .Where(kvp => kvp.Value.Count() <= distance)
                    .Select(kvp => kvp.Key)
                    .ToList();
            }
            #endregion PointsWithin

            #region NavigateTo
            private Dictionary<Coordinate, List<Coordinate>> NavigateTo(Coordinate? goal = null, int? distance = null)
            {
                Dictionary<Coordinate, List<Coordinate>> distances = new Dictionary<Coordinate, List<Coordinate>>();
                List<Coordinate> queue = new List<Coordinate>();
                Coordinate start = new Coordinate(1, 1);

                queue.Add(start);
                distances[start] = new List<Coordinate>();

                while (queue.Any())
                {
                    Coordinate currentSpot = queue.First();
                    queue.Remove(currentSpot);

                    List<Coordinate> currentDistance = distances[currentSpot];

                    //PrintPath(currentDistance, goal);
                    //Thread.Sleep(50);

                    Coordinate[] newCoordinates =
                    {
                        new Coordinate(currentSpot.X - 1, currentSpot.Y), // moving left
                        new Coordinate(currentSpot.X + 1, currentSpot.Y), // moving right
                        new Coordinate(currentSpot.X, currentSpot.Y - 1), // moving up
                        new Coordinate(currentSpot.X, currentSpot.Y + 1)  // moving down
                    };

                    foreach (var newCoordinate in newCoordinates)
                    {
                        if (newCoordinate.IsValid() && 
                            GetCharAt(newCoordinate.X, newCoordinate.Y) != WALL)
                        {
                            if (!distances.ContainsKey(newCoordinate))
                            {
                                List<Coordinate> path = new List<Coordinate>();
                                path.AddRange(currentDistance);
                                path.Add(newCoordinate);
                                distances[newCoordinate] = path;

                                if ((goal.HasValue && !newCoordinate.Equals(goal)) ||
                                    (distance.HasValue && path.Count() < (distance.Value+10)))
                                {
                                    queue.Add(newCoordinate);
                                }
                            }
                        }
                    }
                }

                return distances;
            }
            #endregion NavigateTo

            #region PrintPath
            public void PrintPath(List<Coordinate> path, Coordinate? goal = null)
            {
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var columns in _Layout)
                {
                    int x = columns.Key;
                    foreach (var cell in columns.Value)
                    {
                        int y = cell.Key;
                        Console.CursorLeft = x;
                        Console.CursorTop = y;
                        Console.Write(GetCharAt(x, y));
                    }
                }

                if (goal.HasValue)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.CursorLeft = goal.Value.X;
                    Console.CursorTop = goal.Value.Y;
                    Console.Write('*');
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                for(int i = 0; i < path.Count(); i++)
                {
                    Coordinate cell = path[i];
                    Console.CursorLeft = cell.X;
                    Console.CursorTop = cell.Y;
                    if (i == path.Count() - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write('O');
                }
            }
            #endregion PrintPath

            #region SetCharAt
            private void SetCharAt(int x, int y, char c)
            {
                if (GetCharAt(x, y) != WALL)
                {
                    _Layout[x][y] = c;
                }
            }
            #endregion SetCharAt

            #region ToString
            public override string ToString()
            {
                int width = _Layout.Any() ? _Layout.Keys.Max() : 0;
                int height = 0;
                if (_Layout.Any() && _Layout.Values.Any(dict => dict.Any()))
                {
                    height = _Layout.Values.Where(dict => dict.Any()).Max(dict => dict.Keys.Max());
                }
                StringBuilder builder = new StringBuilder();
                for (int y = 0; y <= height; y++)
                {
                    if (y != 0)
                    {
                        builder.Append(Environment.NewLine);
                    }
                    for (int x = 0; x <= width; x++)
                    {
                        builder.Append(GetCharAt(x, y));
                    }
                }
                return builder.ToString();
            }
            #endregion ToString

            #endregion Methods...
        }

        public struct Coordinate
        {
            public int X;
            public int Y;
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool IsValid()
            {
                return X >= 0 && Y >= 0;
            }

            public override bool Equals(object obj)
            {
                if (obj is Coordinate)
                {
                    Coordinate coord = (Coordinate)obj;
                    return coord.X == X && coord.Y == Y;
                }
                else 
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return $"{X},{Y}".GetHashCode();
            }

            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }
    }
}
