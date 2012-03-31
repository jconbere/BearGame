using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearGame
{
    public class CollisionLayer : Layer
    {
        public bool IsPassable(int column, int row)
        {
            return GetTile(column, row) != '0';
        }
    }
}

