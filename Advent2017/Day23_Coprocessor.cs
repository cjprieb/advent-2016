using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day23_2017
    {
        private const int a = 1;
        private static int b = 108100;
        private const int c = 125100;
        private static int d = 0;
        //private static int e = 0;
        private static int f = 0;
        //private static int g = 0;
        private static int h = 0;
        private static long total = 0;

        public static void Run()
        {
            bool repeat = true;
            while (repeat)
            {
                //Print("While (true)");
                f = 1;
                d = 2;
                int tmp = (int)Math.Sqrt(b);
                total++;
                while (d <= tmp && f != 0)
                {
                    //if (total > 20)
                    //{
                    //    Print("Stopping!");
                    //    repeat = false;
                    //    break;
                    //}
                    int divisor = b / d;
                    if (b % d == 0)
                    {
                        Console.WriteLine("d {0} divides into b {1}", d, b);
                        f = 0;
                    }
                    d += 1;
                }
                //Print("If (f == 0)");
                if (f == 0)
                {
                    h += 1;
                    //Console.WriteLine("Increasing h: {0}", h);
                }
                if (b == c)
                {
                    repeat = false;
                    Print("Done");
                }
                else
                {
                    b += 17;
                    //Print("Repeating");
                }
            }
        }

        private static void Print(string msg)
        {
            Console.WriteLine("Registers - " + msg);
            Console.WriteLine("  a: {0}", a);
            Console.WriteLine("  b: {0}", b);
            Console.WriteLine("  c: {0}", c);
            Console.WriteLine("  d: {0}", d);
            //Console.WriteLine("  e: {0}", e);
            Console.WriteLine("  f: {0}", f);
            //Console.WriteLine("  g: {0}", g);
            Console.WriteLine("  h: {0}", h);
        }
    }
}
