using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Properties;
using static Advent.Day4_RoomDirectory;

namespace Advent.Tests
{
    [TestClass()]
    public class Day4_RoomDirectoryTests
    {
        [TestMethod]
        public void Test_SumOfValidRoomIds()
        {
            //Setup
            string[] input =
            {
                "aaaaa-bbb-z-y-x-123[abxyz]",
                "a-b-c-d-e-f-g-h-987[abcde]",
                "not-a-real-room-404[oarel]",
                "totally-real-room-200[decoy]"
            };

            //Action
            int sum = Day4_RoomDirectory.SumOfValidRoomIds(input);

            //Assert
            Assert.AreEqual(1514, sum, "Sum mismatch");
        }

        [TestMethod]
        public void Test_IsValidRoom_abxyz()
        {
            //Setup - aaaaa-bbb-z-y-x-123[abxyz]
            string input = "aaaaa-bbb-z-y-x";
            string checksum = "abxyz";

            //Action
            bool isValid = Day4_RoomDirectory.IsValidRoom(input, checksum);

            //Assert
            Assert.IsTrue(isValid, $"{input} did not match {checksum}");
        }

        [TestMethod]
        public void Test_IsValidRoom_abcde()
        {
            //Setup - a-b-c-d-e-f-g-h-987[abcde]
            string input = "a-b-c-d-e-f-g-h";
            string checksum = "abcde";

            //Action
            bool isValid = Day4_RoomDirectory.IsValidRoom(input, checksum);

            //Assert
            Assert.IsTrue(isValid, $"{input} did not match {checksum}");
        }

        [TestMethod]
        public void Test_IsValidRoom_oarel()
        {
            //Setup - not-a-real-room-404[oarel]
            string input = "not-a-real-room";
            string checksum = "oarel";

            //Action
            bool isValid = Day4_RoomDirectory.IsValidRoom(input, checksum);

            //Assert
            Assert.IsTrue(isValid, $"{input} did not match {checksum}");
        }

        [TestMethod]
        public void Test_IsValidRoom_decoy()
        {
            //Setup - totally-real-room-200[decoy]
            string input = "totally-real-room";
            string checksum = "decoy";

            //Action
            bool isValid = Day4_RoomDirectory.IsValidRoom(input, checksum);

            //Assert
            Assert.IsFalse(isValid, $"{input} wrongly matched {checksum}");
        }

        [TestMethod]
        public void Answer1_SumOfValidRoomIds()
        {
            //Setup
            string[] input = Resources.Day4_Input.Split('\n');

            //Action
            int sum = Day4_RoomDirectory.SumOfValidRoomIds(input);

            //Assert
            Console.WriteLine($"The sum of the sector IDs of valid rooms is [{sum}]");
            Assert.AreEqual(158835, sum, "Sum mismatch");
        }

        [TestMethod]
        public void Test_DecryptName_qzmt()
        {
            //Setup - qzmt-zixmtkozy-ivhz-343
            string input = "qzmt-zixmtkozy-ivhz";
            int rotation = 343;

            //Action
            string decryptedName = Day4_RoomDirectory.DecryptName(input, rotation);

            //Assert
            Assert.AreEqual("very encrypted name", decryptedName, $"decryption mismatch");
        }

        [TestMethod()]
        public void Answer2_FindRoom()
        {
            //Setup
            string[] input = Resources.Day4_Input.Split('\n');
            string searchString = "north pole";

            //Action
            List<Room> matchingRooms = Day4_RoomDirectory.FindRooms(input, searchString);

            //Assert
            Assert.AreEqual(1, matchingRooms.Count, "count mismatch");

            int sectorId = matchingRooms[0].SectorId;
            Console.WriteLine($"North Pole objects are stored in room [{sectorId}]");
            Assert.AreEqual(993, sectorId, "Id mismatch");
        }
    }
}