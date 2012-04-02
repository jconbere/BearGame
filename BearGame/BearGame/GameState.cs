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
        public enum GameStates
        {
            INTRO, // 0
            TITLE, // 1
            MAINGAME, // 2
            ENDNORMAL, // 3
            ENDWIN, // 4
            ENDPACIFISM, // 5
            ENDKILLER, // 6
            CREDITS //7
        }

        public int requestedState{ get; protected set;}

        public GameState()
        {
            requestedState = -1;
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
        //protected int totalLove;

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
            Honey._beehiveBuzz = Content.Load<SoundEffect>("Audio\\beehive_buzz_01");
            Achievement.AchievementSound = new RandomSound(Content, "Audio\\Achievement-mp3-sound");

            Bear.bearGruntSound = new RandomSound(Content, "Audio\\bear_desperate_whining_01", "Audio\\bear_desperate_whining_02", "Audio\\bear_desperate_whining_03", "Audio\\bear_desperate_whining_04", "Audio\\bear_desperate_whining_05");
            Bear.bearHappySound = new RandomSound(Content, "Audio\\bear_happy_singing_01", "Audio\\bear_happy_singing_02", "Audio\\bear_happy_singing_03");
            Bear.PersonAwwSound = new RandomSound(Content, "Audio\\person_aww_03", "Audio\\person_aww_04");
            Bear.FootstepSound = new RandomSound(Content, 0.5f, "Audio\\footstep_01", "Audio\\footstep_02", "Audio\\footstep_03", "Audio\\footstep_04", "Audio\\footstep_05");
            //Bear.DragSound = new RandomSound(Content, "Audio\\dragging_01", "Audio\\dragging_02", "Audio\\dragging_03");
            Bear.DragSound = new RandomSound(Content, 0.25f, "Audio\\thump_03");
            Bear.TrikeSound = new RandomSound(Content, 0.25f, "Audio\\tricycle_squeak_01", "Audio\\tricycle_squeak_02", "Audio\\tricycle_squeak_03", "Audio\\tricycle_squeak_04", "Audio\\tricycle_squeak_05");
            Entity.emotes = Content.Load<Texture2D>("Sprites\\Emotes");

            view.LoadContent(graphics, Content);

            var worldTiles = Content.Load<Texture2D>("Sprites\\WorldTiles");
            world.LoadContent(graphics, Content, 1);
        }

        public override void UnloadContent(ContentManager Content)
        {
            requestedState = -1;
            world.UnloadContent(Content);
        }
        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            if (world.Bear.Health <= 0)
            {
                bool allAlive = true;
                foreach (Villager v in world.AllVillagers)
                {
                    if (v.IsDead)
                    {
                        allAlive = false;
                    }
                }
                if (allAlive)
                {
                    requestedState = (int)GameStates.ENDPACIFISM;
                    return;             
                }

                requestedState = (int)GameStates.ENDNORMAL;
                return;
            }

            foreach (Villager v in world.AllVillagers)
            {
                    if (v.isHugging == true)
                    {
                        requestedState = (int)GameStates.ENDWIN;
                        return;
                    }
                
            }

            bool allDead = true;
            foreach (Villager v in world.AllVillagers)
            {
                if (v.IsDead == false)
                {
                    allDead = false;
                }

            }

            if (allDead)
            {
                requestedState = (int)GameStates.ENDKILLER;
                return;
            }

            // get the right end game states (check for stats)
            // DO EET!

        }

        public override void Draw(GameTime gameTime)
        {
            view.Draw();
        }
    }

    class Title : GameState
    {
        SpriteBatch _uiBatch;
        Texture2D splashScreen;

        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        public Title()
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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    requestedState = (int)GameStates.MAINGAME;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
                // draw final splash
                _uiBatch.Begin();
                _uiBatch.Draw(splashScreen, new Rectangle(0, 0, 800, 600), Color.White);
                _uiBatch.End();
        }
    }

    /// <summary>
    /// the intro sequence! (text and logos and stuff)
    /// </summary>
    class Intro : GameState
    {
        SpriteBatch _uiBatch;
        SpriteFont introFont;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        List<SplashScreen> introSplashScreens;
        List<TypingTextScreen> typingTextScreens;
        int currentTextScreenIndex;
        int currentSplashScreenIndex;

        public Intro()
        {
            requestedState = -1;
        }

        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;

            typingTextScreens = new List<TypingTextScreen>();
            introSplashScreens = new List<SplashScreen>();
            currentTextScreenIndex = 0;
            currentSplashScreenIndex = 0;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            // make me a font for the intro
            introFont = Content.Load<SpriteFont>("UI\\IntroFont");

            //load images for the intro
            SplashScreen tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Steam"), 700,300, 300);
            introSplashScreens.Add(tempSplash);

            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Criware"), 500, 200, 200);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Nvidia"), 300, 200, 200);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Havok"), 200, 100, 100);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_UDK"), 200, 100, 100);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Unity"), 200, 100, 100);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_THX"), 100, 50, 50);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Apple"), 100, 50, 50);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Paint"), 100, 50, 50);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_DrPepper"), 200, 50, 50);
            introSplashScreens.Add(tempSplash);

            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Werenotused"), 2000, 500, 500);
            introSplashScreens.Add(tempSplash);
            tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\Splash_Molydeux"), 2000, 500, 500);
            introSplashScreens.Add(tempSplash);



            // testing, read from file or something later?
            TypingTextScreen temp = new TypingTextScreen("What if. . .\n\n\n\n\n", 70f, introFont);
            typingTextScreens.Add(temp);

            temp = new TypingTextScreen("What if you were a bear\nthat needed hugs\nto survive. . .\n               ", 70f, introFont);
            typingTextScreens.Add(temp);

            temp = new TypingTextScreen("...But the problem is :\nYou crush the spines\nof those you embrace?\n                      \n", 70f, introFont);
            typingTextScreens.Add(temp);

            temp = new TypingTextScreen("If you were destined to hurt\nthe ones you love...               ", 70f, introFont);
            typingTextScreens.Add(temp);
            //special case
            temp = new TypingTextScreen("... Would    \n    jealousy    \n ", 70f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    fear          ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    anger         ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    despair       ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    resntment     ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    murder        ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    forgiveness   ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    hope          ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    love          ", 10f, introFont);
            typingTextScreens.Add(temp);
            temp = new TypingTextScreen("    loneliness be...\n       ", 70f, introFont);
            typingTextScreens.Add(temp);
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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    if (currentSplashScreenIndex < introSplashScreens.Count)
                    {
                        currentSplashScreenIndex = introSplashScreens.Count;
                        entryGameTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                    else
                        requestedState = 1;
                        //requestedState = 6;
                }
            }

            // update the current texttypingscreen
            // if isDone after, select next screen
            // same with splashscreens
            if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
                if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
                    currentSplashScreenIndex++;
            }
            else if(!(currentTextScreenIndex >= typingTextScreens.Count))
            {
                typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
                if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
                    currentTextScreenIndex++;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            if (currentSplashScreenIndex < introSplashScreens.Count)
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            }
            else if (currentTextScreenIndex < typingTextScreens.Count)
            {
                //call the appropriate textypingscreen (from the list); pass the uiBatch!)
                typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            }
            else
                requestedState = (int)GameStates.TITLE;
        }
    }

    class EndKiller : GameState
    {
        SpriteBatch _uiBatch;
        SpriteFont introFont;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        List<SplashScreen> introSplashScreens;
        List<TypingTextScreen> typingTextScreens;
        int currentTextScreenIndex;
        int currentSplashScreenIndex;

        public EndKiller()
        {
            requestedState = -1;
        }

        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;

            typingTextScreens = new List<TypingTextScreen>();
            introSplashScreens = new List<SplashScreen>();
            currentTextScreenIndex = 0;
            currentSplashScreenIndex = 0;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            // make me a font for the intro
            introFont = Content.Load<SpriteFont>("UI\\UIFont");


            // testing, read from file or something later?
            TypingTextScreen temp = new TypingTextScreen("Casting about... \nFrantic and predatory...\nYour needs have left you alone.       \nThe Lonliness is       \nUNBEARABLE\n\n\n\n\n\n\n\n\n\n\n\n", 70f, introFont);
            typingTextScreens.Add(temp);

            //load images for the intro
            SplashScreen tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\endsplash_lose3"), 3000, 300, 500);
            introSplashScreens.Add(tempSplash);
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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    requestedState = (int)GameStates.TITLE;
                }
            }

            // update the current texttypingscreen
            // if isDone after, select next screen
            // same with splashscreens
            if (!(currentTextScreenIndex >= typingTextScreens.Count))
            {
                typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
                if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
                    currentTextScreenIndex++;
            }
            else if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
                if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
                    currentSplashScreenIndex++;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            if (currentTextScreenIndex < typingTextScreens.Count)
            {
                //call the appropriate textypingscreen (from the list); pass the uiBatch!)
                typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            }
            else if (currentSplashScreenIndex < introSplashScreens.Count)
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            }
            else
                requestedState = 1;
        }
    }


    class EndPacifism : GameState
    {
        SpriteBatch _uiBatch;
        SpriteFont introFont;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        List<SplashScreen> introSplashScreens;
        List<TypingTextScreen> typingTextScreens;
        int currentTextScreenIndex;
        int currentSplashScreenIndex;

        public EndPacifism()
        {
            requestedState = -1;
        }

        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;

            typingTextScreens = new List<TypingTextScreen>();
            introSplashScreens = new List<SplashScreen>();
            currentTextScreenIndex = 0;
            currentSplashScreenIndex = 0;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            // make me a font for the intro
            introFont = Content.Load<SpriteFont>("UI\\UIFont");


            // testing, read from file or something later?
            TypingTextScreen temp = new TypingTextScreen("In taking the selfless path...   \nIn sparing those for whom you yearn...\nThe Lonliness is       \nUNBEARABLE              ", 70f, introFont);
            typingTextScreens.Add(temp);

            //load images for the intro
            SplashScreen tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\endsplash_lose"), 3000, 300, 500);
            introSplashScreens.Add(tempSplash);
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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    requestedState = (int)GameStates.TITLE;
                }
            }

            // update the current texttypingscreen
            // if isDone after, select next screen
            // same with splashscreens
            
            if (!(currentTextScreenIndex >= typingTextScreens.Count))
            {
                typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
                if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
                    currentTextScreenIndex++;
            }
            else if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
                if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
                    currentSplashScreenIndex++;
            }
    
            //if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            //{
            //    introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
            //    if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
            //        currentSplashScreenIndex++;
            //}
            //else if (!(currentTextScreenIndex >= typingTextScreens.Count))
            //{
            //    typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
            //    if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
            //        currentTextScreenIndex++;
            //}
        }

        public override void Draw(GameTime gameTime)
        {

            if (currentTextScreenIndex < typingTextScreens.Count)
            {
                //call the appropriate textypingscreen (from the list); pass the uiBatch!)
                typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            }
            else if (currentSplashScreenIndex < introSplashScreens.Count)
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            }
            


            //if (currentSplashScreenIndex < introSplashScreens.Count)
            //{
            //    introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            //}
            //else if (currentTextScreenIndex < typingTextScreens.Count)
            //{
            //    //call the appropriate textypingscreen (from the list); pass the uiBatch!)
            //    typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            //}
            else
                requestedState = 1;
        }
    }

    class End : GameState
    {
        SpriteBatch _uiBatch;
        SpriteFont introFont;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        List<SplashScreen> introSplashScreens;
        List<TypingTextScreen> typingTextScreens;
        int currentTextScreenIndex;
        int currentSplashScreenIndex;

        public End()
        {
            requestedState = -1;
        }

        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;

            typingTextScreens = new List<TypingTextScreen>();
            introSplashScreens = new List<SplashScreen>();
            currentTextScreenIndex = 0;
            currentSplashScreenIndex = 0;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            // make me a font for the intro
            introFont = Content.Load<SpriteFont>("UI\\UIFont");


            // testing, read from file or something later?
            TypingTextScreen temp = new TypingTextScreen("The Love.     \nThe Pain           \nWhere do you draw the line between\nyour needs and the good of others?\n                  ", 70f, introFont);
            typingTextScreens.Add(temp);

            //load images for the intro
            SplashScreen tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\endsplash_lose2"), 3000, 300, 500);
            introSplashScreens.Add(tempSplash);
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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    requestedState = (int)GameStates.TITLE;
                }
            }

            // update the current texttypingscreen
            // if isDone after, select next screen
            // same with splashscreens
            if (!(currentTextScreenIndex >= typingTextScreens.Count))
            {
                typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
                if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
                    currentTextScreenIndex++;
            }
            else if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
                if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
                    currentSplashScreenIndex++;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            if (currentTextScreenIndex < typingTextScreens.Count)
            {
                //call the appropriate textypingscreen (from the list); pass the uiBatch!)
                typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            }
            else if (currentSplashScreenIndex < introSplashScreens.Count)
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            }
            else
                requestedState = 1;
        }
    }

    class EndWin : GameState
    {
        SpriteBatch _uiBatch;
        SpriteFont introFont;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        List<SplashScreen> introSplashScreens;
        List<TypingTextScreen> typingTextScreens;
        int currentTextScreenIndex;
        int currentSplashScreenIndex;

        public EndWin()
        {
            requestedState = -1;
        }

        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;

            typingTextScreens = new List<TypingTextScreen>();
            introSplashScreens = new List<SplashScreen>();
            currentTextScreenIndex = 0;
            currentSplashScreenIndex = 0;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            // make me a font for the intro
            introFont = Content.Load<SpriteFont>("UI\\UIFont");


            // testing, read from file or something later?
            TypingTextScreen temp = new TypingTextScreen("How few are those that,\n    despite their own needs,\nfind the path to be loved by another?\n          \nto live otherwise would be...     \n\nUNBEARABLE                ", 70f, introFont);
            typingTextScreens.Add(temp);

            //load images for the intro
            SplashScreen tempSplash = new SplashScreen(Content.Load<Texture2D>("SplashUI\\endsplash_win"), 3000, 300, 500);
            introSplashScreens.Add(tempSplash);

            temp = new TypingTextScreen("Credits\n--------\n", 100f, introFont);
            typingTextScreens.Add(temp);


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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    requestedState = (int)GameStates.CREDITS;
                }
            }

            // update the current texttypingscreen
            // if isDone after, select next screen
            // same with splashscreens
            if (!(currentTextScreenIndex >= typingTextScreens.Count))
            {
                typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
                if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
                    currentTextScreenIndex++;
            }
            else if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
                if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
                    currentSplashScreenIndex++;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            if (currentTextScreenIndex < typingTextScreens.Count)
            {
                //call the appropriate textypingscreen (from the list); pass the uiBatch!)
                typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            }
            else if (currentSplashScreenIndex < introSplashScreens.Count)
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            }
            else
                requestedState = 1;
        }
    }

    class Credits : GameState
    {
        SpriteBatch _uiBatch;
        SpriteFont introFont;
        const double GameStateDelay = 0.2;
        double entryGameTime = 0.0f;

        List<SplashScreen> introSplashScreens;
        List<TypingTextScreen> typingTextScreens;
        int currentTextScreenIndex;
        int currentSplashScreenIndex;

        public Credits()
        {
            requestedState = -1;
        }

        public override void Initialize(GameTime gameTime)
        {
            entryGameTime = gameTime.TotalGameTime.TotalSeconds;

            typingTextScreens = new List<TypingTextScreen>();
            introSplashScreens = new List<SplashScreen>();
            currentTextScreenIndex = 0;
            currentSplashScreenIndex = 0;
        }

        public override void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            _uiBatch = new SpriteBatch(graphics);

            // make me a font for the intro
            introFont = Content.Load<SpriteFont>("UI\\UIFont");


            // testing, read from file or something later?
            TypingTextScreen temp = new TypingTextScreen("CREDITS:\nAvery Alix\nFrank Krueger\nJohn Conbere\nTania Pavlisak\nDave Pavlisak\nJohn McLaughlin\n                 \nSPECIAL THANKS TO:\n------------------\n "+
                "Luke McKay\nAdam Smith-Kipnis\nJohanna Nutter\nCollin Moore\nCasey Egan\nMisty Nickle\nPeter Molyneux\nPeter Molydeux             \n                           ", 70f, introFont);
            typingTextScreens.Add(temp);
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
                if (gameTime.TotalGameTime.TotalSeconds - entryGameTime > GameStateDelay)
                {
                    requestedState = (int)GameStates.TITLE;
                }
            }

            // update the current texttypingscreen
            // if isDone after, select next screen
            // same with splashscreens
            if (!(currentSplashScreenIndex >= introSplashScreens.Count))
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Update(gameTime);
                if (introSplashScreens.ElementAt(currentSplashScreenIndex).IsDone)
                    currentSplashScreenIndex++;
            }
            else if (!(currentTextScreenIndex >= typingTextScreens.Count))
            {
                typingTextScreens.ElementAt(currentTextScreenIndex).Update(gameTime);
                if (typingTextScreens.ElementAt(currentTextScreenIndex).IsDone)
                    currentTextScreenIndex++;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            if (currentSplashScreenIndex < introSplashScreens.Count)
            {
                introSplashScreens.ElementAt(currentSplashScreenIndex).Render(gameTime, _uiBatch);
            }
            else if (currentTextScreenIndex < typingTextScreens.Count)
            {
                //call the appropriate textypingscreen (from the list); pass the uiBatch!)
                typingTextScreens.ElementAt(currentTextScreenIndex).Render(gameTime, _uiBatch);
            }
            else
                requestedState = 1;
        }
    }
}
