using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Achievement
    {
        public static RandomSound AchievementSound;

        public bool IsShowing { get; private set; }

        double _beginTime = 0;

        public virtual void Begin(GameTime time)
        {
            _beginTime = time.TotalGameTime.TotalSeconds;
        }

        public virtual void Update(GameTime time)
        {
            var now = time.TotalGameTime.TotalSeconds;

            if (!IsShowing && ((now - _beginTime) > 1))
            {
                IsShowing = true;
                AchievementSound.Play();
            }
        }
    }

    public class DaredevilAchievement : Achievement
    {
    }

    public class JerkAchievement : Achievement
    {
    }
}
