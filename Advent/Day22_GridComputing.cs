using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static Advent.Day13_Maze;

namespace Advent
{
    public class Day22_GridComputing
    {
        #region Static Variables...
        private static readonly Regex _PositionPattern = new Regex(@"node-x(\d+)-y(\d+)");
        private static readonly Regex _ValuePattern = new Regex(@"(\d+)T");
        private static readonly char[] _Space = new char[] { ' ' };
        #endregion Static Variables...

        #region CountGridCells
        public static int CountGridCells(string[] input)
        {
            return ParseNodes(input).Count;
        }
        #endregion CountGridCells

        #region ViableNodePairs
        public static int ViableNodePairs(string[] input)
        {
            int result = 0;
            var dataNodes = ParseNodes(input);
            result = dataNodes.Sum(current =>
            {
                return dataNodes.Count(node => node != current && current.Used > 0 && node.Available >= current.Used);
            });

            return result;
        }
        #endregion ViableNodePairs

        #region NumberOfCommandsToMoveData
        public static int NumberOfCommandsToMoveData(string[] input)
        {
            var list = ParseNodes(input);
            var grid = new Grid(ConvertToGrid(list));
            
            var goalNode = grid[grid.GetLength(0) - 1, 0];
            return grid.MoveEmptyNodeToGoal(goalNode);
        }
        #endregion NumberOfCommandsToMoveData

        #region ConvertToGrid
        private static DataNode[,] ConvertToGrid(List<DataNode> nodes)
        {
            var last = nodes.Last();
            DataNode[,] grid = new DataNode[last.Position.X + 1, last.Position.Y + 1];
            foreach (var node in nodes)
            {
                grid[node.Position.X, node.Position.Y] = node;
            }
            return grid;
        }
        #endregion ConvertToGrid

        #region ParseNodes
        private static List<DataNode> ParseNodes(string[] input)
        {
            List<DataNode> nodes = new List<DataNode>();
            foreach (var row in input)
            {
                if (row.StartsWith("/dev/grid/node-"))
                {
                    string[] fields = row.Split(_Space, StringSplitOptions.RemoveEmptyEntries);
                    Match match = _PositionPattern.Match(fields[0]);
                    if (match.Success)
                    {
                        int x = int.Parse(match.Groups[1].Value);
                        int y = int.Parse(match.Groups[2].Value);
                        int size = ParseValue(fields[1]);
                        nodes.Add(new DataNode(x, y, size)
                        {
                            Used = ParseValue(fields[2])
                        });
                    }
                }
            }
            return nodes;
        }
        #endregion ParseNodes

        #region ParseValue
        private static int ParseValue(string field)
        {
            Match match = _ValuePattern.Match(field);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            else
            {
                throw new Exception($"Expected {field} to match value pattern");
            }
        }
        #endregion ParseValue

        #region PrintGrid
        public static void PrintGrid(Grid grid)
        {
            Console.Clear();

            var fileOutput = new StringBuilder();
            var output = new StringBuilder[]
            {
                new StringBuilder(),
                new StringBuilder(),
                new StringBuilder(),
                new StringBuilder(),
                new StringBuilder(),
            };
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            var goalNode = grid.First(node => node.HasDataWeWant);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var node = grid[x, y];
                    var isLastX = x == (width - 1);
                    var isLastY = y == (height - 1);

                    if (y == 0)
                    {
                        output[0].Append((x == 0) ? '╔' : '╦');
                        output[0].Append("═");
                        if (isLastX) output[0].Append('╗');
                    }
                    else
                    {
                        output[0].Append((x == 0) ? '╠' : '╬');
                        output[0].Append("═");
                        if (isLastX) output[0].Append('╣');
                    }
                    
                    output[1].Append("║");
                    //if (canBeMovedFrom) output[1].Append("<span class=\"moved-from\">");
                    if (x == 0 && y == 0 && !node.HasDataWeWant)
                    {
                        output[1].Append("@");
                    }
                    else if (node.Used == 0)
                    {
                        output[1].Append(' ');
                    }
                    else if (node == goalNode)
                    {
                        output[1].Append('*');
                    }
                    else if (node.Used <= 100)
                    {
                        output[1].Append('.');
                    }
                    else if (node.Used > 100)
                    {
                        output[1].Append('#');
                    }
                    //output[1].Append($"{node.Used,3}");
                    //if (canBeMovedFrom) output[1].Append("</span>");
                    if (isLastX) output[1].Append('║');

                    //output[2].Append((x == 0) ? '╟' : '╫');
                    //output[2].Append("─");
                    //if (isLastX) output[2].Append('╢');

                    //output[3].Append("║");
                    //if (canBeMovedTo) output[3].Append("<span class=\"moved-to\">");
                    //output[3].Append($"{node.Size,3}");
                    //if (canBeMovedTo) output[3].Append("</span>");
                    //if (isLastX) output[3].Append('║');

                    if (isLastY)
                    {
                        output[4].Append((x == 0) ? '╚' : '╩');
                        output[4].Append("═");
                        if (isLastX) output[4].Append('╝');
                    }
                }

