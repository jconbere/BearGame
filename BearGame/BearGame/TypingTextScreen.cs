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
    class TypingTextScreen
    {
        /// <summary>
        /// The text to display
        /// </summary>
        private String text;

        /// <summary>
        /// current text index
        /// </summary>
        private int textIndex;

        /// <summary>
        /// the font to be used
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// the intervall between new letters in ms (maybe make this a rando range or something later)
        /// </summary>
        private float intervall;

        /// <summary>
        /// time that has passed since the last character was displayed
        /// </summary>
        private float totalTime;

        /// <summary>
        /// is true if all text is displayed.
        /// </summary>
        private bool isDone;

        /// <summary>
        /// Create a TypeTextScreen object, displays each character after intervall passes, until whole text is displayed.
        /// </summary>
        /// <param name="Text">The text to be displayed</param>
        /// <param name="Intervall">the intervall in between new characters appearing</param>
        public TypingTextScreen(String Text, float Intervall, SpriteFont Font)
        {
            text = Text;
            intervall = Intervall;
            totalTime = 0;
            textIndex = 0;
            font = Font;
            isDone = false;

        }


        public TypingTextScreen(SpriteFont Font)
        {
            text = "";
            intervall = 0.0f;
            totalTime = 0;
            textIndex = 0;
            font = Font;
            isDone = false;

        }

        /// <summary>
        /// Updates the text display
        /// If intervall has passed displays the next character.
        /// </summary>
        /// <param name="gameTime">time since last frame</param>
        public void Update(GameTime gameTime)
        {
            if (isDone)
                return;

            //add time since last frame to totalTime
            totalTime += gameTime.ElapsedGameTime.Milliseconds;

            //check if intervall is reached
            if (totalTime >= intervall)
            {
                //if yes, reset totalTime and set to display next character (update string index)
                totalTime = 0;
                textIndex += 1;

                // later: play a typing sound

                if (textIndex >= text.Length)
                {
                    // if last char, set isDone true. 
                    textIndex = text.Length - 1;
                    isDone = true;

                    //(later: play Enter/Return key sound)
                }
            }
            

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
            // print text onscreen until current index.
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text.Substring(0, textIndex), new Vector2(5, spriteBatch.GraphicsDevice.Viewport.Height / 2), Color.Green);
            spriteBatch.End();
        }

        public bool IsDone
        {
            get { return isDone; }
        }
    }
}
