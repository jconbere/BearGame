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


        public Villager(World world)
            : base (world)
        {
            this.Health = Settings.Person_HealthDefault;
            this.Love = Settings.Person_Love;
            this.TricycleLove = Settings.Person_TricycleLove;
            this.HealthRegen = Settings.Person_HealthRegen;
            this.Speed = Settings.Person_Speed;
            
        }

        public override void Update(GameTime time)
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
