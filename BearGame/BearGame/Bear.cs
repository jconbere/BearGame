using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BearGame
{
    public class Bear : Actor
    {
        public int Health { get; set; }

        public Prop Inventory { get; set; }

        public Bear(World world)
            : base(world)
        {
            this.Health = Settings.Bear_HealthDefault;
            FacingDirection = Direction.Down;
        }

        public override void Update(GameTime time)
        {
            var now = time.TotalGameTime.TotalSeconds;

            var keyState = Keyboard.GetState();            

            if ((now - LastMoveTime) > Settings.Bear_MoveInterval)
            {
                if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                {
                    MoveCell(time, new CellPosition(-1, 0));
                    FacingDirection = Direction.Left;
                }
                else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    MoveCell(time, new CellPosition(0, -1));
                    FacingDirection = Direction.Up;
                }
                else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                {
                    MoveCell(time, new CellPosition(1, 0));
                    FacingDirection = Direction.Right;
                }
                else if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                {
                    MoveCell(time, new CellPosition(0, 1));
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
