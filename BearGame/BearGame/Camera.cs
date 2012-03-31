using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Camera
    {
        public const int VisibleSize = 9;

        public Vector2 CenterPosition
        {
            get;
            set;
        }
    }
}
