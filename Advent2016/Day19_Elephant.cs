using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day19_Elephant
    {
        /// <summary>
        /// Each Elf brings a present. 
        /// They all sit in a circle, numbered starting with position 1. 
        /// Then, starting with the first Elf, they take turns stealing all the 
        /// presents from the Elf to their left. 
        /// An Elf with no presents is removed from the circle and does not 
        /// take turns.
        /// </summary>
        /// <param name="numberOfElves"></param>
        /// <returns></returns>
        public static int ElfWhoGetsPresents(int numberOfElves)
        {
            int numberOfElvesRemaining = numberOfElves;
            List<int> currentElvesInCircle = new List<int>();
            for (int i = 0; i < numberOfElvesRemaining; i++)
            {
                if (i % 2 == 0)
                {
                    currentElvesInCircle.Add(i+1);
                }
            }
            
            if (numberOfElvesRemaining % 2 == 1)
            {
                currentElvesInCircle.RemoveAt(0);
            }

            List<int> nextElvesInCircle = new List<int>();
            while (currentElvesInCircle.Count > 1)
            {
                numberOfElvesRemaining = currentElvesInCircle.Count;
                for (int i = 0; i < numberOfElvesRemaining; i++)
                {
                    if (i % 2 == 0)
                    {
                        nextElvesInCircle.Add(currentElvesInCircle[i]);
                    }
                }

                if (numberOfElvesRemaining % 2 == 1)
                {
                    nextElvesInCircle.RemoveAt(0);
                }

                currentElvesInCircle.Clear();
                currentElvesInCircle.AddRange(nextElvesInCircle);
                nextElvesInCircle.Clear();
            }
            return currentElvesInCircle.FirstOrDefault();
        }

        public static int ElfWhoGetsPresents_SideVariation(int numberOfElves)
        {
            ElfCircle circle = new ElfCircle();
            for (int i = 0; i < numberOfElves; i++)
            {
                if (i % 2 == 0)
                {
                    circle.Add(new Elf(i + 1));
                }
            }

            Elf elf = circle.Root;
            while (circle.Count > 1)
            {
                circle.Remove(elf.Next);
                elf = elf.Next;
            }
            return circle.Root.Position;
        }

        public static int ElfWhoGetsPresents_AcrossVariation(int numberOfElves)
        {
            ElfCircle circle = new ElfCircle();
            for (int i = 0; i < numberOfElves; i++)
            {
                circle.Add(new Elf(i + 1));
            }

            Elf elf = circle.Root;
            while (circle.Count > 1)
            {
                circle.Remove(circle.Across);
                elf = elf.Next;
                circle.Across = circle.Across.Next;
            }
            return circle.Root.Position;
        }

        class ElfCircle
        {
            private Elf _Root = null;
            private Elf _Across = null;
            private int _Count = 0;

            public int Count
            {
                get { return _Count; }
            }

            public Elf Root
            {
                get { return _Root; }
            }

            public Elf Across
            {
                get { return _Across; }
                set { _Across = value; }
            }

            public void Add(Elf elf)
            {
                if (_Root == null)
                {
                    _Root = elf;
                    _Root.Next = elf;
                    _Root.Previous = elf;

                    _Across = elf;
                }
                else
                {
                    var tmp = _Root.Previous;
                    tmp.Next = elf;
                    elf.Next = _Root;
                    _Root.Previous = elf;
                    elf.Previous = tmp;
                }

                _Count++;
                if (_Count % 2 == 0)
                {
                    _Across = _Across.Next;
                }
            }

            public void Remove(Elf elf)
            {
                _Count--;
                if (_Count % 2 == 1)
                {
                    _Across = _Across.Previous;
                }

                var next = elf.Next;
                var prev = elf.Previous;
                next.Previous = prev;
                prev.Next = next;
                elf.Next = null;
                elf.Previous = null;
                if (_Root == elf)
                {
                    _Root = next;
                }
                if (_Across == elf)
                {
                    _Across = next;
                }
            }
        }

        class Elf
        {
            public int Position { get; private set; }

            public Elf Next { get; set; }

            public Elf Previous { get; set; }

            public Elf(int index)
            {
                Position = index;
            }

            public override string ToString()
            {
                return $"Elf #{Position}";
            }
        }
    }
}
