using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent.Day13_Maze;

namespace Advent
{
    public static class Day24_AirDuct
    {
        public static int StepsThroughMaze(string[] input)
        {
            MazeGrid grid = new MazeGrid(false);
            grid.BuildFromInput(input);
            grid.BuildPathsBetweenNumbers();
            MazeCell start = grid.First(cell => cell.Number == 0);
            List<MazeCell> path = grid.PathToAllNumbers(start, new List<int>() { 0 });
            foreach (var cell in path)
            {
                Console.WriteLine(cell);
            }
            return path.Count;
        }

        public static int StepsThroughMazeBackToBeginning(string[] input)
        {
            MazeGrid grid = new MazeGrid(true);
            grid.BuildFromInput(input);
            grid.BuildPathsBetweenNumbers();
            MazeCell start = grid.First(cell => cell.Number == 0);
            List<MazeCell> path = grid.PathToAllNumbers(start, new List<int>() { 0 });
            foreach (var cell in path)
            {
                Console.WriteLine(cell);
            }
            return path.Count;
        }

        public class MazeGrid : IEnumerable<MazeCell>
        {
            private MazeCell[,] _grid = null;
            private List<MazeCell> _cellsWithNumbers;
            private bool _goBackToBeginning = false;

            public MazeCell this[int x, int y]
            {
                get { return _grid[x, y]; }
            }

            public MazeGrid(bool backToBeginning)
            {
                _goBackToBeginning = backToBeginning;
            }

            #region BuildFromInput
            public void BuildFromInput(string[] input)
            {
                int width = input[0].Trim().Length;
                int height = input.Length;

                _grid = new MazeCell[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        char value = input[y][x];
                        _grid[x, y] = new MazeCell(x, y, value);
                    }
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        MazeCell cell = _grid[x, y];
                        if (x > 0)
                        {
                            cell.Set(Direction.Left, _grid[x - 1, y]);
                        }
                        if (x < (width-1))
                        {
                            cell.Set(Direction.Right, _grid[x + 1, y]);
                        }
                        if (y > 0)
                        {
                            cell.Set(Direction.Up, _grid[x, y - 1]);
                        }
                        if (y < (height - 1))
                        {
                            cell.Set(Direction.Down, _grid[x, y + 1]);
                        }
                    }
                }
                _cellsWithNumbers = _grid.Cast<MazeCell>().Where(cell => cell.Number.HasValue).ToList();
            }
            #endregion BuildFromInput

            public void BuildPathsBetweenNumbers()
            {
                foreach (var starting in _cellsWithNumbers)
                {
                    foreach (var ending in _cellsWithNumbers)
                    {
                        if (starting.Position.Equals(ending.Position)) continue;

                        if (starting.GetPathTo(ending.Number.Value) == null)
                        {
                            var path = FindShortestPath(starting, ending);
                            starting.SetPathTo(ending.Number.Value, path);
                            ending.SetPathTo(starting.Number.Value, path.Reverse());
                        }
                    }
                }
            }

            public IEnumerable<MazeCell> FindShortestPath(MazeCell start, MazeCell end)
            {
                Dictionary<Coordinate, List<MazeCell>> placesVisited = new Dictionary<Coordinate, List<MazeCell>>();
                placesVisited[start.Position] = new List<MazeCell>();

                Queue<MazeCell> queue = new Queue<MazeCell>();
                queue.Enqueue(start);

                List<MazeCell> path = null;

                while (queue.Any())
                {
                    MazeCell current = queue.Dequeue();
                    List<MazeCell> currentPath = placesVisited[current.Position];
                    foreach (var neighbor in current.Neighbors)
                    {
                        if (!placesVisited.ContainsKey(neighbor.Position))
                        {
                            var newPath = new List<MazeCell>();
                            newPath.AddRange(currentPath);
                            newPath.Add(current);
                            placesVisited[neighbor.Position] = newPath;
                            
                            if (neighbor.Position.Equals(end.Position))
                            {
                                path = newPath;
                            }
                            else
                            {
                                queue.Enqueue(neighbor);
                            }
                        }
                    }
                }
                return path;
            }

            public List<MazeCell> PathToAllNumbers(MazeCell start, List<int> numbersVisited)
            {
                List<MazeCell> totalPath = null;

                if (numbersVisited.Count == _cellsWithNumbers.Count)
                {
                    if (_goBackToBeginning)
                    {
                        return new List<MazeCell>(start.GetPathTo(0));
                    }
                    else
                    {
                        return new List<MazeCell>();
                    }
                }
                else
                {
                    foreach (var cell in _cellsWithNumbers)
                    {
                        if (cell.Position.Equals(start.Position)) continue;

                        var cellNumber = cell.Number.Value;
                        if (!numbersVisited.Contains(cellNumber))
                        {
                            var newNumbersVisited = new List<int>(numbersVisited);
                            newNumbersVisited.Add(cellNumber);
                            var possiblePath = PathToAllNumbers(cell, newNumbersVisited);
                            possiblePath.AddRange(start.GetPathTo(cellNumber));
                            if (totalPath == null || totalPath.Count > possiblePath.Count)
                            {
                                totalPath = possiblePath;
                            }
                        }
                    }
                }
                return totalPath;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<MazeCell> GetEnumerator()
            {
                return _grid.Cast<MazeCell>().GetEnumerator();
            }
        }

        public class MazeCell
        {
            static readonly Direction[] _AllDirections =
            {
                Direction.Up, Direction.Down, Direction.Left, Direction.Right
            };

            Dictionary<Direction, MazeCell> _neighbors = new Dictionary<Direction, MazeCell>();
            Dictionary<int, IEnumerable<MazeCell>> _pathToNumberedCell = new Dictionary<int, IEnumerable<MazeCell>>();

            public MazeCell this[Direction dir]
            {
                get { return Get(dir); }
                set { Set(dir, value); }
            }

            public IEnumerable<MazeCell> Neighbors
            {
                get
                {
                    return _AllDirections.Select(dir => Get(dir)).Where(cell => cell != null && !cell.IsWall);
                }
            }

            public Coordinate Position { get; private set; }


            public bool IsWall { get; private set; }

            public int? Number { get; private set; }

            public MazeCell(int x, int y, char c)
            {
                IsWall = (c == '#');
                if (!IsWall && c != '.')
                {
                    try
                    {
                        Number = int.Parse(c.ToString());
                    }
                    catch (FormatException ex)
                    {
                        throw new Exception($"{c} is not a number", ex);
                    }
                }
                Position = new Coordinate(x, y);
            }

            public MazeCell Get(Direction direction)
            {
                if (_neighbors.ContainsKey(direction))
                {
                    return _neighbors[direction];
                }
                else
                {
                    return null;
                }
            }

            public IEnumerable<MazeCell> GetPathTo(int number)
            {
                if (_pathToNumberedCell.ContainsKey(number))
                {
                    return _pathToNumberedCell[number];
                }
                else
                {
                    return null;
                }
            }

            public void Set(Direction direction, MazeCell cell)
            {
                _neighbors[direction] = cell;
                Direction opposite = direction.Opposite();
                if (cell[opposite] != this)
                {
                    cell[opposite] = this;
                }
            }

            public void SetPathTo(int number, IEnumerable<MazeCell> path)
            {
                _pathToNumberedCell[number] = path;
            }

            public override string ToString()
            {
                return $"Cell ({Position}){(Number.HasValue ? " #"+ Number.Value.ToString() : "")}";
            }
        }

        public static Direction Opposite(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
            }
            return Direction.None;
        }

        public enum Direction
        {
            None, Up, Down, Left, Right
        }
    }
}
