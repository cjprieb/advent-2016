using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
    public class Day21_Scrambling
    {
        public static string ScramblePassword(string[] input, string password)
        {
            Descrambler descrambler = new Descrambler(password, reverse: false);
            foreach (var instruction in input)
            {
                descrambler.Descramble(instruction.Trim());
            }
            return descrambler.Password;
        }

        public static string DescramblePassword(string[] input, string password)
        {
            Descrambler descrambler = new Descrambler(password, reverse: true);
            foreach (var instruction in input.Reverse())
            {
                descrambler.Descramble(instruction.Trim());
            }
            return descrambler.Password;
        }

        public class Descrambler
        {
            private StringBuilder _currentPassword;
            private bool _reverse = false;

            public string Password { get { return _currentPassword.ToString(); } }

            public Descrambler(string password, bool reverse)
            {
                _reverse = reverse;
                _currentPassword = new StringBuilder(password);
            }

            public void Descramble(string instruction)
            {
                Match swap = Regex.Match(instruction, @"swap (\w+) ([\w\d]+) with (\w+) ([\w\d]+)");
                if (swap.Success)
                {
                    string type = swap.Groups[1].Value;
                    string first = swap.Groups[2].Value;
                    string second = swap.Groups[4].Value;
                    if (type == "letter")
                    {
                        SwapLetters(first[0], second[0]);
                    }
                    else if (type == "position")
                    {
                        SwapPosition(int.Parse(first), int.Parse(second));
                    }
                    else
                    {
                        throw new Exception($"Unknown swap type of [{type}]");
                    }
                    return;
                }

                Match rotateStep = Regex.Match(instruction, @"rotate (\w+) (\d+) step");
                if (rotateStep.Success)
                {
                    string direction = rotateStep.Groups[1].Value;
                    int steps = int.Parse(rotateStep.Groups[2].Value);
                    RotateSteps(direction, steps);
                    return;
                }

                Match rotatePosition = Regex.Match(instruction, @"rotate based on position of letter (\w)");
                if (rotatePosition.Success)
                {
                    string letter = rotatePosition.Groups[1].Value;
                    RotateBasedOnPosition(letter[0]);
                    return;
                }

                Match reversePosition = Regex.Match(instruction, @"reverse positions (\d+) through (\d+)");
                if (reversePosition.Success)
                {
                    int start = int.Parse(reversePosition.Groups[1].Value);
                    int end = int.Parse(reversePosition.Groups[2].Value);
                    ReversePositions(start, end);
                    return;
                }

                Match movePositions = Regex.Match(instruction, @"move position (\d+) to position (\d+)");
                if (movePositions.Success)
                {
                    int start = int.Parse(movePositions.Groups[1].Value);
                    int end = int.Parse(movePositions.Groups[2].Value);
                    MovePositions(start, end);
                    return;
                }

                if (!string.IsNullOrEmpty(instruction))
                {
                    throw new Exception($"[{instruction}] didn't match any patterns");
                }
            }

            private void MovePositions(int start, int end)
            {
                if (_reverse)
                {
                    var tmp = end;
                    end = start;
                    start = tmp;
                }
                var removedChar = _currentPassword[start];
                _currentPassword.Remove(start, 1);
                _currentPassword.Insert(end, removedChar);
            }

            private void ReversePositions(int start, int end)
            {
                string password = Password;
                int length = end - start + 1;
                var reversedString = password.Substring(start, length).Reverse().ToArray();
                _currentPassword.Remove(start, length);
                _currentPassword.Insert(start, reversedString);
            }

            public void RotateBasedOnPosition(char letter)
            {
                string password = Password;
                int length = password.Length;
                int letterPosition = password.IndexOf(letter);
                if (!_reverse)
                {
                    int steps = (1 + letterPosition + (letterPosition >= 4 ? 1 : 0)) % length;
                    RotateSteps("right", steps);
                }
                else
                {
                    int originalIndex = -1;
                    if (letterPosition == 0)
                    {
                        //customized for length
                        originalIndex = length-1;
                    }
                    else if (letterPosition % 2 == 0)
                    {
                        originalIndex = (letterPosition + length - 2) / 2;
                    }
                    else
                    {
                        originalIndex = (letterPosition - 1) / 2;
                    }

                    int steps = (1 + originalIndex + (originalIndex >= 4 ? 1 : 0)) % length;
                    RotateSteps("right", steps);
                }
            }

            public void RotateSteps(string direction, int steps)
            {
                int length = _currentPassword.Length;
                if ((!_reverse && direction == "right") || (_reverse && direction == "left"))
                {
                    while (steps > 0)
                    {
                        _currentPassword.Insert(0, _currentPassword[_currentPassword.Length-1]);
                        _currentPassword.Remove(length, 1);
                        steps--;
                    }
                }
                else if ((_reverse && direction == "right") || (!_reverse && direction == "left"))
                {
                    while (steps > 0)
                    {
                        _currentPassword.Append(_currentPassword[0]);
                        _currentPassword.Remove(0, 1);
                        steps--;
                    }
                }                
            }

            public void SwapLetters(char first, char second)
            {
                string password = Password;
                SwapPosition(password.IndexOf(first), password.IndexOf(second));
            }

            public void SwapPosition(int first, int second)
            {
                var tmp = _currentPassword[second];
                _currentPassword[second] = _currentPassword[first];
                _currentPassword[first] = tmp;
            }
        }
    }
}
