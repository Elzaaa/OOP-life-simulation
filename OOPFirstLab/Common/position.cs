using System;
using System.Collections.Generic;

namespace OOPFirstLab.Common
{
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public static bool operator ==(Position p1, Position p2)
        {
            if (ReferenceEquals(p1, p2))
                return true;

            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null))
                return false;

            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }

        public Position GetNeaghborPosition(MoveDirection md)
        {
            int x = X;
            int y = Y;
            switch (md)
            {
                case MoveDirection.Up:
                    y -= 1;
                    break;
                case MoveDirection.RightUp:
                    x += 1;
                    y -= 1;
                    break;
                case MoveDirection.Right:
                    x += 1;
                    break;
                case MoveDirection.RightDown:
                    x += 1;
                    y += 1;
                    break;
                case MoveDirection.Down:
                    y += 1;
                    break;
                case MoveDirection.LeftDown:
                    x -= 1;
                    y += 1;
                    break;
                case MoveDirection.Left:
                    x -= 1;
                    break;
                case MoveDirection.LeftUp:
                    x -= 1;
                    y -= 1;
                    break;
            }

            return new Position(x, y);
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public Position GetPosOnWayTo(Position targetPos)
        {
            int xOffset = X < targetPos.X ? 1 : X > targetPos.X ? -1 : 0;
            int yOffset = Y < targetPos.Y ? 1 : Y > targetPos.Y ? -1 : 0;
            return new Position(X + xOffset, Y + yOffset);
        }

        public IEnumerable<Position> GetPositionsAtDistance(int distance, int minX, int maxX, int minY, int maxY)
        {
            bool InBounds(int x, int y)
            {
                return x >= minX && x <= maxX && y >= minY && y <= maxY;
            }

            if (distance == 0)
            {
                yield return this;
            }
            else if (distance > 0)
            {
                int currX = X - distance;
                int currY = Y - distance;

                // Бежим по верхнему краю квадрата
                for (; currX < X + distance; ++currX)
                {
                    if (InBounds(currX, currY))
                        yield return new Position(currX, currY);
                }

                // Бежим по правому краю квадрата
                for (; currY < Y + distance; ++currY)
                {
                    if (InBounds(currX, currY))
                        yield return new Position(currX, currY);
                }

                // Бежим по нижнему краю квадрата
                for (; currX > X - distance; --currX)
                {
                    if (InBounds(currX, currY))
                        yield return new Position(currX, currY);
                }

                // Бежим по левому краю квадрата
                for (; currY > Y - distance; --currY)
                {
                    if (InBounds(currX, currY))
                        yield return new Position(currX, currY);
                }
            }
        }

        public int DistanceTo(Position pos)
        {
            int xOffset = Math.Abs(X - pos.X);
            int yOffset = Math.Abs(Y - pos.Y);
            return Math.Max(xOffset, yOffset);
        }

        public override string ToString()
        {
            return $"Position({X},{Y})";
        }
    }
}
