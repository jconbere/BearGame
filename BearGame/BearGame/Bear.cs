using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BearGame
{
    class Bear : Actor
    {
        public int Health;
        public bool HasRibbon = false;
        public bool HasTricycle = false;
        public bool HasHoney = false;

        public Bear(GameSetting settings)
            : base(settings)
        {
            this.Health = settings.Bear_HealthDefault;
            FacingDirection = Direction.Down;
        }

        double _lastMoveTime = 0;

        public override void Update(GameTime time, World world)
        {
            var now = time.TotalGameTime.TotalSeconds;

            Action<CellPosition> MoveCell = delegate(CellPosition diff)
            {
                var newPos = c_position + diff;
                if (world.IsPassable(newPos))
                {
                    c_position = newPos;
                    Position = newPos.ToPixelPosition();
                    _lastMoveTime = now;
                }
            };

            var keyState = Keyboard.GetState();            

            if ((now - _lastMoveTime) > Settings.Bear_MoveInterval)
            {
                if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                {
                    MoveCell(new CellPosition(-1, 0));
                    FacingDirection = Direction.Left;
                }
                else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    MoveCell(new CellPosition(0, -1));
                    FacingDirection = Direction.Up;
                }
                else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                {
                    MoveCell(new CellPosition(1, 0));
                    FacingDirection = Direction.Right;
                }
                else if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                {
                    MoveCell(new CellPosition(0, 1));
                    FacingDirection = Direction.Down;
                }                
            }
        }

        protected override void  UpdateSpriteIndex()
        {
            spriteIndex = 16 * (int)FacingDirection;
        }
    }
}
