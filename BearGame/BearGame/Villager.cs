﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BearGame
{
    public class Villager : Actor
    {
        Random _rand;
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

                    var volume = 0.125f;
                    var sound = HurtSound;

                    if (_health == Settings.Person_HealthDefault - 2)
                    {
                        HurtBadSound.Play();
                        volume = 0.25f;
                    }
                    else if (_health == Settings.Person_HealthDefault - 3)
                    {
                        HurtBadSound.Play();
                        volume = 0.5f;
                    }

                    if (_health != Settings.Person_HealthDefault)
                    {
                        sound.Play(volume);

                        var crunchVol = volume + 0.5f;
                        if (crunchVol > 1)
                        {
                            crunchVol = 1;
                        }
                        DeadSound.Play(crunchVol);
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

        public int LoveFromTricycle { get; set; }
        public int LoveFromHoney { get; set; }

        private int prevLove;

        //Emote Display Times
        public const float prevEmoteTime = 20.0f;
        public const float nextEmoteTime = 40.0f;
        public double loveIncreaseTime = 0.0;
        public float emoteDisplayTime = 0.0f; 

        public int HoneyTaken;
        public int HoneyMax;
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
        public static RandomSound DeadSound;

        public Villager(World world)
            : base(world)
        {
            this.Health = Settings.Person_HealthDefault;
            this.Love = Settings.Person_InitialLove;
            this.prevLove = this.Love;
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
            _rand = new Random();

        }

        protected override double TransitionInterval
        {
            get
            {
                return 1.0 / 2;
            }
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
                Vector2 bearDirection = targetDirecton(this, World.Bear);

                ApproachDistance = Settings.Person_BaseApproachDistance - (Love - Settings.Person_InitialLove);
                if (_love < Settings.Person_LoveMax && ApproachDistance < 2)
                {
                    ApproachDistance = 2;
                }

                Speed = Settings.Person_Speed - (Math.Abs(Settings.Person_InitialLove - Love) * SpeedStep);
                if (Speed < 0.1f)
                {
                    Speed = 0.1f;
                }
                else if (Speed > 10.0f)
                {
                    Speed = 10.0f;
                }

                if (Math.Abs(bearDirection.X) > ActivityThreshold)
                {
                    bearDirection.X = 0;
                }
                if (Math.Abs(bearDirection.Y) > ActivityThreshold)
                {
                    bearDirection.Y = 0;
                }
               

                if ((time.TotalGameTime.TotalSeconds - LastMoveTime) > (Speed))
                {
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

                    //Check for 
                    if (bearDirection.X == 0 && bearDirection.Y == 0)
                    {
                        if (Love == Settings.Person_LoveMax)
                        {
                            //on bear initiate hugging
                            isHugging = true;
                        }
                        else
                        {
                            //bearDirection.X = _rand.Next(2);
                            //bearDirection.Y = _rand.Next(2);
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
                            int currentIndex = indexOf;
                            for (int i = 0; i < 4; i++)
                            {
                                newPos = Pathfind_Nodes[(indexOf + i) % 8];
                                if (World.IsPassable(this.c_position + newPos))
                                {
                                    break;
                                }

                                newPos = Pathfind_Nodes[(indexOf + i + 4) % 8];
                                if (World.IsPassable(this.c_position + newPos))
                                {
                                    break;
                                }
                            }
                        }
                    }

                    MoveCell(time, newPos);
                    
                    if (bearDirection.Y > 0)
                    {
                        FacingDirection = Direction.Down;
                    }
                    else
                    {
                        FacingDirection = Direction.Up;
                    }

                    if (bearDirection.X > 0)
                    {
                        FacingDirection = Direction.Right;
                    }
                    else if(bearDirection.X < 0)
                    {
                        FacingDirection = Direction.Left;
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
                spriteIndex = 16 * (int)FacingDirection + (4 * (3 - Health));
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
            Color villagerColor = Color.White;
            
            /*switch (World.AllVillagers.IndexOf(this))
            {
                case 0:
                    villagerColor = Color.Blue;
                    break;
                case 1:
                    villagerColor = Color.Red;
                    break;
                case 2:
                    villagerColor = Color.Green;
                    break;
                case 3:
                    villagerColor = Color.Yellow;
                    break;
                default:
                    break;

            }*/
            spriteBatch_IN.Draw(SpriteTexture, position, sourceRec, villagerColor);
            
            //Draw Emotes
            if (emoteDisplayTime > 0 && !IsDead)
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
