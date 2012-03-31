using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    class Villager : Actor
    {
        public int Health;
        public int Love;
        public int HoneyTaken;
        public int HoneyMax;
        public int TricycleLove;
        public int HealthRegen;
        public int Speed;
        const int ActivityThreshold = 7;  // how far from the bear do we update
        const int RespawnThreshold = 10;  //how far fro the bear do we put them back in their starting place
        // ONLY respawn them if both the villager and the respawn location are offscreen


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
            this.Health = settings.Person_HealthDefault;
            this.Love = settings.Person_Love;
            this.TricycleLove = settings.Person_TricycleLove;
            this.HealthRegen = settings.Person_HealthRegen;
            this.Speed = settings.Person_Speed;
            
        }

        public void Update(World Myworld)
        {
            
            //what to do?
            
            //love =0 flee
            //love =3 irritated
            //love =4 neutral
            //love =7 unconditional love


            // if no bear closeby.. mill aimlessly

            //if (this.Distance(Myworld.Bear) <= ActivityThreshold)
            //{ 
            //    //do on screen stuff
            //    switch (this.Love)
            //    {
            //        case 0:


            //            break;

            //        case 1:
            //            break;

            //        default:
            //            break;
            //    }
        
            //}
            //else if ((this.Distance(bear,this) >= RespawnThreshold) // && ( // calc bear distance))
            //{ 
            //    // force respawn
            //    this.c_position = spawn_position; 
            //}
        }
    }
}
