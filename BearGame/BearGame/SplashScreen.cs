using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    /// <summary>
    /// Displays text as though it is typed.
    /// Will play sound in the future (maybe)
    /// </summary>
    class SplashScreen
    {
        /// <summary>
        /// Time for which the screen is displayed in MS
        /// </summary>
        private float displayTime;

        /// <summary>
        /// time the screen needs to fadeIn
        /// </summary>
        private float fadeInTime;

        /// <summary>
        /// time the screen needs to fadeOut
        /// </summary>
        private float fadeOutTime;

        /// <summary>
        /// the image to show
        /// </summary>
        private Texture2D splashImage;

         /// <summary>
        /// time that has passed since the last character was displayed
        /// </summary>
        private float totalTime;

        /// <summary>
        /// is true if we're done (faded out).
        /// </summary>
        private bool isDone;

        /// <summary>
        /// Creates a splashscreen
        /// </summary>
        /// <param name="SplashImage">the image to display</param>
        /// <param name="DisplayTime">the time it is displayed</param>
        /// <param name="FadeOutTime">the time for fadeIn</param>
        /// <param name="FadeInTime">the time for fadeOut</param>
        public SplashScreen(Texture2D SplashImage, float DisplayTime, float FadeInTime, float FadeOutTime)
        {
            splashImage = SplashImage;
            displayTime = DisplayTime;
            fadeInTime = FadeInTime;
            fadeOutTime = FadeOutTime;
            totalTime = 0;
            isDone = false;
        }


        public SplashScreen()
        {
            splashImage = null;
            displayTime = 0;
            fadeInTime = 0;
            fadeOutTime = 0;
            totalTime = 0;
            isDone = false;
        }

        /// <summary>
        /// Updates the display and total Time
        /// </summary>
        /// <param name="gameTime">time since last frame</param>
        public void Update(GameTime gameTime)
        {
            if (isDone)
                return;

            //add time since last frame to totalTime
            totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //if no, return;
            return;
        }

        /// <summary>
        /// renders the currently displayed characters
        /// Needs a spritebatch from somewhere
        /// </summary>
        /// <param name="gameTime">time that has passed</param>
        /// <param name="spriteBatch"> Sprite Batch for drawing text on screen</param>
        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (totalTime > fadeInTime+displayTime+fadeOutTime)
            {
                isDone = true;
            }
                            // determine alpha
            float alpha = 0.5f;
            if(totalTime < fadeInTime)
                alpha = MathHelper.SmoothStep(0.0f, 1.0f, totalTime/fadeInTime);
            else if(totalTime > fadeInTime + displayTime)
                alpha = MathHelper.SmoothStep(1.0f, 0.0f, (totalTime - (fadeInTime + fadeOutTime)) / fadeOutTime);

            // print text onscreen until current index.
            spriteBatch.Begin();
            spriteBatch.Draw(splashImage, new Rectangle(0, 0, 800, 600), Color.White * alpha);
            spriteBatch.End();

            return;
        }

        public bool IsDone
        {
            get { return isDone; }
        }

    }
}
