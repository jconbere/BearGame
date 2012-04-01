using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace BearGame
{
    public class Honey : Prop
    {
        SoundEffect _currentAmbientEffect;
        SoundEffectInstance _currentAmbientEffectInstance;
        SoundEffect _beehiveBuzz;

        public Honey(World world)
            : base(world)
        {
            spriteIndex = 0;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Graphics.Texture2D texture_IN, CellPosition cellPosition, Microsoft.Xna.Framework.Vector2 pixelPosition)
        {
            //_beehiveBuzz = content.Load<SoundEffect>("Audio\\beehive_buzz_01");
            base.LoadContent(texture_IN, cellPosition, pixelPosition);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
        }

        protected override void UpdateSpriteIndex()
        {
            spriteIndex = 0;
        }


    }
}
