using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static Advent.Day13_Maze;

namespace Advent
{
    public class Day17_TwoStepsForward
    {
        public static MD5 _Md5;

        public static string PathToVault(string passcode)
        {
            _Md5 = MD5.Create();
            BuildingLayout layout = SetupVault(4, 4, passcode);
            List<Direction> path = layout.Navigate();
            return string.Join("", path.Select(d => Format(d)));
        }

        public static int LongestPathToVault(string passcode)
        {
            _Md5 = MD5.Create();
            BuildingLayout layout = SetupVault(4, 4, passcode);
            List<Direction> path = layout.NavigateLongest();
            return path.Count;
        }

        public static BuildingLayout SetupVault(int width, int height, string passcode)
        {
            BuildingLayout layout = new BuildingLayout(width, height, passcode);

            foreach (var row in layout.Rooms)
            {
                foreach (var room in row)
                {
                    if (room.Y > 0)
                    {
                        room.Up = layout[room.X, room.Y - 1];
                    }
                    if (room.Y < height-1)
                    {
                        room.Down = layout[room.X, room.Y + 1];
                    }
                    if (room.X > 0)
                    {
                        room.Left = layout[room.X - 1, room.Y];
                    }
                    if (room.X < width - 1)
                    {
                        room.Right = layout[room.X + 1, room.Y];
                    }
                }
            }

            return layout;
        }

        public class BuildingLayout
        {
            #region Member Variables...
            Room[][] _Rooms;
            string _Passcode;
            List<List<Direction>> _FoundPaths = new List<List<Direction>>();
            #endregion Member Variables...

            #region Properties...

            public Room this[int x, int y]
            {
                get
                {
                    return _Rooms[x][y];
                }

                set
                {
                    _Rooms[x][y] = value;
                }
            }

            public Room[][] Rooms
            {
                get { return _Rooms; }
            }
            #endregion Properties...

            #region Constructor
            public BuildingLayout(int width, int height, string passcode)
            {
                _Passcode = passcode;
                _Rooms = new Room[width][];
                for(int x = 0; x < _Rooms.Length; x++)
                {
                    _Rooms[x] = new Room[height];
                    for(int y = 0; y < _Rooms[x].Length; y++)
                    {
                        _Rooms[x][y] = new Room(x, y, (x == width-1 && y == height-1));
                    }
                }                
            }
            #endregion Constructor

            public List<Direction> Navigate()
            {
                NavigateRooms(this[0, 0], new List<Direction>(), shortest: true);
                return _FoundPaths.OrderBy(path => path.Count).First();
            }

            public List<Direction> NavigateLongest()
            {
                Room current = this[0, 0];
                NavigateRooms(this[0, 0], new List<Direction>(), shortest: false);
                return _FoundPaths.OrderBy(path => path.Count).Last();
            }

            private void NavigateRooms(Room current, List<Direction> pathToHere, bool shortest)
            {
                var openDirections = GetOpenRooms(pathToHere).Where(direction => current[direction] != null);
                if (openDirections.Any())
                {
                    //List<Direction> possiblePath = null;
                    foreach (var direction in openDirections)
                    {
                        List<Direction> path = new List<Direction>();
                        path.AddRange(pathToHere);
                        path.Add(direction);
                        if (current[direction].HasVault)
                        {
                            _FoundPaths.Add(path);
                            if (shortest) return;
                        }
                        else
                        {
                            NavigateRooms(current[direction], path, shortest);
                        }
                    }
                }
            }

            private List<Direction> GetOpenRooms(List<Direction> path)
            {
                List<Direction> openRooms = new List<Direction>();
                string hash = GetHashString(_Passcode + string.Join("", path.Select(d => Format(d))));
                char[] open = { 'b', 'c', 'd', 'e', 'f' };
                if (open.Contains(hash[0]))
                {
                    openRooms.Add(Direction.Up);
                }
                if (open.Contains(hash[1]))
                {
                    openRooms.Add(Direction.Down);
                }
                if (open.Contains(hash[2]))
                {
                    openRooms.Add(Direction.Left);
                }
                if (open.Contains(hash[3]))
                {
                    openRooms.Add(Direction.Right);
                }
                return openRooms;
            }
        }

        #region class Room
        public class Room
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            private Dictionary<Direction, Room> _Rooms;

            public Room Down
            {
                get { return GetRoom(Direction.Down); }
                set { SetRoom(Direction.Down, value); }
            }

            public Room Left
            {
                get { return GetRoom(Direction.Left); }
                set { SetRoom(Direction.Left, value); }
            }

            public Room Right
            {
                get { return GetRoom(Direction.Right); }
                set { SetRoom(Direction.Right, value); }
            }

            public Room Up
            {
                get { return GetRoom(Direction.Up); }
                set { SetRoom(Direction.Up, value); }
            }

            public bool HasVault { get; internal set; }

            public Room this[Direction direction]
            {
                get { return GetRoom(direction); }
            }

            public Room(int x, int y, bool hasVault)
            {
                X = x;
                Y = y;
                HasVault = hasVault;
                _Rooms = new Dictionary<Direction, Room>();
            }

            public Room GetRoom(Direction direction)
            {
                return _Rooms.ContainsKey(direction) ? _Rooms[direction] : null;
            }

            public void SetRoom(Direction direction, Room room)
            {
                _Rooms[direction] = room;
            }

            public override string ToString()
            {
                return $"Room at [{X},{Y}]";
            }
        }
        #endregion class Room

        #region enum Direction
        public enum Direction
        {
            Up, Down, Left, Right
        }
        #endregion enum Direction


        private static string GetHashString(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = _Md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        
        private static char Format(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down: return 'D';
                case Direction.Left: return 'L';
                case Direction.Right: return 'R';
                case Direction.Up: return 'U';
            }
            return (char)0;
        }
    }
}
