using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    class GameView
    {
        World _world;

        public GameView(World world, GameSetting settings)
        {
            _world = world;
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime time)
        {
        }

        public void Draw()
        {
            var vw = 800;
            var vh = 600;
            var ws = 400;
            _world.Draw(new Rectangle((vw-ws)/2, (vh-ws)/2, ws, ws));
        }
    }
}
