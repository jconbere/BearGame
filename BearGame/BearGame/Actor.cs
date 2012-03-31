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
        private int[] spriteSize = new int[2];
        private int animationDelay = 0;

        public Actor()
        {

        }

        public void initialize(Texture2D texture_IN, Vector2 position_IN, int index_IN, int width, int height)
        {
            SpriteTexture = texture_IN;
            position = position_IN;
            spriteIndex = index_IN;
            spriteSize[0] = width;
            spriteSize[1] = height;
        }

        public void update()
        {
            animationDelay = animationDelay % 100;

            if (animationDelay == 0)
            {
                spriteIndex++;
                spriteIndex = spriteIndex % 3;
            }

            animationDelay++;
        }

        public void draw(SpriteBatch spriteBatch_IN)
        {
            Rectangle sourceRec;
            sourceRec = new Rectangle(spriteIndex * spriteSize[0], 0, spriteSize[0], spriteSize[1]);
            spriteBatch_IN.Draw(SpriteTexture, position, sourceRec, Color.White);


        }


    }
}
