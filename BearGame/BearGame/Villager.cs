using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    class Villager
    {
        public int Health;
        public int Love;
        public int HoneyTaken;
        public int HoneyMax;
        public int TricycleLove;
        public int HealthRegen;
        public int Speed;


        Villager()
        {
            this.Health = 3;
            this.Love = 1;
            this.TricycleLove = 1;
            this.HealthRegen = 0;
            this.Speed = 1;
        }

        Villager(GameSetting settings)
        {
            this.Health = settings.Bear_HealthDefault;
        }

    }
}
