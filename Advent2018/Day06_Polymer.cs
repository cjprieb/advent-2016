using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2018
{
    public struct Coordinate
    {
        public int x;
        public int y;
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Point
    {
        public Coordinate coordinate;
        public char? id;
        public Coordinate? nearest;

        public Point(Coordinate c)
        {
            coordinate = c;
            id = null;
            nearest = null;
        }

        public Point(Coordinate c, char? id, Coordinate? nearest)
        {
            coordinate = c;
            this.id = id;
            this.nearest = nearest;
        }
    }

    public struct Distance
    {
        public Coordinate coordinate;
        public int distance;

        public Distance(Coordinate a, int d)
        {
            coordinate = a;
            distance = d;
        }
    }

    public struct Rectangle
    {
        public int left;
        public int top;
        public int bottom;
        public int right;

        public Rectangle(int top, int left, int bottom, int right)
        {
            this.top = top;
            this.left = left;
            this.bottom = bottom;
            this.right = right;
        }
    }

    public class Day06
    {
        private Point[,] Map = new Point[,] { };
        private Coordinate[] Coordinates = { };
        private Dictionary<Coordinate, char> Ids = new Dictionary<Coordinate, char>();
        private HashSet<char> HasInfiniteArea = new HashSet<char>();

        private Rectangle GetEdges()
        {
            int top = Coordinates.Min(a => a.y);
            int left = Coordinates.Min(a => a.x);
            int bottom = Coordinates.Max(a => a.y);
            int right = Coordinates.Max(a => a.x);
            return new Rectangle(top, left, bottom, right);
        }

        private Coordinate GetCenter(Rectangle edges)
        {
            int x = edges.left + (edges.right - edges.left);
            int y = edges.top + (edges.bottom - edges.top);
            return new Coordinate(x, y);
        }

        private int GetDistance(Coordinate a, Coordinate b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        private void SetIds()
        {
            char c = 'a';
            foreach (var a in Coordinates)
            {
                Ids[a] = c;
                if (c == 'z') c = 'A';
                else c = (char)(c + 1);
            }
        }

        private void SetNearest(Rectangle edges, Coordinate a)
        {
            Point p;
            if (Ids.ContainsKey(a))
            {
                p = new Point(a, Ids[a], null);
            }
            else
            {
                var distances = Coordinates
                    .Select(b => new Distance(b, GetDistance(a, b)))
                    .OrderBy(d => d.distance);
                var min = distances.First();
                if (distances.TakeWhile(d => d.distance == min.distance).Count() > 1)
                {
                    p = new Point(a);
                }
                else
                {
                    p = new Point(a, null, min.coordinate);
                }
            }
            if (a.x == edges.left || a.x == edges.right || a.y == edges.top || a.y == edges.bottom)
            {
                if (p.nearest != null)
                {
                    HasInfiniteArea.Add(Ids[p.nearest.Value]);
                }
            }
            Map[a.x, a.y] = p;
        }

        public void BuildMapForMax(Coordinate[] coordinates)
        {
            Coordinates = coordinates;
            SetIds();
            Rectangle edges = GetEdges();
            Coordinate center = GetCenter(edges);
            Map = new Point[edges.right+1, edges.bottom+1];
            for (int x = edges.left; x <= edges.right; x++)
            {
                for (int y = edges.top; y <= edges.bottom; y++)
                {
                    Coordinate a = new Coordinate(x, y);
                    SetNearest(edges, a);
                }
            }
        }

        public int GetMaxRegionSize(Coordinate[] coordinates, int maxDistance)
        {
            Coordinates = coordinates;
            List<Distance> distances = new List<Distance>();
            Rectangle edges = GetEdges();
            for (int x = edges.left; x <= edges.right; x++)
            {
                for (int y = edges.top; y <= edges.bottom; y++)
                {
                    Coordinate a = new Coordinate(x, y);
                    int totalDistance = Coordinates.Sum(b => GetDistance(a, b));
                    if (totalDistance < maxDistance) distances.Add(new Distance(a, totalDistance));
                }
            }
            return distances.Count();
        }

        public Distance GetCoordinateWithMaxArea()
        {

            Dictionary<Coordinate, int> areas = new Dictionary<Coordinate, int>();
            foreach (var coord in Coordinates)
            {
                areas.Add(coord, HasInfiniteArea.Contains(Ids[coord]) ? 0 : 1);
            }

            foreach (var point in Map)
            {
                if (point.nearest.HasValue)
                {
                    var nearestCoordinate = point.nearest.Value;
                    if (!HasInfiniteArea.Contains(Ids[nearestCoordinate]))
                    {
                        areas[nearestCoordinate]++;
                    }
                }
            }
            
            var max = areas.OrderByDescending(kvp => kvp.Value).First();
            return new Distance(max.Key, max.Value);
        }
    }
}
