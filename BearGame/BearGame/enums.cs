using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public struct CellPosition
    {
        public int Col;
        public int Row;

        public CellPosition(int column = 0, int row = 0)
        {
            Col = column;
            Row = row;
        }

        public Vector2 ToPixelPosition()
        {
            return new Vector2(Col * World.TileSize, Row * World.TileSize);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is CellPosition)
            {
                var b = (CellPosition)obj;
                return Col == b.Col && Row == b.Row;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Col.GetHashCode() + Row.GetHashCode();
        }

        public static bool operator ==(CellPosition a, CellPosition b)
        {
            return a.Col == b.Col && a.Row == b.Row;
        }

        public static bool operator !=(CellPosition a, CellPosition b)
        {
            return a.Col != b.Col || a.Row != b.Row;
        }

        public static CellPosition operator + (CellPosition a, CellPosition b)
        {
            return new CellPosition(a.Col + b.Col, a.Row + b.Row);
        }

        public static CellPosition operator -(CellPosition a, CellPosition b)
        {
            return new CellPosition(a.Col - b.Col, a.Row - b.Row);
        }
    }

    public enum Direction
    {
        Down = 0,
        Up = 1,
        Right = 2,
        Left = 3,
    }
}

