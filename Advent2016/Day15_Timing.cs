using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent
{
    public class Day15_Timing
    {
        //const int _MAX_TIME = 440895;

        public static int TimeToTriggerCapsule(string[] input, Disc extraDisc = null)
        {
            List<Disc> discs = ParseInput(input);
            if (extraDisc != null)
            {
                discs.Add(extraDisc);
            }
            int maxTime = discs.Select(disc => disc.MaxPositions).Aggregate(1, (a, b) => a * b);
            List<Capsule> capsulesDropped = new List<Capsule>();

            int timeToTriggerCapsule = -1;
            for (int ticks = 0; ticks < maxTime; ticks++)
            {
                capsulesDropped.Add(new Capsule(ticks));

                foreach (var disc in discs)
                {
                    disc.Tick();

                    Capsule capsuleAtDisc = capsulesDropped.FirstOrDefault(cap => cap.CurrentDiscIndex + 1 == disc.Index);
                    if (capsuleAtDisc != null && !disc.CapsuleDrops())
                    {
                        capsulesDropped.Remove(capsuleAtDisc);
                    }
                }

                foreach (var capsule in capsulesDropped)
                {
                    capsule.CurrentDiscIndex++; // all could drop, so move to next disc
                }

                Capsule capsuleAtBottom = capsulesDropped.FirstOrDefault(cap => cap.CurrentDiscIndex == discs.Max(disc => disc.Index));
                if (capsuleAtBottom != null)
                {
                    timeToTriggerCapsule = capsuleAtBottom.StartingTick;
                    break;
                }
            }

            return timeToTriggerCapsule;
        }

        #region ParseInput
        public static List<Disc> ParseInput(string[] input)
        {
            List<Disc> discs = new List<Disc>();
            foreach (string discString in input)
            {
                Match match = Regex.Match(discString, @"Disc #(?<disc>\d+) has (?<max>\d+) positions; at time=0, it is at position (?<current>\d+)\.");
                if (match.Success)
                {
                    int index = int.Parse(match.Groups["disc"].Value);
                    int maxPositions = int.Parse(match.Groups["max"].Value);
                    int currentPosition = int.Parse(match.Groups["current"].Value);
                    discs.Add(new Disc(index, maxPositions, currentPosition));
                }
                else if (discString.Trim().Length > 0)
                {
                    throw new Exception($"{discString} does not match expected pattern");
                }
            }
            return discs;
        }
        #endregion ParseInput

        #region Disc class
        public class Disc
        {
            private int _CurrentPosition;
            private int _MaxPositions;

            public int CurrentPosition { get { return _CurrentPosition; } }
            public int Index { get; set; }
            public int MaxPositions { get { return _MaxPositions; } }
            public string Id { get; set; }

            public Disc(int index, int maxPositions, int currentPosition)
            {
                _MaxPositions = maxPositions;
                _CurrentPosition = currentPosition;
                Index = index;
                Id = $"#{index}";
            }

            public void Tick()
            {
                _CurrentPosition = (_CurrentPosition + 1) % _MaxPositions;
            }

            public bool CapsuleDrops()
            {
                return _CurrentPosition == 0;
            }

            public override string ToString()
            {
                return $"[{Id} at: {CurrentPosition}({_MaxPositions})]";
            }
        }
        #endregion Disc class

        public class Capsule
        {
            public int StartingTick { get; private set; }
            public int CurrentDiscIndex { get; set; }

            public Capsule(int ticks)
            {
                StartingTick = ticks;
                CurrentDiscIndex = 0;
            }
        }
    }
}
