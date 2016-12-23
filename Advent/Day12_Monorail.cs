using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent
{
    public class Day12_Monorail
    {
        public static AssemBunny ProcessInstructions(string[] input, Dictionary<char, int> startingValues = null)
        {
            AssemBunny assembler = new AssemBunny(startingValues);
            assembler.RunInstructions(input.Select(str => str.Trim()));
            return assembler;
        }
    }

    public class AssemBunny
    {
        private Registers _Registers;

        private int _CurrentInstructionIndex = 0;
        public List<AssemBunnyInstruction> Instructions { get; private set; }

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
            Instructions = new List<AssemBunnyInstruction>();
        }

        public void RunInstructions(IEnumerable<string> instructions)
        {
            instructions = instructions.Where(item => item.Trim().Length > 0);
            Instructions.Clear();
            Instructions.AddRange(instructions.Select(item => new AssemBunnyInstruction(item)));
            while (_CurrentInstructionIndex < Instructions.Count)
            {
                var instruction = Instructions[_CurrentInstructionIndex];
                instruction.Run(this);
                _CurrentInstructionIndex++;
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
            int value = 0;
            if (_Registers.IsValidRegister(valueString))
            {
                char register = valueString[0];
                value = _Registers[register];
            }
            else
            {
                if (!int.TryParse(valueString, out value))
                {
                    throw new FormatException($"{valueString} for instruction {_CurrentInstructionIndex} could not be parsed as an integer");
                }
            }
            if (value > 0)
            {
                int jumpValue = 0;

                if (_Registers.IsValidRegister(jumpString))
                {
                    char register = jumpString[0];
                    jumpValue = _Registers[register];
                    _CurrentInstructionIndex += jumpValue - 1; //subtract 1 since instruction will auto-advance 1
                }
                else if (int.TryParse(jumpString, out jumpValue))
                {
                    _CurrentInstructionIndex += jumpValue - 1; //subtract 1 since instruction will auto-advance 1
                }
                else
                { 
                    //Skip if invalid instruction
                    //throw new FormatException($"[{jumpString}] for instruction {_CurrentInstructionIndex} could not be parsed as an integer");
                }
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

        internal void Toggle(string valueString)
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
            var toggleInstructionAt = _CurrentInstructionIndex + value;
            if (toggleInstructionAt >= 0 && toggleInstructionAt < Instructions.Count)
            {
                Instructions[toggleInstructionAt].Toggle();
            }
        }
    }

    #region class AssemBunnyInstruction
    public class AssemBunnyInstruction
    {
        public AssemBunnyInstruction(string instruction)
        {
            string[] parts = instruction.Trim().Split(' ');
            if (parts.Length > 1)
            {
                Instruction = parts[0];
                ArgumentOne = parts[1];
                switch (Instruction)
                {
                    case "jnz":
                    case "cpy":
                        ArgumentTwo = parts[2];
                        break;

                    default:
                        break;
                }
            }
        }

        public string Instruction { get; set; }

        public string ArgumentOne { get; set; }

        public string ArgumentTwo { get; set; }

        public bool IsOneArgumentInstruction
        {
            get
            {
                return Instruction.Equals("inc") || Instruction.Equals("dec") || Instruction.Equals("tgl");
            }
        }

        public bool IsTwoArgumentInstruction
        {
            get
            {
                return Instruction.Equals("jnz") || Instruction.Equals("cpy");
            }
        }

        public void Toggle()
        {
            if (IsOneArgumentInstruction)
            {
                Instruction = Instruction.Equals("inc") ? "dec" : "inc";
            }
            else if (IsTwoArgumentInstruction)
            {
                Instruction = Instruction.Equals("jnz") ? "cpy" : "jnz";
            }
        }

        /// <summary>
        ///  The assembunny code you've extracted operates on four registers (a, b, c, and d) 
        ///     that start at 0 and can hold any integer. 
        ///  However, it seems to make use of only a few instructions:
        /// 'cpy x y' copies x (either an integer or the value of a register) into register y.
        /// 'inc x' increases the value of register x by one.
        /// 'dec x' decreases the value of register x by one.
        /// 'jnz x y' jumps to an instruction y away (positive means forward; negative means backward), 
        ///     but only if x is not zero.
        /// The jnz instruction moves relative to itself: an offset of -1 would continue at the previous 
        ///     instruction, while an offset of 2 would skip over the next instruction.
        /// </summary>
        /// <param name="computer"></param>
        public void Run(AssemBunny computer)
        {
            switch (Instruction)
            {
                case "cpy":
                    computer.Copy(ArgumentOne, ArgumentTwo);
                    break;

                case "inc":
                    computer.Increase(ArgumentOne);
                    break;

                case "dec":
                    computer.Decrease(ArgumentOne);
                    break;

                case "jnz":
                    computer.Jump(ArgumentOne, ArgumentTwo);
                    break;

                case "tgl":
                    computer.Toggle(ArgumentOne);
                    break;

                default:
                    throw new Exception($"Unable to run instruction '{Instruction}'");
            }
        }

        public override string ToString()
        {
            return $"[{Instruction} {ArgumentOne}{(IsTwoArgumentInstruction ? " " + ArgumentTwo : "")}]";
        }
    }
    #endregion class AssemBunnyInstruction

    #region class Registers
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
    #endregion class Registers
}
