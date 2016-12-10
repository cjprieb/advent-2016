using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
    public class Day8_TwoFactorAuthentication
    {
        public class ScreenPainter
        {
            private char[][] _Screen = null;

            /// <summary>
            /// First index is height.
            /// Second index is width.
            /// </summary>
            public string Screen
            {
                get { return string.Join("\n", _Screen.Select(row => new string(row))); }
            }

            public int LitPixelsCount
            {
                get { return _Screen.Sum(row => row.Count(c => c == '#')); }
            }

            public ScreenPainter(int width, int height)
            {
                if (width <= 0) throw new ArgumentOutOfRangeException("width", "width must be a postive non-zero integer");
                if (height <= 0) throw new ArgumentOutOfRangeException("height", "height must be a postive non-zero integer");

                _Screen = new char[height][];
                for (int i = 0; i < height; i++)
                {
                    _Screen[i] = new char[width];
                    for (int j = 0; j < width; j++)
                    {
                        _Screen[i][j] = '.';
                    }
                }
            }

            public void ParseInstructions(string[] instructions)
            {
                Regex rectRegex = new Regex(@"rect (\d+)x(\d+)");
                Regex rotateRegex = new Regex(@"rotate (column|row) (x|y)=(\d+) by (\d+)");
                foreach (string instruction in instructions)
                {
                    Match rectPattern = rectRegex.Match(instruction);
                    if (rectPattern.Success)
                    {
                        int width = int.Parse(rectPattern.Groups[1].Value);
                        int height = int.Parse(rectPattern.Groups[2].Value);
                        Rectangle(width, height);
                    }
                    else
                    {
                        Match rotatePattern = rotateRegex.Match(instruction);
                        if (rotatePattern.Success)
                        {
                            string xOrY = rotatePattern.Groups[2].Value;
                            int rowOrColumn = int.Parse(rotatePattern.Groups[3].Value);
                            int pixels = int.Parse(rotatePattern.Groups[4].Value);
                            if (xOrY.Equals("x", StringComparison.OrdinalIgnoreCase))
                            {
                                RotateColumn(rowOrColumn, pixels);
                            }
                            else if (xOrY.Equals("y", StringComparison.OrdinalIgnoreCase))
                            {
                                RotateRow(rowOrColumn, pixels);
                            }
                        }
                    }
                }
            }

            public void Rectangle(int width, int height)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        _Screen[y][x] = '#';
                    }
                }
            }

            public void RotateColumn(int column, int pixels)
            {
                char[] tempColumn = _Screen.Select(row => row[column]).ToArray();
                for (int y = 0; y < tempColumn.Length; y++)
                {
                    int index = (y + pixels) % tempColumn.Length;
                    _Screen[index][column] = tempColumn[y];
                }
            }

            public void RotateRow(int row, int pixels)
            {
                char[] tempRow = _Screen[row].Select(c => c).ToArray();
                for (int x = 0; x < tempRow.Length; x++)
                {
                    int index = (x + pixels) % tempRow.Length;
                    _Screen[row][index] = tempRow[x];
                }
            }
        }
    }
}
