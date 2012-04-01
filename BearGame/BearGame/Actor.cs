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
        }

        public Interaction ActiveInteraction { get; private set; }

        public void BeginInteraction(GameTime time, Interaction inter)
        {
            ActiveInteraction = inter;
            inter.OnBegin(this, time);

            if (!inter.IsActive)
            {
                inter.OnEnd(this, time);
                ActiveInteraction = null;
            }

            UpdateSpriteIndex();
        }

        public void EndInteraction(GameTime time)
        {
            if (ActiveInteraction != null)
            {
                ActiveInteraction.OnEnd(this, time);
                ActiveInteraction = null;
                UpdateSpriteIndex();
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (ActiveInteraction != null)
            {
                if (ActiveInteraction.IsActive)
                {
                    ActiveInteraction.Update(this, time);

                    if (!ActiveInteraction.IsActive)
                    {
                        EndInteraction(time);
                    }
                }
                else
                {
                    ActiveInteraction = null;
                }
            }
        }

        double _lastMoveTime = 0;
        protected double LastMoveTime { get { return _lastMoveTime; } }

        public virtual void MoveCell(GameTime time, CellPosition diff)
        {
            MoveToCell(time, c_position + diff);
        }

        public virtual void MoveToCell(GameTime time, CellPosition newPos)
        {
            if (World.IsPassable(newPos))
            {
                c_position = newPos;
                Position = newPos.ToPixelPosition();
                _lastMoveTime = time.TotalGameTime.TotalSeconds;
            }
        }
    }
}
