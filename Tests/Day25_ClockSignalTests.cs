using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Properties;

namespace Advent.Tests
{
    [TestClass]
    public class Day25_ClockSignalTests
    {
        [TestMethod]
        public void Answer_FindInitialValueForRegister()
        {
            //Setup
            string[] input = Resources.Day25_Input.Split('\n');
            int expected = 0;

            //Action
            var result = Day25_ClockSignal.FindInitialValueForRegister(input);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_GetTransmittedValuesFor_0()
        {
            //Setup
            string[] input = Resources.Day25_Input.Split('\n');
            int value = 0;

            //Action
            var result = Day25_ClockSignal.GetTransmittedValuesFor(input, value);

            //Assert
            CheckTransmittedValues(result, value, "0,0,1,1,0,0,0,0,0,1,0");
        }

        [TestMethod]
        public void Test_GetTransmittedValuesFor_1()
        {
            //Setup
            string[] input = Resources.Day25_Input.Split('\n');
            int value = 1;

            //Action
            var result = Day25_ClockSignal.GetTransmittedValuesFor(input, value);

            //Assert
            CheckTransmittedValues(result, value, "1,0,1,1,0,0,0,0,0,1,0");
        }

        [TestMethod]
        public void Test_GetTransmittedValuesFor_2()
        {
            //Setup
            string[] input = Resources.Day25_Input.Split('\n');
            int value = 2;

            //Action
            var result = Day25_ClockSignal.GetTransmittedValuesFor(input, value);

            //Assert
            CheckTransmittedValues(result, value, "0,1,1,1,0,0,0,0,0,1,0");
        }

        [TestMethod]
        public void Test_GetTransmittedValuesFor_158()
        {
            //Setup
            string[] input = Resources.Day25_Input.Split('\n');
            int value = 158;

            //Action
            var result = Day25_ClockSignal.GetTransmittedValuesFor(input, value);

            //Assert
            CheckTransmittedValues(result, value);
        }

        private void CheckTransmittedValues(IEnumerable<int> result, int value, string expected = null)
        {
            string resultString = string.Join(",", result.Select(i => i.ToString()));
            Console.WriteLine($"Values transmitted for initial value {value} are {resultString}");

            if (expected != null)
            {
                Assert.AreEqual(expected, resultString);
            }
            else
            {
                int previous = 1;
                foreach (var item in result)
                {
                    if ((previous == 0 && item == 1) || (previous == 1 && item == 0))
                    {
                        previous = item;
                    }
                    else
                    {
                        Assert.Fail($"Expected {item} to be {(previous == 1 ? 0 : 1)}");
                    }
                }
            }
        }
    }
}