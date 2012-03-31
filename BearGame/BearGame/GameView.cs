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
        SpriteBatch _uiBatch;
        World _world;

        Texture2D _meterTexture;

        public GameView(World world)
        {
            
            _world = world;
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _uiBatch = new SpriteBatch(device);

            _meterTexture = content.Load<Texture2D>("UI\\Meter");
        }

        public void Update(GameTime time)
        {
        }

        public void Draw()
        {
            var vw = 800;
            var vh = 600;
            var ws = 400;

            var gameViewLeft = (vw-ws)/2;
            var gameViewTop = (vh-ws)/2;

            _uiBatch.Begin();
            _uiBatch.Draw(_meterTexture, new Rectangle(gameViewLeft, gameViewTop - 90, _meterTexture.Width, _meterTexture.Height), Color.White);
            _uiBatch.End();

            _world.Draw(new Rectangle(gameViewLeft, gameViewTop, ws, ws));
        }
    }
}
