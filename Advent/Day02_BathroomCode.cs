using System.Text;
using Advent.Directions;

namespace Advent
{
    public class Day02_BathroomCode
    {
        private static string[][] _NormalCodePad =
        {
            new string [] { "1", "2", "3" },
            new string [] { "4", "5", "6" },
            new string [] { "7", "8", "9" }
        };

        private static string[][] _ImprovedCodePad =
        {
            new string [] { " ", " ", "1", " ", " " },
            new string [] { " ", "2", "3", "4", " " },
            new string [] { "5", "6", "7", "8", "9" },
            new string [] { " ", "A", "B", "C", " " },
            new string [] { " ", " ", "D", " ", " " }
        };

        public static string ParseInstructions(string[] instructions)
        {
            Day02_BathroomCode codeParser = new Day02_BathroomCode(new Point(1, 1), _NormalCodePad, instructions);
            return codeParser._CodeBuilder.ToString();
        }

        public static string ParseInstructions_Part2(string[] instructions)
        {
            Day02_BathroomCode codeParser = new Day02_BathroomCode(new Point(0, 2), _ImprovedCodePad, instructions);
            return codeParser._CodeBuilder.ToString();
        }

        private string[][] _CodePad = null;
        private Point _CurrentPosition;
        private StringBuilder _CodeBuilder = new StringBuilder();

        public Day02_BathroomCode(Point startingPoint, string[][] codepad, string[] instructions)
        {
            _CurrentPosition = startingPoint;
            _CodePad = codepad;
            foreach (var instruction in instructions)
            {
                ParseInstruction(instruction);
                _CodeBuilder.Append(CharAtPosition(_CurrentPosition));
            }
        }

        private char CharAtPosition(Point point)
        {
            return _CodePad[point.Y][point.X][0];
        }

        private void ParseInstruction(string instruction)
        {
            foreach (var direction in instruction)
            {
                MoveCursor(direction);
            }            
        }

        private void MoveCursor(char direction)
        {
            int x = _CurrentPosition.X;
            int y = _CurrentPosition.Y;
            switch (direction)
            {
                case Direction.Up:
                    y -= 1;
                    break;

                case Direction.Down:
                    y += 1;
                    break;

                case Direction.Right:
                    x += 1;
                    break;

                case Direction.Left:
                    x -= 1;
                    break;
            }

            Point newPosition = new Point(x, y);
            if (0 <= x && x < _CodePad.Length &&
                0 <= y && y < _CodePad.Length &&
                CharAtPosition(newPosition) != ' ')
            {
                _CurrentPosition = newPosition;
            }
        }
    }
}
