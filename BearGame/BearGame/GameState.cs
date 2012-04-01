using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    class GameState
    {
        protected Camera camera;
        protected World world;
        protected GameView view;

        public GameState()
        {

        }


        public virtual void Initialize()
        {

        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }

    class MainGame:GameState
    {
        public MainGame()
        {

        }


        public override void Initialize()
        {
            var settings = new GameSetting();

            camera = new Camera();
            world = new World(settings);
            view = new GameView(world);
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
