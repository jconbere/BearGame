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
        }

        double _lastMoveTime = 0;

        public override void Update(GameTime time, World world)
        {
            var keyState = Keyboard.GetState();
            var now = time.TotalGameTime.TotalSeconds;

            if ((now - _lastMoveTime) > Settings.Bear_MoveInterval)
            {
                if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                {
                    Position = Position + new Vector2(-World.TileSize, 0);
                    _lastMoveTime = now;
                }
                else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    Position = Position + new Vector2(0, -World.TileSize);
                    _lastMoveTime = now;
                }
                else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                {
                    Position = Position + new Vector2(World.TileSize, 0);
                    _lastMoveTime = now;
                }
                else if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                {
                    Position = Position + new Vector2(0, World.TileSize);
                    _lastMoveTime = now;
                }
                
            }
        }
    }
}
