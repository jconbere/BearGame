using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    public abstract class Achievement
    {
        public static RandomSound AchievementSound;

        bool _finished;

        public bool IsVisible { get; private set; }

        double _beginTime = 0;
        double _showTime = 0;

        public virtual void Begin(GameTime time)
        {
            _beginTime = time.TotalGameTime.TotalSeconds;
        }

        public virtual void Update(GameTime time)
        {
            if (_finished) return;

            var now = time.TotalGameTime.TotalSeconds;

            if (IsVisible)
            {
                if ((now - _showTime) > 3)
                {
                    IsVisible = false;
                    _finished = true;
                }
            }
            else
            {
                if ((now - _beginTime) > 1)
                {
                    _showTime = now;
                    IsVisible = true;
					if (AchievementSound != null) {
	                    AchievementSound.Play();
					}
                }
            }
        }

        public virtual void Draw(SpriteBatch uiBatch)
        {
            if (IsVisible)
            {
                var tex = GetTexture();
                var w = 300;
                uiBatch.Draw(GetTexture(), new Rectangle(200+(400-w)/2, 400, w, (int)(tex.Height/(float)tex.Width*w)), Color.White);
            }
        }

        protected abstract Texture2D GetTexture();
    }

    public class DaredevilAchievement : Achievement
    {
        public static Texture2D DaredevilTexture;

        protected override Texture2D GetTexture()
        {
            return DaredevilTexture;
        }
    }

    public class JerkAchievement : Achievement
    {
        public static Texture2D JerkTexture;

        protected override Texture2D GetTexture()
        {
            return JerkTexture;
        }
    }
}
