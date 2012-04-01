using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    public class Villager : Actor
    {
        static List<CellPosition> Pathfind_Nodes;
        int _health;
        public bool isHugging = false;
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

        private int prevLove;

        //Emote Display Times
        public const float prevEmoteTime = 20.0f;
        public const float nextEmoteTime = 40.0f;
        public double loveIncreaseTime = 0.0;
        public float emoteDisplayTime = 0.0f; 

        public int HoneyTaken;
        public int HoneyMax;
        public int TricycleLove;
        public int HealthRegen;
        public float Speed = 1.0f;
        int ActivityThreshold;  // how far from the bear do we run away/follow
        public float SpeedStep;
        public int ApproachDistance;
        //const int RespawnThreshold = 10;  //how far fro the bear do we put them back in their starting place

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
            this.prevLove = this.Love;
            this.TricycleLove = Settings.Person_TricycleLove;
            this.HealthRegen = Settings.Person_HealthRegen;
            this.Speed = Settings.Person_Speed;
            this.SpeedStep = Settings.Person_SpeedStep;
            this.ActivityThreshold = Settings.Person_ActivityThreshold;
            this.ApproachDistance = Settings.Person_BaseApproachDistance;
            this.Name = (GameSetting.VillagerNames)world.AllVillagers.Count;
            this.IsActive = true;

            CellPosition[] nodes = {new CellPosition(0,1),
                              new CellPosition(1,1),
                              new CellPosition(1,0),
                              new CellPosition(1,-1),
                              new CellPosition(0,-1),
                              new CellPosition(-1,-1),
                              new CellPosition(-1,0),
                              new CellPosition(-1,1)                         
                              };

            Pathfind_Nodes = new List<CellPosition>(nodes);

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
                /*
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
                }*/

                Vector2 bearDirection = targetDirecton(this, World.Bear);

                ApproachDistance = Settings.Person_BaseApproachDistance - (Love - Settings.Person_InitialLove);

                Speed = Speed - (Math.Abs(Settings.Person_InitialLove - Love) * SpeedStep);
                if (Math.Abs(bearDirection.X) > ActivityThreshold)
                {
                    bearDirection.X = 0;
                }
                if (Math.Abs(bearDirection.Y) > ActivityThreshold)
                {
                    bearDirection.Y = 0;
                }
               
                if (Love > Settings.Person_InitialLove)
                {
                    if (Math.Abs(bearDirection.X) < ApproachDistance)
                    {
                        bearDirection.X = 0;
                    }
                    if (Math.Abs(bearDirection.Y) < ApproachDistance)
                    {
                        bearDirection.Y = 0;
                    }
                    bearDirection.X = bearDirection.X * -1;
                    bearDirection.Y = bearDirection.Y * -1;
                }

                if ((time.TotalGameTime.TotalSeconds - LastMoveTime) > (Speed))
                {
                    //Check for Hug
                    if (Love == Settings.Person_LoveMax)
                    {
                        if (bearDirection.X == 0 && bearDirection.Y == 0)
                        {
                            //on bear initiate hugging
                            isHugging = true;
                        }

                    }

                    //Make move 1 square
                    if (bearDirection.X != 0)
                    {
                        bearDirection.X = bearDirection.X / Math.Abs(bearDirection.X);
                    }
                    if (bearDirection.Y != 0)
                    {
                        bearDirection.Y = bearDirection.Y / Math.Abs(bearDirection.Y);
                    }

                    CellPosition newPos = new CellPosition((int)bearDirection.X, (int)bearDirection.Y);
                    if (!(newPos.Row == 0 && newPos.Col == 0))
                    {
                        if (!World.IsPassable(this.c_position + newPos))
                        {
                            int indexOf = Pathfind_Nodes.FindIndex(i => i.Row == newPos.Row && i.Col == newPos.Col);
                            if (indexOf >= 0)
                            {
                                int currentIndex = indexOf;
                                for (int i = 1; i <= 4; i++)
                                {
                                    newPos = Pathfind_Nodes[(indexOf + i) % 7];
                                    if (World.IsPassable(this.c_position + newPos))
                                    {
                                        break;
                                    }

                                    newPos = Pathfind_Nodes[(indexOf - i) % 7];
                                    if (World.IsPassable(this.c_position + newPos))
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // TODO: John, handle this case, ask Avery
                            }
                        }

                        MoveCell(time, newPos);

                        if (newPos.Row > 0)
                        {
                            FacingDirection = Direction.Right;
                        }
                        else
                        {
                            FacingDirection = Direction.Left;
                        }
                    }
                }

            }
            // base update
            UpdateSpriteIndex();

            //Update Emotion Timer
            /*public const float prevEmoteTime = 0.5f;
            public const float nextEmoteTime = 2.5f;
            public double loveIncreaseTime = 0.0;
            public float emoteDisplayTime = 0.0f; */
            if (emoteDisplayTime > 0)
            {
                emoteDisplayTime -= (float)(time.TotalGameTime.TotalSeconds - loveIncreaseTime);
            }
           


        }

        private void Act(GameTime time)
        {
            /*
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

            }*/
        }

        /*
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
        }*/

        public void ChangeLove(int loveValue, GameTime time)
        {
            prevLove = Love;
            Love += loveValue;
            loveIncreaseTime = time.TotalGameTime.TotalSeconds;
            emoteDisplayTime = prevEmoteTime + nextEmoteTime;
            return;

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

        public override void Draw(SpriteBatch spriteBatch_IN)
        {
            if (!IsVisible) return;

            Rectangle sourceRec;

            var row = spriteIndex / NumColumnsInSpriteTexture;
            var col = spriteIndex - row * NumColumnsInSpriteTexture;

            sourceRec = new Rectangle(col * World.TileSize, row * World.TileSize, World.TileSize, World.TileSize);
            spriteBatch_IN.Draw(SpriteTexture, position, sourceRec, Color.White);

            //Draw Emotes
            if (emoteDisplayTime > 0)
            {
                Vector2 drawPos = this.position;
                drawPos.Y -= 50;
                if ((emoteDisplayTime) > nextEmoteTime)
                {
                    spriteBatch_IN.Draw(emotes, drawPos, new Rectangle(this.prevLove * 64, 0, 64, 64), Color.White);
                }
                else
                {
                    spriteBatch_IN.Draw(emotes, drawPos, new Rectangle(this.Love * 64, 0, 64, 64), Color.White);

                }
                
            }
            
        }
    }
}
