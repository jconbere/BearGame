using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace BearGame
{
    class GameState
    {
        public int requestedState{ get; protected set;}

        public GameState()
        {
            requestedState = -1;
        }
        public virtual void Initialize()
        {

        }

        public virtual void Initialize(GameTime gameTime)
        {

        }

        public virtual void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            
        }

        public virtual void UnloadContent(ContentManager Content)
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }
        public int SwitchStates()
        {
            return requestedState;
        }
    }

    class MainGame:GameState
    {
        protected Camera camera;
        protected World world;
        protected GameView view;
        protected int totalLove;

        public MainGame()
        {
            requestedState = -1;
        }


        public override void Initialize(GameTime gameTime)
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
            Villager.DeadSound = new RandomSound(Content, "Audio\\spine_crunch_01", "Audio\\spine_crunch_02", "Audio\\spine_crunch_03");
            TakeHoney.PickUpHoneySound = new RandomSound(Content, "Audio\\bear_pick_up_honey_02");
            EatHoney.EatHoneySound = new RandomSound(Content, "Audio\\bear_eat_honey_02");
            GiveHoney.PersonThanksSound = new RandomSound(Content, "Audio\\person_thanks_01", "Audio\\person_thanks_02");
            RideTricycle.GetOnSound = new RandomSound(Content, "Audio\\bear_get_on_off_tricycle_03");
            GetOffTricycle.GetOffSound = new RandomSound(Content, "Audio\\bear_get_on_off_tricycle_03");
            Daredevil.DaredevilSound = new RandomSound(Content, "Audio\\thump_01", "Audio\\thump_02", "Audio\\thump_03");
            RunOver.TricycleBodyImpactSound = new RandomSound(Content, "Audio\\tricycle_body_impact_01");
            Achievement.AchievementSound = new RandomSound(Content, "Audio\\achivement_unlocked_01");
            Honey._beehiveBuzz = Content.Load<SoundEffect>("Audio\\beehive_buzz_01");

            Bear.PersonAwwSound = new RandomSound(Content, "Audio\\person_aww_03", "Audio\\person_aww_04");
            Bear.FootstepSound = new RandomSound(Content, 0.5f, "Audio\\footstep_01", "Audio\\footstep_02", "Audio\\footstep_03", "Audio\\footstep_04", "Audio\\footstep_05");
            //Bear.DragSound = new RandomSound(Content, "Audio\\dragging_01", "Audio\\dragging_02", "Audio\\dragging_03");
            Bear.DragSound = new RandomSound(Content, 0.5f, "Audio\\thump_03");
            Bear.TrikeSound = new RandomSound(Content, "Audio\\tricycle_squeak_01", "Audio\\tricycle_squeak_02", "Audio\\tricycle_squeak_03", "Audio\\tricycle_squeak_04", "Audio\\tricycle_squeak_05");
            Entity.emotes = Content.Load<Texture2D>("Sprites\\Emotes");

            view.LoadContent(graphics, Content);

            var worldTiles = Content.Load<Texture2D>("Sprites\\WorldTiles");
            world.LoadContent(graphics, Content, 1);
        }

        public override void UnloadContent(ContentManager Content)
        {
            requestedState = -1;
        }
        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            if (world.Bear.Health <= 0)
            {
                requestedState = 2;
            }
            else
            {
                foreach (Villager v in world.AllVillagers)
                {
                    if (v.isHugging == true)
                    {
                        requestedState = 3;
                    }
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            view.Draw();
        }
    }

    class Intro : GameState
    {
        SpriteBatch _uiBatch;
        Texture2D splashScreen;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;
        public Intro()
        {
            requestedState = -1;
        }


        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            splashScreen = Content.Load<Texture2D>("SplashUI\\introsplash");
        }

        public override void UnloadContent(ContentManager Content)
        {
            requestedState = -1;
            //Content.Unload();
        }
        public override void Update(GameTime gameTime)
        {

            var keyState = Keyboard.GetState();

            //If any keys pressed
            if (keyState.GetPressedKeys().Count<Keys>() > 0)
            {
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay || entryGameTime == 0.0)
                {
                    requestedState = 1;
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            _uiBatch.Begin();
            _uiBatch.Draw(splashScreen, new Rectangle(0, 0, 800, 600), Color.White);
            _uiBatch.End();
            
        }
    }

    class End : GameState
    {
        SpriteBatch _uiBatch;
        Texture2D splashScreen;
        const double GameStateDelay = 0.4;
        double entryGameTime = 0.0f;

        public End()
        {
            requestedState = -1;
        }


        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            splashScreen = Content.Load<Texture2D>("SplashUI\\endsplash_lose");
        }

        public override void UnloadContent(ContentManager Content)
        {
            requestedState = -1;
            //Content.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            //If any keys pressed
            if (keyState.GetPressedKeys().Count<Keys>() > 0)
            {
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay || entryGameTime == 0.0)
                {
                    requestedState = 0;
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            _uiBatch.Begin();
            _uiBatch.Draw(splashScreen, new Rectangle(0, 0, 800, 600), Color.White);
            _uiBatch.End();

        }
    }

    class EndWin : GameState
    {
        SpriteBatch _uiBatch;
        Texture2D splashScreen;
        const double GameStateDelay = 0.4;
        double entryGameTime = 0.0f;

        public EndWin()
        {
            requestedState = -1;
        }


        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            splashScreen = Content.Load<Texture2D>("SplashUI\\endsplash_win");
        }

        public override void UnloadContent(ContentManager Content)
        {
            requestedState = -1;
            //Content.Unload();
        }
        public override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            //If any keys pressed
            if (keyState.GetPressedKeys().Count<Keys>() > 0)
            {
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay || entryGameTime == 0.0)
                {
                    requestedState = 0;
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            _uiBatch.Begin();
            _uiBatch.Draw(splashScreen, new Rectangle(0, 0, 800, 600), Color.White);
            _uiBatch.End();

        }
    }
}
