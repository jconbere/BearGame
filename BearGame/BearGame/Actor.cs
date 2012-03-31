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

        public Actor(World world)
            : base(world)
        {
            IsVisible = true;
        }

        public bool IsVisible { get; set; }

        public Interaction CurrentInteraction { get; private set; }

        public void BeginInteraction(GameTime time, Interaction inter)
        {
            CurrentInteraction = inter;
            inter.Begin(this, time);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (CurrentInteraction != null)
            {
                if (CurrentInteraction.IsActive)
                {
                    CurrentInteraction.Update(this, time);
                }
                else
                {
                    CurrentInteraction = null;
                }
            }
        }

        double _lastMoveTime = 0;
        protected double LastMoveTime { get { return _lastMoveTime; } }

        protected void MoveCell(GameTime time, CellPosition diff)
        {
            var newPos = c_position + diff;
            if (World.IsPassable(newPos))
            {
                c_position = newPos;
                Position = newPos.ToPixelPosition();
                _lastMoveTime = time.TotalGameTime.TotalSeconds;
            }
        }
    }
}
