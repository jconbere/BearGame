using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Villager : Actor
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


        public Villager(World world)
            : base(world)
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

            //movement 0 once a turn, 1 75% 2 50% 3 20%  away
            //  0   1   2   3   4   5   6   7    Love
            // 100 75  50  30  30   50  75 100   Activity %
           
            if (!IsDead)
            {
                if ((Distance(World.Bear, this) >= RespawnThreshold) &&
                            Math.Abs(World.Bear.c_position.Row - this.spawn_position.Row) > 6 &&
                            Math.Abs(World.Bear.c_position.Col - this.spawn_position.Col) > 6) // assuming 6 visual radius
                {
                    // force respawn
                    if (IsDead) this.c_position = spawn_position; // Leave bodies alone!
                }

                else if (Distance(World.Bear, this) <= ActivityThreshold)
                {
                    //do on screen stuff
                    int DeltaRow = 0;
                    int DeltaCol = 0;

                    DeltaRow = this.c_position.Row - World.Bear.c_position.Row;
                    DeltaCol = this.c_position.Col - World.Bear.c_position.Col;

                    switch (Love)
                    {
                        case 0:

                        default:
                            // simple stupid state machine.  run in the farthest direction

                            if (Math.Abs(DeltaCol) >= Math.Abs(DeltaRow))
                            {
                                if (DeltaCol >= 0)
                                {
                                    if (World.IsPassable(this.c_position.Col+1, this.c_position.Row))
                                    {
                                        FacingDirection = Direction.Right;
                                        UpdateSpriteIndex();
                                    }
                                }
                                else
                                {
                                    if (World.IsPassable(this.c_position.Col-1, this.c_position.Row))
                                    {
                                        FacingDirection = Direction.Left;
                                        UpdateSpriteIndex();
                                    }
                                }
                            }
                            else
                            {
                                if (DeltaRow >= 0)
                                {
                                    if (World.IsPassable(this.c_position.Col, this.c_position.Row + 1))
                                    {
                                        FacingDirection = Direction.Down;
                                        UpdateSpriteIndex();
                                    }
                                }
                                else
                                {
                                    if (World.IsPassable(this.c_position.Col, this.c_position.Row - 1))
                                    {
                                        FacingDirection = Direction.Up;
                                        UpdateSpriteIndex();
                                    }
                                }
                            }
                         break;
                    }
                }
            }
        }

        protected override void UpdateSpriteIndex()
        {
            spriteIndex = 16 * (int)FacingDirection;
        }
    }
}
