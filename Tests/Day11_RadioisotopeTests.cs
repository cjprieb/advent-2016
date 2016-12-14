using Microsoft.VisualStudio.TestTools.UnitTesting;
using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Properties;
using static Advent.Day11_Radioisotope;

namespace Advent.Tests
{
    [TestClass]
    public class Day11_RadioisotopeTests
    {
        [TestMethod]
        public void Test_CreateFloor_1()
        {
            //Setup
            string floorData = "The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.";

            //Action
            Floor floor = CreateFloorData(floorData);

            //Assert
            Assert.AreEqual(1, floor.FloorNumber, "floor number mismatch");
            Assert.AreEqual(2, floor.FloorItems.Count, "floor item count mismatch");
            Assert.AreEqual("hydrogen", floor.FloorItems[0].ElementType, "floor item element mismatch");
            Assert.AreEqual(EquipmentType.Microchip, floor.FloorItems[0].Type, "floor item type mismatch");

            //The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
            //The second floor contains a hydrogen generator.
            //The fourth floor contains nothing relevant.
        }
        [TestMethod]
        public void Test_CreateFloor_2()
        {
            //Setup
            string floorData = "The second floor contains a hydrogen generator.";

            //Action
            Floor floor = CreateFloorData(floorData);

            //Assert
            Assert.AreEqual(2, floor.FloorNumber, "floor number mismatch");
            Assert.AreEqual(1, floor.FloorItems.Count, "floor item count mismatch");
            Assert.AreEqual("hydrogen", floor.FloorItems[0].ElementType, "floor item element mismatch");
            Assert.AreEqual(EquipmentType.Generator, floor.FloorItems[0].Type, "floor item type mismatch");
        }
        [TestMethod]
        public void Test_CreateFloor_4()
        {
            //Setup
            string floorData = "The fourth floor contains nothing relevant.";

            //Action
            Floor floor = CreateFloorData(floorData);

            //Assert
            Assert.AreEqual(4, floor.FloorNumber, "floor number mismatch");
            Assert.AreEqual(0, floor.FloorItems.Count, "floor item count mismatch");
        }

        [TestMethod]
        public void Test_StepsToMoveEquipmentToTopFloor()
        {
            //Setup
            string[] floors = Resources.Day11_TestInput.Split('\n');

            //Action
            int steps = StepsToMoveEquipmentToTopFloor(floors);

            //Assert
            Assert.AreEqual(11, steps);
        }

        [TestMethod]
        public void Answer_StepsToMoveEquipmentToTop()
        {
            //Setup
            string[] floors = Resources.Day11_Input.Split('\n');

            //Action
            int steps = StepsToMoveEquipmentToTopFloor(floors);

            //Assert
            Assert.AreEqual(11, steps);
        }
    }
}