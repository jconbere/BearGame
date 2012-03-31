using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearGame
{
    class Bear
    {
        public int Health;
        
        Bear()
        {
            this.Health = 255;
        }

        Bear(GameSetting settings)
        {
            this.Health = settings.Bear_HealthDefault;
        }

        public override void Update()
        {
        
        }
    }
}
