using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Villager : Actor
    {
        int _health;
        public int Health
        {
            get { return _health; }
            set
            {
                var v = Math.Min(Settings.Person_HealthMax, Math.Max(Settings.Person_HealthMin, value));
                if (v != _health)
                {
                    _health = v;

                    if (_health == Settings.Person_HealthDefault)
                    {
                    }
                    else if (_health == Settings.Person_HealthMin)
                    {
                        HurtBadSound.Play();
                    }
                    else
                    {
                        HurtSound.Play();
                    }
                }
            }
        }

        int _love;
        public int Love
        {
            get { return _love; }
            set
            {
                _love = Math.Min(Settings.Person_LoveMax, Math.Max(Settings.Person_LoveMin, value));
            }
        }

        public int HoneyTaken;
        public int HoneyMax;
        public int TricycleLove;
        public int HealthRegen;
        public int Speed;
        const int ActivityThreshold = 7;  // how far from the bear do we update
        const int RespawnThreshold = 10;  //how far fro the bear do we put them back in their starting place

        // ONLY respawn them if both the villager and the respawn location are offscreen
        public bool IsDead { get { return Health <= Settings.Person_HealthMin; } }

        public GameSetting.VillagerNames Name;

        public static RandomSound HurtSound;
        public static RandomSound HurtBadSound;
        
        public Villager(World world)
            : base(world)
        {
            this.Health = Settings.Person_HealthDefault;
            this.Love = Settings.Person_InitialLove;
            this.TricycleLove = Settings.Person_TricycleLove;
            this.HealthRegen = Settings.Person_HealthRegen;
            this.Speed = Settings.Person_Speed;
            this.Name = (GameSetting.VillagerNames)world.AllVillagers.Count;

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
            UpdateSpriteIndex();
        }

        private void Act(GameTime time)
        {

            int DeltaRow = 0;
            int DeltaCol = 0;
            int winningindex = 0;
            
            DeltaRow = this.c_position.Row - World.Bear.c_position.Row;
            DeltaCol = this.c_position.Col - World.Bear.c_position.Col;
            
            // simple stupid state machine.  run in the farthest direction

            double[] ActionWeight = new double[5];

            ActionWeight[0] = EvalWeight(this.c_position.Col, this.c_position.Row, World.Bear, 1, 0);  //down
            ActionWeight[1] = EvalWeight(this.c_position.Col, this.c_position.Row, World.Bear, -1, 0); // up
            ActionWeight[2] = EvalWeight(this.c_position.Col, this.c_position.Row, World.Bear, 0, 1); // right
            ActionWeight[3] = EvalWeight(this.c_position.Col, this.c_position.Row, World.Bear, 0, -1); // left
            ActionWeight[4] = EvalWeight(this.c_position.Col, this.c_position.Row, World.Bear, 0, 0); // stay put
            
            if (Love <= 1)
            {
                double tempval = Double.MaxValue;
                for (int i = 0; i < 5; i++)
                {
                    if (ActionWeight[i] < tempval && ActionWeight[i] != -1)
                    {
                        winningindex = i;
                        tempval = ActionWeight[i];
                    }
                }
            }
            else 
            {
                double tempval=0;                
                for (int i = 0; i < 5; i++)
                {
                    if (ActionWeight[i] > tempval)
                    {
                        winningindex = i;
                        tempval = ActionWeight[i];
                    }
                }
            }

            switch (winningindex)
            { 
                case 0:
                    FacingDirection = Direction.Up;
                    MoveCell(time, new CellPosition(0, -1));
                    UpdateSpriteIndex();
                break;
                case 1:
                    FacingDirection = Direction.Down;
                    MoveCell(time, new CellPosition(0, 1));
                    UpdateSpriteIndex();
                    break;
                case 2:
                    FacingDirection = Direction.Right;
                    MoveCell(time, new CellPosition(0, -1));
                    UpdateSpriteIndex();
                    break;
                case 3:
                    FacingDirection = Direction.Left;
                    MoveCell(time, new CellPosition(0, 1));
                    UpdateSpriteIndex();
                    break;
                default:
                    UpdateSpriteIndex();
                    break;

            }
        }

        private double EvalWeight(int Col, int Row, Bear bear, int DeltaCol, int DeltaRow)
        {
            double weight = 0;

            int BearDeltaRow = Row - bear.c_position.Row + DeltaRow;
            int BearDeltaCol = Col - bear.c_position.Col + DeltaCol;

            if (World.IsPassable(Row + DeltaRow, Col + DeltaCol))
            {
                weight = ((Math.Pow(BearDeltaRow,2) + Math.Pow(BearDeltaCol,2)));
                return weight;
            }
            else
            {
                return -1;
            }
        }
        protected override void UpdateSpriteIndex()
        {
            if (!IsDead)
            {
                spriteIndex = 16 * (int)FacingDirection;
            }
            else
            {
                spriteIndex = 96;
            }
        }
    }
}
