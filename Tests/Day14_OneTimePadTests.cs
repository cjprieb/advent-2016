using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Tests
{
    [TestClass]
    public class Day14_OneTimePadTests
    {
        [TestMethod]
        public void Test_GenerateKeys()
        {
            //Setup
            string salt = "abc";

            //Action
            var keys = Day14_OneTimePad.GenerateKeys(salt);

            //Assert
            Assert.IsFalse(keys.Contains(18), "list contains 18");
            Assert.IsTrue(keys.Contains(39), "list doesn't contain 39");
            Assert.AreEqual(64, keys.Count(), "not enough items in list");
            Assert.AreEqual(22728, keys.Max(), "64th item doesn't match");
        }

        [TestMethod]
        public void Answer1_GenerateKeys()
        {
            //Setup
            string salt = "zpqevtbw";

            //Action
            var keys = Day14_OneTimePad.GenerateKeys(salt);

            //Assert
            Assert.AreEqual(64, keys.Count(), "not enough items in list");
            Assert.AreEqual(16106, keys.Max(), "64th item doesn't match");
        }

        [TestMethod]
        public void Test_GenerateStretchedKeys()
        {
            //Setup
            string salt = "abc";

            //Action
            var keys = Day14_OneTimePad.GenerateKeys(salt, 2016);

            //Assert
            Assert.IsFalse(keys.Contains(5), "list contains 5");
            Assert.IsTrue(keys.Contains(10), "list doesn't contain 10");
            Assert.AreEqual(64, keys.Count(), "not enough items in list");
            Assert.AreEqual(22551, keys.Max(), "64th item doesn't match");
        }

        [TestMethod]
        public void Answer2_GenerateStretchedKeys()
        {
            //Setup
            string salt = "zpqevtbw";

            //Action
            var keys = Day14_OneTimePad.GenerateKeys(salt, 2016);

            //Assert
            Assert.AreEqual(64, keys.Count(), "not enough items in list");
            Assert.AreEqual(22423, keys.Max(), "64th item doesn't match");
        }
    }
}