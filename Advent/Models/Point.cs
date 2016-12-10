using System;

namespace Advent
{
    public struct Point
    {
        public int X { get; }

        public int Y { get; }

        public int BlockDistanceFromCenter
        {
            get
            {
                return Math.Abs(X) + Math.Abs(Y);
            }
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                Point position = (Point)obj;
                return position.X == X && position.Y == Y;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
