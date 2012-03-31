using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    class Bear : Actor
    {
        public int Health;
        public bool HasRibbon = false;
        public bool HasTricycle = false;
        public bool HasHoney = false;

        public Bear(GameSetting settings)
            : base(settings)
        {
            this.Health = settings.Bear_HealthDefault;
        }

        public override void Update(GameTime time)
        {
        }
    }
}
