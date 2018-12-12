using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary1.Day07
{
    public struct Step
    {
        public char Id;
        public int Duration;

        public Step(char id)
        {
            Id = id;
            Duration = 61 + (id - 'A');
        }

        public Step(char id, bool isTest)
        {
            Id = id;
            Duration = (isTest ? 1 : 61) + (id - 'A');
        }
    }

    public class StepNode
    {
        public bool IsDone { get; set; }
        public int? StartSecond { get; set; }
        public HashSet<Step> Dependencies { get; private set; } = new HashSet<Step>();
        public Step Step { get; private set; }

        public StepNode(Step step)
        {
            Step = step;
        }

        internal bool IsStepDone(int second)
        {
            return StartSecond.HasValue && ((second - StartSecond) >= Step.Duration);
        }

        internal bool IsStepReady(Dictionary<char, StepNode> steps)
        {
            return !IsDone && !StartSecond.HasValue && Dependencies.All(step => steps[step.Id].IsDone);
        }
    }


    public class Day07_Instructions
    {

        private void AddAll(SortedList<char, char> readySteps, IEnumerable<Step> steps)
        {
            foreach (var step in steps)
            {
                if (!readySteps.ContainsKey(step.Id))
                {
                    readySteps.Add(step.Id, step.Id);
                }
            }
        }

        public Dictionary<char, StepNode> Build(IEnumerable<string> lines, bool isTest = false)
        {
            Dictionary<char, StepNode> Result = new Dictionary<char, StepNode>();

            foreach (var line in lines)
            {
                Parse(line, Result, isTest);
            }

            return Result;
        }

        public string DoSteps(Dictionary<char, StepNode> steps)
        {

            SortedList<char, char> readySteps = new SortedList<char, char>();
            List<char> doneSteps = new List<char>();
            AddAll(readySteps, GetReadySteps(steps));

            while (readySteps.Any())
            {
                var nextStep = readySteps.First();
                steps[nextStep.Key].IsDone = true;
                doneSteps.Add(nextStep.Key);
                readySteps.Remove(nextStep.Key);
                AddAll(readySteps, GetReadySteps(steps));
            }

            return new string(doneSteps.ToArray());
        }

        public int DoStepsWithTime(int numberOfHelpers, Dictionary<char, StepNode> steps)
        {
            SortedList<char, char> readySteps = new SortedList<char, char>();
            StepNode[] inProgressSteps = new StepNode[numberOfHelpers];
            List<char> doneSteps = new List<char>();
            AddAll(readySteps, GetReadySteps(steps));
            int second = -1;            

            while (!steps.Values.All(step => step.IsDone))
            {
                bool printDebug = false;
                string c = "";
                second++;
                for (int i = 0; i < numberOfHelpers; i++)
                {
                    var node = inProgressSteps[i];
                    if (node != null && node.IsStepDone(second))
                    {
                        node.IsDone = true;

                        char id = node.Step.Id;
                        doneSteps.Add(id);
                        
                        inProgressSteps[i] = null;
                        printDebug = true;
                    }
                }

                AddAll(readySteps, GetReadySteps(steps));
                c = string.Join(", ", readySteps.Keys);

                for (int i = 0; i < numberOfHelpers && readySteps.Any(); i++)
                {
                    if (inProgressSteps[i] == null && readySteps.Any())
                    {
                        var nextStep = readySteps.First();
                        char id = nextStep.Key;
                        inProgressSteps[i] = steps[nextStep.Key];
                        inProgressSteps[i].StartSecond = second;
                        readySteps.Remove(id);
                        printDebug = true;
                    }
                }

                if (printDebug)
                {
                    var a = inProgressSteps.Select(x => x == null ? ' ' : x.Step.Id);
                    var b = string.Join(" | ", a);
                    Console.WriteLine($"{second} | {b} | {new string(doneSteps.ToArray())} ({c})");
                }
            }

            return second;
        }

        public List<Step> GetReadySteps(Dictionary<char, StepNode> steps)
        {
            List<Step> readySteps = new List<Step>();
            foreach (var kvp in steps)
            {
                StepNode node = kvp.Value;
                if (node.IsStepReady(steps))
                {
                    readySteps.Add(node.Step);
                }
            }
            return readySteps;
        }

        private void Parse(string line, Dictionary<char, StepNode> nodes, bool isTest)
        {
            Match match = Regex.Match(line, @"Step (\w) must be finished before step (\w) can begin");

            char prereq = match.Groups[1].Value[0];
            if (!nodes.ContainsKey(prereq))
            {
                nodes[prereq] = new StepNode(new Step(prereq, isTest));
            }

            char step = match.Groups[2].Value[0];
            if (!nodes.ContainsKey(step))
            {
                nodes[step] = new StepNode(new Step(step, isTest));
            }

            nodes[step].Dependencies.Add(nodes[prereq].Step);
        }

        public string Solve1()
        {
            var input = Properties.Resources.Day07.Split('\n');
            return Solve1(input);
        }

        public string Solve1(IEnumerable<string> input)
        {
            var steps = Build(input);
            return DoSteps(steps);
        }

        public int Solve2(int numberOfHelpers)
        {
            var input = Properties.Resources.Day07.Split('\n');
            return Solve2(input, numberOfHelpers, false);
        }

        public int Solve2(IEnumerable<string> input, int numberOfHelpers, bool isTest)
        {
            var steps = Build(input, isTest);
            return DoStepsWithTime(numberOfHelpers, steps);
        }
    }
}
