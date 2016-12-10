using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day3_Triangles
    {
        public static int CountValidTriangles(string[] triangles)
        {
            return triangles
                .Select(triangle => ParseTriangle(triangle))
                .Where(sides => IsValidTriangle(sides))
                .Count();
        }

        public static int CountValidVerticalTriangles(string[] triangleStrings)
        {
            List<int[]> listOfSides = triangleStrings
                .Select(triangle => ParseTriangle(triangle))
                .Where(sides => sides != null)
                .ToList();
            List<int[]> triangles = new List<int[]>();
            Dictionary<int, int[]> currentSides = new Dictionary<int, int[]>();
            for (int i = 0; i < listOfSides.Count(); i++)
            {
                int sideIndex = i % 3;
                for (int currIndex = 0; currIndex < 3; currIndex++)
                {
                    if ( !currentSides.ContainsKey(currIndex) )
                    {
                        currentSides[currIndex] = new int[3];
                    }
                    currentSides[currIndex][sideIndex] = listOfSides[i][currIndex];
                }

                if (sideIndex == 2)
                {
                    triangles.AddRange(currentSides.Values);
                    currentSides.Clear();
                }
            }

            return triangles.Where(sides => IsValidTriangle(sides)).Count();
        }

        public static bool IsValidTriangle(int[] sides)
        {
            bool isValid = false;

            if (sides != null && sides.Length == 3 && sides.All(i => i > 0))
            {
                sides = sides.OrderBy(i => i).ToArray();
                isValid = (sides[0] + sides[1]) > sides[2];
            }

            return isValid;
        }

        private static int[] ParseTriangle(string triangle)
        {
            string[] parts = triangle.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            int[] sides = null;
            if (parts.Length == 3)
            {
                sides = new int[3];
                for (int i = 0; i < sides.Length; i++)
                {
                    sides[i] = int.Parse(parts[i].Trim());
                }
            }
            return sides;
        }
    }
}
