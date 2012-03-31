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
        Texture2D _skullTexture;
        Texture2D _happyTexture;
        Texture2D _healthbarTexture;

        public GameView(World world)
        {
            
            _world = world;
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _uiBatch = new SpriteBatch(device);

            _meterTexture = content.Load<Texture2D>("UI\\health_meter");
            _healthbarTexture = content.Load<Texture2D>("UI\\healthbar");
            _skullTexture = content.Load<Texture2D>("UI\\skull");
            _happyTexture = content.Load<Texture2D>("UI\\happy");
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
            float healthPercentage = (_world.Bear.Health / _world.Settings.Bear_HealthMax);
            
            _uiBatch.Begin();
            _uiBatch.Draw(_skullTexture, new Rectangle(gameViewLeft, gameViewTop - 90, _skullTexture.Width, _skullTexture.Height), Color.White);
            _uiBatch.Draw(_meterTexture, new Rectangle(gameViewLeft + _skullTexture.Width, gameViewTop - 90, (int)(_meterTexture.Width * healthPercentage), _meterTexture.Height), Color.Red);
            _uiBatch.Draw(_healthbarTexture, new Rectangle(gameViewLeft + _skullTexture.Width, gameViewTop - 90, _healthbarTexture.Width, _healthbarTexture.Height), Color.White);
            _uiBatch.Draw(_happyTexture, new Rectangle(gameViewLeft + _healthbarTexture.Width + _skullTexture.Width, gameViewTop - 90, _happyTexture.Width, _happyTexture.Height), Color.White);
            _uiBatch.End();

            _world.Draw(new Rectangle(gameViewLeft, gameViewTop, ws, ws));
        }
    }
}
