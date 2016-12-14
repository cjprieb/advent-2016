using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day12_Monorail
    {
        public static AssemBunny ProcessInstructions(string[] input, Dictionary<char, int> startingValues = null)
        {
            AssemBunny assembler = new AssemBunny(startingValues);
            assembler.Instructions.AddRange(input.Select(str => str.Trim()));
            assembler.RunInstructions();
            return assembler;
        }
    }

    public class AssemBunny
    {
        private Registers _Registers;

        private int _CurrentInstruction = 0;
        public List<string> Instructions { get; private set; }

        public Registers Registers
        {
            get
            {
                return _Registers;
            }
        }
        public AssemBunny(Dictionary<char, int> startingValues)
        {
            _Registers = new Registers(startingValues);
            Instructions = new List<string>();
        }

        public void RunInstructions()
        {
            while (_CurrentInstruction < Instructions.Count)
            {
                string instruction = Instructions[_CurrentInstruction];
                try
                {
                    RunInstruction(instruction);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Unable to run instruction '{instruction}' at index {_CurrentInstruction}", ex);
                }
                _CurrentInstruction++;
            }
        }

        /*
         The assembunny code you've extracted operates on four registers (a, b, c, and d) that start at 0 and can hold any integer. However, it seems to make use of only a few instructions:

'cpy x y' copies x (either an integer or the value of a register) into register y.
'inc x' increases the value of register x by one.
'dec x' decreases the value of register x by one.
'jnz x y' jumps to an instruction y away (positive means forward; negative means backward), but only if x is not zero.
The jnz instruction moves relative to itself: an offset of -1 would continue at the previous instruction, while an offset of 2 would skip over the next instruction.
         */
        public void RunInstruction(string instruction)
        {
            string[] parts = instruction.Split(' ');
            if (parts.Length > 1)
            {
                string action = parts[0];
                string x = parts[1];
                switch (action)
                {
                    case "cpy":
                        if (parts.Length == 3)
                        {
                            string y = parts[2];
                            Copy(x, y);
                        }
                        break;

                    case "inc":
                        Increase(x);
                        break;

                    case "dec":
                        Decrease(x);
                        break;

                    case "jnz":
                        if (parts.Length == 3)
                        {
                            string y = parts[2];
                            Jump(x, y);
                        }
                        break;
                }
            }
        }

        public void Copy(string valueString, string registerString)
        {
            int value;
            if (_Registers.IsValidRegister(valueString))
            {
                char register = valueString[0];
                value = _Registers[register];
            }
            else
            {
                value = int.Parse(valueString);
            }
            _Registers[registerString] = value;
        }

        public void Decrease(string registerString)
        {
            _Registers[registerString]--;
        }
        
        public void Jump(string valueString, string jumpString)
        {
            int value;
            if (_Registers.IsValidRegister(valueString))
            {
                char register = valueString[0];
                value = _Registers[register];
            }
            else
            {
                value = int.Parse(valueString);
            }
            if (value > 0)
            {
                int jumpValue = int.Parse(jumpString);
                _CurrentInstruction += jumpValue - 1; //subtract 1 since instruction will auto-advance 1
            }
        }

        public void Increase(string registerString)
        {
            _Registers[registerString]++;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            char[] keys = {'a', 'b', 'c', 'd'};
            foreach (var key in keys)
            {
                builder.Append($"[{key}: {_Registers[key]}]\n");
            }
            return builder.ToString();
        }
    }

    public class Registers
    {
        private Dictionary<char, int> _Registers = new Dictionary<char, int>();

        public Registers(Dictionary<char, int> startingValues = null)
        {
            char[] keys = { 'a', 'b', 'c', 'd' };
            foreach (char key in keys)
            {
                if (startingValues != null && startingValues.ContainsKey(key))
                {
                    _Registers[key] = startingValues[key];

                }
                else
                {
                    _Registers[key] = 0;
                }
            }
        }

        public int this[char register]
        {
            get
            {
                if (IsValidRegister(register))
                {
                    if (_Registers.ContainsKey(register))
                    {
                        return _Registers[register];
                    }
                    else
                    {
                        throw new ArgumentException($"Register {register} has not been set yet");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{register} is invalid. Register must be a, b, c, or d");
                }
            }

            set
            {
                if (IsValidRegister(register))
                {
                    _Registers[register] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{register} is invalid. Register must be a, b, c, or d");
                }
            }
        }

        public int this[string register]
        {
            get
            {
                if (IsValidRegister(register))
                {
                    if (_Registers.ContainsKey(register[0]))
                    {
                        return _Registers[register[0]];
                    }
                    else
                    {
                        throw new ArgumentException($"Register {register} has not been set yet");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{register} is invalid. Register must be a, b, c, or d");
                }
            }

            set
            {
                if (IsValidRegister(register))
                {
                    _Registers[register[0]] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{register} is invalid. Register must be a, b, c, or d");
                }
            }
        }

        internal bool IsValidRegister(string x)
        {
            if (x.Length == 1)
            {
                return IsValidRegister(x[0]);
            }
            else
            {
                return false;
            }
        }

        internal bool IsValidRegister(char register)
        {
            return register == 'a' || register == 'b' || register == 'c' || register == 'd';
        }
    }
}
