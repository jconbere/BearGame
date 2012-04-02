using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BearGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        SoundEffect backgroundMusic;
        SoundEffectInstance backgroundMusicInstance;

        List<GameState> gameStates = new List<GameState>();
        GameState current_GameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;            

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameStates.Add(new Intro());
            gameStates.Add(new Title());
            gameStates.Add(new MainGame());
            gameStates.Add(new End());
            gameStates.Add(new EndWin());
            gameStates.Add(new EndPacifism());
            gameStates.Add(new EndKiller());
            gameStates.Add(new Credits());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (current_GameState == null)
            {
                switchGameStates(0, gameTime);
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            current_GameState.Update(gameTime);

            if (current_GameState.requestedState >= 0)
            {
                switchGameStates(current_GameState.requestedState, gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            current_GameState.Draw(gameTime);
           
            base.Draw(gameTime);
        }

        protected void switchGameStates(int index, GameTime gameTime)
        {
            if (index == 1)
            {
                if (backgroundMusicInstance == null)
                {
                    backgroundMusic = Content.Load<SoundEffect>("Audio\\BackgroundMusic");
                    backgroundMusicInstance = backgroundMusic.CreateInstance();
                    backgroundMusicInstance.IsLooped = true;
                    backgroundMusicInstance.Volume = 0.125f;
                    backgroundMusicInstance.Play();
                }
            }

            //Unload Stuff from current state
            if (current_GameState != null)
            {
                current_GameState.UnloadContent(Content);
            }

            //Start Up new State
            current_GameState = gameStates.ElementAt<GameState>(index);
            current_GameState.Initialize(gameTime);
            current_GameState.LoadContent(GraphicsDevice, Content);


        }
    }
}
