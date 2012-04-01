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

        }

        public void EndInteraction(GameTime time)
        {
            if (ActiveInteraction != null)
            {
                ActiveInteraction.OnEnd(this, time);
                ActiveInteraction = null;
            }
        }

        Vector2 _transitionFromPosition;
        Vector2 _transitionToPosition;
        double _transitionStartTime = 0;

        public override void Update(GameTime time)
        {
            base.Update(time);

            var now = time.TotalGameTime.TotalSeconds;

            //
            // Interactions update
            //
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

            //
            // Move transition
            //
            var transF = (float)((now - _transitionStartTime) / TransitionInterval);
            if (0 <= transF && transF <= 1) {
                Position = (_transitionToPosition - _transitionFromPosition) * transF + _transitionFromPosition;
            }
            else {
                Position = c_position.ToPixelPosition();
            }            

            //
            // Sprite index update
            //
            UpdateSpriteIndex();
        }

        double _lastMoveTime = 0;
        protected double LastMoveTime { get { return _lastMoveTime; } }

        public void MoveCell(GameTime time, CellPosition diff)
        {
            MoveToCell(time, c_position + diff);
        }

        protected virtual double TransitionInterval { get { return 1.0 / 3; } }

        public void MoveToCell(GameTime time, CellPosition newPos)
        {
            var now = time.TotalGameTime.TotalSeconds;

            if (newPos != c_position)
            {
                if (World.IsPassable(newPos))
                {
                    _transitionFromPosition = Position;
                    _transitionToPosition = newPos.ToPixelPosition();
                    _transitionStartTime = now;

                    c_position = newPos;

                    _lastMoveTime = time.TotalGameTime.TotalSeconds;
                    OnMove(time);
                }
            }
        }

        protected virtual void OnMove(GameTime time)
        {
        }
    }
}
