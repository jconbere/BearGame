using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Honey : Prop
    {
        SoundEffectInstance _buzzInstance;
        public static SoundEffect _beehiveBuzz;

        public Honey(World world)
            : base(world)
        {
            spriteIndex = 0;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Graphics.Texture2D texture_IN, CellPosition cellPosition, Microsoft.Xna.Framework.Vector2 pixelPosition)
        {
            _buzzInstance = _beehiveBuzz.CreateInstance();
            _buzzInstance.IsLooped = true;
            _buzzInstance.Volume = 0.125f;
            _buzzInstance.Play();

            base.LoadContent(texture_IN, cellPosition, pixelPosition);
        }

        public override void UnloadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.UnloadContent(content);
            if (_buzzInstance != null)
            {
                _buzzInstance.Stop();
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (this.IsVisible)
            {

                int bearDistance = Distance(this, World.Bear);
                switch (bearDistance)
                {
                    case 0:
                        _buzzInstance.Volume = 1.0f;
                        break;
                    case 1:
                        _buzzInstance.Volume = 0.8f;
                        break;
                    case 2:
                        _buzzInstance.Volume = 0.5f;
                        break;
                    case 3:
                        _buzzInstance.Volume = 0.3f;
                        break;
                    default:
                        _buzzInstance.Stop();
                        break;
                }
                if (bearDistance < 4)
                {

                    _buzzInstance.Resume();
                }
                else
                {
                    _buzzInstance.Stop();
                }
            }
            else
            {
                _buzzInstance.Stop();
            }

            base.Update(time);
        }

        protected override void UpdateSpriteIndex()
        {
            spriteIndex = 0;
        }


    }
}
