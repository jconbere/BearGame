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

        Camera camera;
        World world;
        GameView view;

        SoundEffect backgroundMusic;
        SoundEffectInstance backgroundMusicInstance;

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
            var settings = new GameSetting();

            camera = new Camera();
            world = new World(settings);
            view = new GameView(world);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            backgroundMusic = Content.Load<SoundEffect>("Audio\\BackgroundMusic");
            backgroundMusicInstance = backgroundMusic.CreateInstance();
            backgroundMusicInstance.IsLooped = true;
            backgroundMusicInstance.Volume = 0.5f;
            backgroundMusicInstance.Play();

            Villager.HurtSound = new RandomSound(Content, "Audio\\person_hurt_01", "Audio\\person_hurt_02", "Audio\\person_hurt_03", "Audio\\person_hurt_04", "Audio\\person_hurt_05");
            Villager.HurtBadSound = new RandomSound(Content, "Audio\\person_hurt_bad_03", "Audio\\person_hurt_bad_04", "Audio\\person_hurt_bad_05", "Audio\\person_hurt_bad_06");
            TakeHoney.PickUpHoneySound = new RandomSound(Content, "Audio\\bear_pick_up_honey_02");
            EatHoney.EatHoneySound = new RandomSound(Content, "Audio\\bear_eat_honey_02");
            GiveHoney.PersonThanksSound = new RandomSound(Content, "Audio\\person_thanks_01", "Audio\\person_thanks_02");
            RideTricycle.GetOnSound = new RandomSound(Content, "Audio\\bear_get_on_off_tricycle_03");
            GetOffTricycle.GetOffSound = new RandomSound(Content, "Audio\\bear_get_on_off_tricycle_03");
            Bear.PersonAwwSound = new RandomSound(Content, "Audio\\person_aww_03", "Audio\\person_aww_04");

            view.LoadContent(GraphicsDevice, Content);

            var worldTiles = Content.Load<Texture2D>("Sprites\\WorldTiles");
            world.LoadContent(GraphicsDevice, Content, 1);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            world.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            view.Draw();

            base.Draw(gameTime);
        }
    }
}
