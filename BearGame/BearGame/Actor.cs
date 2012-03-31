using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    public class Actor : Entity
    {
        Direction _facingDirection;
        public Direction FacingDirection
        {
            get { return _facingDirection; }
            set
            {
                _facingDirection = value;
                UpdateSpriteIndex();
            }
        }

        public Actor(GameSetting settings)
            : base(settings)
        {
        }
    }
}
