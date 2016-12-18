using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day18_Rogue
    {
        public static List<string> GetTilesOnFloor(string startingRow, int numberOfRows)
        {
            List<string> rows = new List<string>() { startingRow };
            while (rows.Count != numberOfRows)
            {
                rows.Add(NextRow(rows.Last()));
            }
            return rows;
        }

        public static int NumberOfSafeTiles(string startingRow, int numberOfRows)
        {
            int rowCount = 1;
            int safeTileCount = CountSafeTiles(startingRow);
            string currentRow = startingRow;
            while (rowCount < numberOfRows)
            {
                currentRow = NextRow(currentRow);
                safeTileCount += CountSafeTiles(currentRow);
                rowCount++;
            }
            return safeTileCount;
        }

        public static string NextRow(string row)
        {
            StringBuilder newRow = new StringBuilder();
            for (int i = 0; i < row.Length; i++)
            {
                bool currentIsTrap = IsTrap(row[i]);
                bool leftIsTrap = i > 0 ? IsTrap(row[i - 1]) : false;
                bool rightIsTrap = (i + 1) < row.Length ? IsTrap(row[i + 1]) : false;
                if ((leftIsTrap && currentIsTrap && !rightIsTrap) ||
                    (!leftIsTrap && currentIsTrap && rightIsTrap) ||
                    (!leftIsTrap && !currentIsTrap && rightIsTrap) ||
                    (leftIsTrap && !currentIsTrap && !rightIsTrap))
                {
                    newRow.Append('^');
                }
                else
                {
                    newRow.Append('.');
                }
                
            }
            return newRow.ToString();
        }

        private static int CountSafeTiles(string row)
        {
            return row.Count(c => !IsTrap(c));
        }

        private static bool IsTrap(char c)
        {
            return c == '^';
        }
    }
}
