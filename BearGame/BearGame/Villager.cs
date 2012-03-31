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
        const int ActivityThreshold = 6;  // how far from the bear do we update
        const int RespawnThreshold = 9;  //how far fro the bear do we put them back in their starting place
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
            base.Update(time);

            if (!IsVisible) 
            {
                return;
            }

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
                            (Math.Abs(World.Bear.c_position.Row - this.spawn_position.Row) > 6 ||
                            Math.Abs(World.Bear.c_position.Col - this.spawn_position.Col) > 6)) // assuming 6 visual radius
                {
                    // force respawn
                    if (!IsDead) this.c_position = spawn_position; // Leave bodies alone!
                }


                else if (Distance(World.Bear, this) <= ActivityThreshold)
                {
                    //do on screen stuff
                    var now = time.TotalGameTime.TotalSeconds;
                    if ((now - LastMoveTime) > Settings.People_MoveInterval)
                    {
                        Act(time);
                    }
                }
            }
            // base update
        }

        private void Act(GameTime time)
        {

            int DeltaRow = 0;
            int DeltaCol = 0;

            DeltaRow = this.c_position.Row - World.Bear.c_position.Row;
            DeltaCol = this.c_position.Col - World.Bear.c_position.Col;

            Random rnd = new Random();
            
            // simple stupid state machine.  run in the farthest direction

            double[] ActionWeight = new double[5];

            
            ActionWeight[0] = EvalWeight(this.c_position.Row, this.c_position.Col, World.Bear, 0, 0, Love);
            ActionWeight[1] = EvalWeight(this.c_position.Row, this.c_position.Col, World.Bear, 1, 0, Love);
            ActionWeight[2] = EvalWeight(this.c_position.Row, this.c_position.Col, World.Bear, -1, 0, Love);
            ActionWeight[3] = EvalWeight(this.c_position.Row, this.c_position.Col, World.Bear, 0, 1, Love);
            ActionWeight[4] = EvalWeight(this.c_position.Row, this.c_position.Col, World.Bear, 0, -1, Love);

            if (Love<=3)
            {
                //double val =0;
                //int minindex = MaxDouble;
                //for (int I = 0; I < 5; I++) 
                //{
                //    if (ActionWeight[I] < val)
                //    {
                //        minindex = I;
                //    }
                //}
            
            }


            
            //if (Math.Abs(DeltaCol) >= Math.Abs(DeltaRow))
            //{
            //    if (DeltaCol <= 0)
            //    {
            //        if (World.IsPassable(this.c_position.Col + 1, this.c_position.Row))
            //        {
            //            FacingDirection = Direction.Right;
            //            MoveCell(time, new CellPosition(1, 0));
            //            UpdateSpriteIndex();
            //        }
            //    }
            //}
            //else
            //{
            //    if (World.IsPassable(this.c_position.Col - 1, this.c_position.Row))
            //    {
            //        FacingDirection = Direction.Left;
            //        MoveCell(time, new CellPosition(-1, 0));
            //        UpdateSpriteIndex();
            //    }
            //    else
            //    {
            //        if (DeltaRow <= 0)
            //        {
            //            if (World.IsPassable(this.c_position.Col, this.c_position.Row + 1))
            //            {
            //                FacingDirection = Direction.Down;
            //                MoveCell(time, new CellPosition(0, 1));
            //                UpdateSpriteIndex();
            //            }
            //        }
            //        else
            //        {
            //            if (World.IsPassable(this.c_position.Col, this.c_position.Row - 1))
            //            {
            //                FacingDirection = Direction.Up;
            //                MoveCell(time, new CellPosition(0, -1));
            //                UpdateSpriteIndex();
            //            }
            //        }
            //    }
            //}
        }

        private double EvalWeight(int Row, int Col, Bear bear, int DeltaRow, int DeltaCol, int Love)
        {
            double weight = 0;

            int BearDeltaRow = Row - bear.c_position.Row;
            int BearDeltaCol = Col - bear.c_position.Col;

            if (World.IsPassable(Row + DeltaRow, Col + DeltaCol))
            {
                weight = ((Math.Pow(BearDeltaRow,2)+ Math.Pow(BearDeltaCol,2)));
                return weight;
            }
            else
            {
                return 0;
            }
        }
        protected override void UpdateSpriteIndex()
        {
            spriteIndex = 16 * (int)FacingDirection;
        }
    }
}
