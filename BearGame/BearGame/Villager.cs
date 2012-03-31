﻿using System;
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
        public bool IsDead = false;


        public Villager(GameSetting settings)
            : base (settings)
        {
            this.Health = settings.Person_HealthDefault;
            this.Love = settings.Person_Love;
            this.TricycleLove = settings.Person_TricycleLove;
            this.HealthRegen = settings.Person_HealthRegen;
            this.Speed = settings.Person_Speed;
            
        }

        public override void Update(GameTime time, World Myworld)
        {
            
            //what to do?
            
            //love =0 flee
            //love =3 irritated
            //love =4 neutral
            //love =7 unconditional love

            //movement 0 once a turn, 1 75% 2 50% 3 20%  away

            // if no bear closeby.. mill aimlessly

            if (Distance(Myworld.Bear,this) <= ActivityThreshold)
            { 
                //do on screen stuff
                int DeltaRow = 0;
                int DeltaCol = 0;

                DeltaRow = this.c_position.Row -Myworld.Bear.c_position.Row ;
                DeltaCol = this.c_position.Col - Myworld.Bear.c_position.Col;
                
                // simple stupid state machine.  run in the farthest direction

                if (Math.Abs(DeltaRow) > Math.Abs(DeltaCol))
                {
                    if (DeltaRow > 0)
                    { 
                            //MoveCell
                    }
                    else 
                    {
                    
                    }
                }
                else
                {
                    if (DeltaCol > 0)
                    {
                    }
                    else
                    {
                    }
                }
                    //Myworld.IsPassable(this.c_position.Row)

                FacingDirection = Direction.Down;

        
            }
            else if ((Distance(Myworld.Bear,this) >= RespawnThreshold) && 
                Math.Abs(Myworld.Bear.c_position.Row - this.spawn_position.Row)>6 &&
                Math.Abs(Myworld.Bear.c_position.Col - this.spawn_position.Col)>6) // assuming 6 visual radius
                
            { 
                // force respawn
                if (!IsDead) this.c_position = spawn_position; // Leave bodies alone!
            }
        }
    }
}
