using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Achievement
    {
        public virtual void Begin(GameTime time)
        {
        }
    }

    public class DaredevilAchievement : Achievement
    {
    }

    public class JerkAchievement : Achievement
    {
    }
}
