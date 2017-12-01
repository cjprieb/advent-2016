using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent
{
    public class Day11_Radioisotope
    {
        public static int StepsToMoveEquipmentToTopFloor(string[] floorData)
        {
            IEnumerable<Floor> floors = floorData.Select(data => CreateFloorData(data));
            BuildingLayout building = new BuildingLayout(floors);
            MoveEquipmentToTopFloor(building);

            return building.Steps;
        }

        public static void MoveEquipmentToTopFloor(BuildingLayout building)
        {
            Floor currentFloor = building.CurrentFloor;
            Floor floorWithLonelyMicrochips = building.FindFirstFloorWithLonelyMicrochips();
            if (floorWithLonelyMicrochips != null)
            {
                foreach (var microchip in floorWithLonelyMicrochips.FloorItems)
                {
                    var match = currentFloor.FloorItems.First(item => item.CompatibleWith(microchip));
                    building.AddToElevator(match);
                }
                while (currentFloor != floorWithLonelyMicrochips)
                {
                    building.MoveUp();
                }
            }
        }
                
        #region CreateFloorData
        public static Floor CreateFloorData(string floorData)
        {
            Floor result = null;

            Match match = Regex.Match(floorData, @"The (\w+) floor contains (.+)\.");
            if (match.Success)
            {
                result = new Floor(match.Groups[1].Value);

                string equipmentList = match.Groups[2].Value;
                MatchCollection matches = Regex.Matches(equipmentList, @"(?<element>\w+)(-compatible)? (?<type>generator|microchip)");
                List<Equipment> itemList = new List<Equipment>();
                foreach (Match equipmentMatch in matches)
                {
                    string name = equipmentMatch.Groups[0].Value;
                    string element = equipmentMatch.Groups["element"].Value;
                    string type = equipmentMatch.Groups["type"].Value;
                    itemList.Add(new Equipment(name, element, type));
                }
                result.AddEquipment(itemList);
            }

            return result;
        }
        #endregion CreateFloorData

        #region Models...

        public class BuildingLayout
        {
            #region Member Variables...
            private int _CurrentFloorNumber = 0;
            private int _Steps = 0;
            private List<Equipment> _ElevatorItems = new List<Equipment>();
            private Floor[] _Floors = null;
            #endregion Member Variables...

            #region Properties...

            #region CurrentFloor
            public Floor CurrentFloor
            {
                get { return Floors[_CurrentFloorNumber]; }
            }
            #endregion CurrentFloor

            //#region ElevatorItems
            //public IEnumerable<Equipment> ElevatorItems
            //{
            //    get { return _ElevatorItems; }
            //}
            //#endregion ElevatorItems

            #region Floors
            public Floor[] Floors
            {
                get { return _Floors; }
            }
            #endregion Floors

            #region Steps
            public int Steps
            {
                get { return _Steps; }
            }
            #endregion Steps

            #endregion Properties...

            #region Constructors...

            #region BuildingLayout
            public BuildingLayout(IEnumerable<Floor> floors)
            {
                _Floors = floors.ToArray();
            }
            #endregion BuildingLayout

            #region BuildingLayout
            public BuildingLayout(int current, int steps, IEnumerable<Floor> floors)
            {
                _CurrentFloorNumber = current;
                _Steps = steps;
                _Floors = floors.ToArray();
            }
            #endregion BuildingLayout

            #endregion Constructors...

            internal void AddToElevator(Equipment item)
            {
                if (_ElevatorItems.Count == 0)
                {
                    _ElevatorItems.Add(item);
                }
                else if (_ElevatorItems.Count == 1)
                {
                    if (_ElevatorItems[0].Type != item.Type && !_ElevatorItems[0].CompatibleWith(item))
                    {
                        throw new Exception($"Cannot add {item} to elevator - not compatible with {_ElevatorItems[0]}");
                    }
                    else
                    {
                        _ElevatorItems.Add(item);
                        if (_ElevatorItems[0].CompatibleWith(item))
                        {
                            _ElevatorItems[0].PairUp(item);
                        }
                    }
                }
                else
                {
                    throw new Exception($"Cannot add {item} to elevator, it's too full");
                }
            }

            public bool AreMicrochipsAtCurrentFloorSafe()
            {
                foreach (var floorItem in CurrentFloor.FloorItems.Where(o => o.IsMicrochip && !o.IsPaired))
                {
                    return CurrentFloor.FloorItems.Any(o => o.IsGenerator);
                }
                return true;
            }

            public bool CanElevatorMove()
            {
                if (_ElevatorItems.Count == 1)
                {
                    return true;
                }
                else if (_ElevatorItems.Count == 2)
                {
                    return _ElevatorItems.All(item => item.IsGenerator) ||
                        _ElevatorItems.All(item => item.IsMicrochip) ||
                        _ElevatorItems[0].CompatibleWith(_ElevatorItems[1]);
                }
                else
                {
                    return false;
                }
            }

            public bool CanElevatorMoveDown()
            {
                return _CurrentFloorNumber > 0 && CanElevatorMoveTo(_CurrentFloorNumber - 1);
            }

            public bool CanElevatorMoveUp()
            {
                return _CurrentFloorNumber < (_Floors.Length - 1) && CanElevatorMoveTo(_CurrentFloorNumber + 1);
            }

            private bool CanElevatorMoveTo(int floor)
            {
                if (CanElevatorMove())
                {
                    Floor newFloor = _Floors[floor];
                    var allItems = _ElevatorItems.Union(newFloor.FloorItems);
                    var microchips = allItems.Where(item => item.IsMicrochip);
                    var generator = allItems.Where(item => item.IsGenerator);
                    foreach ( var microchip in microchips )
                    {
                        if ( generator.Any() && !generator.Any(gen => gen.CompatibleWith(microchip)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool MoveUp()
            {
                if (CanElevatorMove())
                {
                    _CurrentFloorNumber++;
                    return UpdateAll();
                }
                else
                {
                    //throw new Exception("nothing in elevator, can't move up");
                    return false;
                }
            }
            

            public bool MoveDown()
            {
                if (CanElevatorMove())
                {
                    _CurrentFloorNumber--;
                    return UpdateAll();
                }
                else
                {
                    //throw new Exception("nothing in elevator, can't move down");
                    return false;
                }
            }

            private bool UpdateAll()
            {
                _Steps++;
                CurrentFloor.AddEquipment(_ElevatorItems);
                _ElevatorItems.Clear();
                return AreMicrochipsAtCurrentFloorSafe();
            }

            public BuildingLayout Clone()
            {
                return new BuildingLayout(_CurrentFloorNumber, Steps, Floors);
            }

            internal Floor FindFirstFloorWithLonelyMicrochips()
            {
                Floor floor = null;
                for (int i = 0; i < _Floors.Length; i++)
                {
                    if (_Floors[i].FloorItems.Any(item => item.IsMicrochip && !item.IsPaired))
                    {
                        floor = _Floors[i];
                        break;
                    }
                }
                return floor;
            }
        }

        public class Floor
        {
            #region Properties...
            public int FloorNumber { get; private set; }
            public List<Equipment> FloorItems { get; private set; }
            public string OrdinalNumber { get; private set; }
            #endregion Properties...

            #region Constructors...
            #region Floor
            public Floor(string ordinalNumber)
            {
                if (ordinalNumber.Equals("first", StringComparison.CurrentCultureIgnoreCase))
                {
                    FloorNumber = 0;
                }
                else if (ordinalNumber.Equals("second", StringComparison.CurrentCultureIgnoreCase))
                {
                    FloorNumber = 1;
                }
                if (ordinalNumber.Equals("third", StringComparison.CurrentCultureIgnoreCase))
                {
                    FloorNumber = 2;
                }
                if (ordinalNumber.Equals("fourth", StringComparison.CurrentCultureIgnoreCase))
                {
                    FloorNumber = 3;
                }
                OrdinalNumber = ordinalNumber;
                FloorItems = new List<Equipment>();
            }
            #endregion Floor
            #endregion Constructors...

            internal Equipment FindMatch(Equipment equipment)
            {
                Equipment matchingItem = null;
                foreach (Equipment item in FloorItems)
                {
                    if (item.CompatibleWith(equipment))
                    {
                        matchingItem = item;
                        break;
                    }
                }
                return matchingItem;
            }

            internal void AddEquipment(IEnumerable<Equipment> equipment)
            {
                foreach (var item in equipment)
                {
                    foreach (var floorItem in FloorItems)
                    {
                        if (item.CompatibleWith(floorItem))
                        {
                            item.PairUp(floorItem);
                        }
                    }
                    FloorItems.Add(item);
                }
            }

            internal void RemoveEquipment(Equipment item)
            {
                item.SplitUp();
                FloorItems.Remove(item);
            }
        }

        public class Equipment
        {
            public string Name { get; private set; }
            public EquipmentType Type { get; private set; }
            public string ElementType { get; private set; }
            public Equipment Pair { get; set; }
            public bool IsPaired
            {
                get { return Pair != null; }
            }

            public bool IsMicrochip { get { return Type == EquipmentType.Microchip; } }

            public bool IsGenerator { get { return Type == EquipmentType.Generator; } }

            public Equipment(string name, string element, string type)
            {
                if (type.Equals("microchip", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = EquipmentType.Microchip;
                }
                else if (type.Equals("generator", StringComparison.CurrentCultureIgnoreCase))
                {
                    Type = EquipmentType.Generator;
                }
                else
                {
                    throw new ArgumentException($"Invalid Equipment Type: {type}");
                }
                Name = name;
                ElementType = element.ToLower();
            }

            internal bool CompatibleWith(Equipment equipment)
            {
                return ElementType == equipment.ElementType && Type != equipment.Type;
            }

            internal void PairUp(Equipment other)
            {
                if (CompatibleWith(other))
                {
                    other.Pair = this;
                    Pair = other;
                }
                else
                {
                    throw new ArgumentException($"Cannot pair {this} with {other}");
                }
            }

            internal void SplitUp()
            {
                if (Pair != null)
                {
                    Pair.Pair = null;
                    Pair = null;
                }
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public enum EquipmentType
        {
            Generator, Microchip
        }

        #endregion Models
    }
}
