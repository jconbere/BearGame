using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace BearGame
{
    class GameState
    {

        public GameState()
        {

        }


        public virtual void Initialize()
        {

        }

        public virtual void LoadContent(GraphicsDevice graphics, ContentManager Content)
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
        protected Camera camera;
        protected World world;
        protected GameView view;

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

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            Villager.HurtSound = new RandomSound(Content, "Audio\\person_hurt_01", "Audio\\person_hurt_02", "Audio\\person_hurt_03", "Audio\\person_hurt_04", "Audio\\person_hurt_05");
            Villager.HurtBadSound = new RandomSound(Content, "Audio\\person_hurt_bad_03", "Audio\\person_hurt_bad_04", "Audio\\person_hurt_bad_05", "Audio\\person_hurt_bad_06");
            TakeHoney.PickUpHoneySound = new RandomSound(Content, "Audio\\bear_pick_up_honey_02");
            EatHoney.EatHoneySound = new RandomSound(Content, "Audio\\bear_eat_honey_02");
            GiveHoney.PersonThanksSound = new RandomSound(Content, "Audio\\person_thanks_01", "Audio\\person_thanks_02");
            RideTricycle.GetOnSound = new RandomSound(Content, "Audio\\bear_get_on_off_tricycle_03");
            GetOffTricycle.GetOffSound = new RandomSound(Content, "Audio\\bear_get_on_off_tricycle_03");
            Bear.PersonAwwSound = new RandomSound(Content, "Audio\\person_aww_03", "Audio\\person_aww_04");
            Entity.emotes = Content.Load<Texture2D>("Sprites\\Emotes");

            view.LoadContent(graphics, Content);

            var worldTiles = Content.Load<Texture2D>("Sprites\\WorldTiles");
            world.LoadContent(graphics, Content, 1);
        }

        public override void UnloadContent()
        {

        }
        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            view.Draw();
        }
    }
}
