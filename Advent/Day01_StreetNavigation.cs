using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Directions;

namespace Advent
{
    public class Day01_StreetNavigation
    {
        public static BlockPosition GetLocation(string[] directions)
        {
            BlockPosition Result = new BlockPosition();
            
            foreach (string str in directions)
            {
                string RotationType = str.Substring(0, 1);
                int BlockCount = int.Parse(str.Substring(1));
                Result = Result.UpdatePosition(BlockCount, RotationType);
            }

            return Result;
        }

        public static Point GetFirstLocationVisitedTwice(string[] directions)
        {
            BlockPosition CurrentPosition = new BlockPosition();
            Dictionary<Point, int> PlacesVisited = new Dictionary<Point, int>();

            foreach (string str in directions)
            {
                string RotationType = str.Substring(0, 1);
                int BlockCount = int.Parse(str.Substring(1));
                CurrentPosition = CurrentPosition.Rotate(RotationType);

                for (int i = 0; i < BlockCount; i++)
                {
                    CurrentPosition = CurrentPosition.UpdatePosition(1);
                    if (PlacesVisited.ContainsKey(CurrentPosition.Position))
                    {
                        PlacesVisited[CurrentPosition.Position] += 1;
                    }
                    else
                    {
                        PlacesVisited[CurrentPosition.Position] = 1;
                    }
                }
            }

            return PlacesVisited.Where(kvp => kvp.Value >= 2).Select(kvp => kvp.Key).FirstOrDefault();
        }

        public struct BlockPosition
        {
            private static List<CardinalDirection> _RightDirectionOrder = new List<CardinalDirection>()
            {
                CardinalDirection.North,
                CardinalDirection.East,
                CardinalDirection.South,
                CardinalDirection.West
            };

            public Point Position { get; }

            public CardinalDirection FacingDirection { get; }

            public int TotalBlocks 
            {
                get
                {
                    return Position.BlockDistanceFromCenter;
                }
            }

            public BlockPosition(CardinalDirection direction, int x, int y)
            {
                Position = new Point(x, y);
                FacingDirection = direction;
            }

            public BlockPosition(CardinalDirection direction, Point position)
            {
                Position = position;
                FacingDirection = direction;
            }

            public BlockPosition UpdatePosition(int blocks, string rotationType = null)
            {
                CardinalDirection NewDirection = FacingDirection;
                int newXCoord = Position.X;
                int newYCoord = Position.Y;
                if( rotationType != null)
                {
                    NewDirection = Rotate(rotationType).FacingDirection;
                }
                switch (NewDirection)
                {
                    case CardinalDirection.East:  newXCoord = newXCoord + blocks; break;
                    case CardinalDirection.North: newYCoord = newYCoord + blocks; break;
                    case CardinalDirection.South: newYCoord = newYCoord - blocks; break;
                    case CardinalDirection.West:  newXCoord = newXCoord - blocks; break;
                }
                return new BlockPosition(NewDirection, newXCoord, newYCoord);
            }

            public BlockPosition Rotate(string rotationType)
            {
                int offset = _RightDirectionOrder.IndexOf(FacingDirection);
                if (rotationType.Equals("R", StringComparison.OrdinalIgnoreCase))
                {
                    offset = (offset + 1) % _RightDirectionOrder.Count;
                }
                else if (rotationType.Equals("L", StringComparison.OrdinalIgnoreCase))
                {
                    offset = (_RightDirectionOrder.Count + offset - 1) % _RightDirectionOrder.Count;
                }
                return new BlockPosition(_RightDirectionOrder[offset], Position);
            }

            public override bool Equals(object obj)
            {
                if(obj is BlockPosition)
                {
                    BlockPosition position = (BlockPosition)obj;
                    return position.Position.Equals(Position) && FacingDirection == position.FacingDirection;
                }
                else
                {
                    return false;
                }                     
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public override string ToString()
            {
                return $"{Position},{FacingDirection}";
            }
        }
    }
}
