using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    class Actor
    {

        int spriteIndex = 0;
        private Texture2D SpriteTexture;
        private Vector2 position;

        public Actor()
        {

        }

        public void initialize(Texture2D texture_IN, Vector2 position_IN, int index_IN)
        {
            SpriteTexture = texture_IN;
            position = position_IN;
            spriteIndex = index_IN;

        }

        public void update()
        {


        }

        public void draw(SpriteBatch spriteBatch_IN)
        {
            spriteBatch_IN.Draw(SpriteTexture, position, Color.White);


        }


    }
}