                foreach (var row in output)
                {
                    if (row.Length > 0)
                    {
                        Console.WriteLine(row);
                        row.Clear();
                    }
                }
            }
        }
        #endregion PrintGrid

        #region class DataNode
        public class DataNode
        {
            public Coordinate Position { get; private set; }

            public int Size { get; private set; }

            public int Used { get; set; }

            public bool HasDataWeWant { get; set; }

            public int Available
            {
                get { return Size - Used; }
            }

            public DataNode(DataNode node)
            {
                Position = node.Position;
                Size = node.Size;
                Used = node.Used;
                HasDataWeWant = node.HasDataWeWant;
            }

            public DataNode(int x, int y, int size)
            {
                Position = new Coordinate(x, y);
                Size = size;
            }

            public override bool Equals(object obj)
            {
                return Position.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }

            public override string ToString()
            {
                return $"Node-x{Position.X}-y{Position.Y} [Used: {Used}; Available: {Available}{(HasDataWeWant ? "; **Valuable**" : "")}]";
            }
        }
        #endregion class DataNode

        #region class Movement
        public class Movement
        {
            public Coordinate FromPosition { get; private set; }

            public Coordinate ToPosition { get; private set; }

            public Movement(Coordinate from, Coordinate to)
            {
                FromPosition = from;
                ToPosition = to;
            }

            public override string ToString()
            {
                return $"\tMoving from {ToPosition} to {FromPosition}";
            }
        }
        #endregion class Movement

        #region class Grid
        public class Grid : IEnumerable<DataNode>
        {
            private DataNode[,] _OriginalGrid = null;

            public DataNode this[int x, int y]
            {
                get { return _OriginalGrid[x, y]; }
            }

            public DataNode this[Coordinate position]
            {
                get { return _OriginalGrid[position.X, position.Y]; }
            }

            public Grid(DataNode[,] grid)
            {
                _OriginalGrid = grid;
            }

            public IEnumerator<DataNode> GetEnumerator()
            {
                return _OriginalGrid.Cast<DataNode>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int GetLength(int dimension)
            {
                return _OriginalGrid.GetLength(dimension);
            }

            #region MoveEmptyNodeToGoal
            public int MoveEmptyNodeToGoal(DataNode goal)
            {
                goal.HasDataWeWant = true;

                var startingNode = this[0, 0];
                var emptyNode = this.First(node => node.Used == 0);
                var moves = 0;

                while (!startingNode.HasDataWeWant)
                {
                    var nodeToMoveDataFrom = GetNext(emptyNode, goal);
                    if (nodeToMoveDataFrom != null)
                    {
                        emptyNode.Used += nodeToMoveDataFrom.Used;
                        emptyNode.HasDataWeWant = nodeToMoveDataFrom.HasDataWeWant;
                        if(emptyNode.HasDataWeWant)
                        {
                            goal = emptyNode;
                        }

                        emptyNode = nodeToMoveDataFrom;
                        nodeToMoveDataFrom.Used = 0;
                        nodeToMoveDataFrom.HasDataWeWant = false;
                        moves++;
                        PrintGrid(this);
                        Thread.Sleep(200);
                    }
                    else
                    {
                        throw new Exception($"Expected to be able to move to another node from {emptyNode}");
                    }
                }

                return moves;
            }
            #endregion MoveEmptyNodeToGoal

            #region GetNext
            internal DataNode GetNext(DataNode emptyNode, DataNode goal)
            {
                var position = emptyNode.Position;
                var goalPosition = goal.Position;

                if (position.Y > 0)
                {
                    bool moveToTop = goalPosition.X > position.X;
                    if (moveToTop && this[position.X, position.Y - 1].Used < emptyNode.Size)
                    {
                        return this[position.X, position.Y - 1];
                    }

                    if (position.X > 0 && this[position.X - 1, position.Y].Used < emptyNode.Size)
                    {
                        return this[position.X - 1, position.Y];
                    }
                    else
                    {
                        return this[position.X, position.Y - 1];
                    }
                }
                else if (position.Y == 0) // position is in first row;
                {
                    bool moveToRight = goalPosition.X > position.X;
                    if (moveToRight && position.X < (GetLength(0) - 1) && this[position.X + 1, position.Y].Used < emptyNode.Size)
                    {
                        return this[position.X + 1, position.Y];
                    }

                    if (this[position.X, position.Y + 1].Used < emptyNode.Size)
                    {
                        return this[position.X, position.Y + 1];
                    }
                }

                return null;
            }
            #endregion GetNext

            #region GetSurroundingNodes
            public LinkedList<Coordinate> GetSurroundingNodes(Coordinate position)
            {
                LinkedList<Coordinate> list = new LinkedList<Coordinate>();
                if (position.X > 0)
                {
                    list.AddLast(new Coordinate(position.X - 1, position.Y));
                }

                if (position.X < (GetLength(0) - 1))
                {
                    list.AddLast(new Coordinate(position.X + 1, position.Y));
                }

                if (position.Y > 0)
                {
                    list.AddLast(new Coordinate(position.X, position.Y - 1));
                }

                if (position.Y < (GetLength(1) - 1))
                {
                    list.AddLast(new Coordinate(position.X, position.Y + 1));
                }
                return list;
            }
            #endregion GetSurroundingNodes
        }
        #endregion class Grid
    }
}
