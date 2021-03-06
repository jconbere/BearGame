﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace BearGame
{
    public class Bear : Actor
    {
        Random _rand;

        public static RandomSound FootstepSound;
        public static RandomSound DragSound;
        public static RandomSound TrikeSound;
        public static RandomSound bearGruntSound;
        public static RandomSound bearHappySound;

        private double nextsoundplay = 1;
        const double sounddelayVariance = 5;
        const double sounddelayMin = 2;

        public static int currentFrame = 0;
        double frameSpeed
        {
            get
            {
                return TransitionInterval / 4;
            }
        }
        double lastAnimTime = 0;

        float _health;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = Math.Min(Settings.Bear_HealthMax, Math.Max(Settings.Bear_HealthMin, value));
            }
        }

        public Prop Inventory { get; set; }

        public Bear(World world)
            : base(world)
        {
            this.Health = Settings.Bear_HealthDefault;
            FacingDirection = Direction.Down;
            Achievements = new List<Achievement>();

            _rand = new Random();
        }

        public List<Achievement> Achievements { get; private set; }

        public bool AddAchievement(GameTime time, Achievement achievement)
        {
            var a = Achievements.FirstOrDefault(x => x.GetType() == achievement.GetType());
            if (a == null)
            {
                Achievements.Add(achievement);
                achievement.Begin(time);
                return true;
            }
            else
            {
                return false;
            }
        }

        double _lastInteractionBeginTime = 0;

        public Interaction PossibleInteraction { get; private set; }

        public bool IsHugging { get { return ActiveInteraction != null && ActiveInteraction is Grab; } }

        double _lastImpressTime = 0;

        public static RandomSound PersonAwwSound;

        protected override void OnMove(GameTime time)
        {
            var now = time.TotalGameTime.TotalSeconds;
            //
            // Drag people
            //
            var grab = ActiveInteraction as Grab;
            if (grab != null)
            {
                grab.Villager.MoveToCell(time, c_position);
                DragSound.Play();
            }
            else
            {
                if (Inventory is Tricycle)
                {
                    TrikeSound.Play();
                }
                else
                {
                    FootstepSound.Play();
                }
            }

            //
            // Impress villages when on bike
            //
            var tri = Inventory as Tricycle;
            if (tri != null && (now - _lastImpressTime) > Settings.Bear_RidingImpressInterval)
            {
                var impressed = false;
                foreach (var v in World.AllVillagers)
                {
                    if (v.c_position.DistanceTo(c_position) <= Settings.Bear_RidingImpressPeopleDistance)
                    {
                        if (v.LoveFromTricycle < Settings.Person_MaxTricycleLove && !v.IsDead)
                        {
                            v.LoveFromTricycle += Settings.Bear_RidingImpressLoveIncrease;
                            v.ChangeLove(Settings.Bear_RidingImpressLoveIncrease, time);
                            impressed = true;
                        }
                    }
                }
                if (impressed)
                {
                    PersonAwwSound.Play();
                    _lastImpressTime = now;
                }
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            var now = time.TotalGameTime.TotalSeconds;

            if (now > nextsoundplay)
            {
                nextsoundplay = now + (sounddelayVariance * _rand.NextDouble() + sounddelayMin);

                if (Health / Settings.Bear_HealthMax < 0.3)
                {
                    bearGruntSound.Play(0.5f);
                }
                else if (Health / Settings.Bear_HealthMax > 0.7)
                {
                    bearHappySound.Play(0.5f);
                }
            }
            //
            // Movement input
            //
            var keyState = Keyboard.GetState();

            var moveInterval = TransitionInterval;

            if ((now - LastMoveTime) > moveInterval)
            {
                if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                {
                    MoveCell(time, new CellPosition(-1, 0));
                    FacingDirection = Direction.Left;
                }
                else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                {
                    MoveCell(time, new CellPosition(0, -1));
                    FacingDirection = Direction.Up;
                }
                else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                {
                    MoveCell(time, new CellPosition(1, 0));
                    FacingDirection = Direction.Right;
                }
                else if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                {
                    MoveCell(time, new CellPosition(0, 1));
                    FacingDirection = Direction.Down;
                }
            }

            //
            // Find possible interactions
            //
            if (ActiveInteraction == null)
            {
                var other = World.GetEntityInSameLocation(this);
                PossibleInteraction = GetPossibleInteration(other);
            }
            else
            {
                PossibleInteraction = null;
            }

            //
            // Interaction input
            //
            if (ActiveInteraction == null)
            {
                if (((now - _lastInteractionBeginTime) > Settings.Bear_InteractionInterval) &&
                    (keyState.IsKeyDown(Keys.Space)) &&
                    (PossibleInteraction != null))
                {
                    _lastInteractionBeginTime = now;
                    BeginInteraction(time, PossibleInteraction);
                    PossibleInteraction = null;
                }
            }
            else
            {
                if (!(keyState.IsKeyDown(Keys.Space)))
                {
                    EndInteraction(time);
                }
            }

            //
            // Update the achievements
            //
            foreach (var a in Achievements)
            {
                a.Update(time);
            }

            //
            // The ever-decreasing health
            //
            Health -= (float)(time.ElapsedGameTime.TotalSeconds * Settings.Bear_HealthDecreaseRate);

            if (Health < 0)
            {
                Health = 0;
            }
            UpdateSpriteIndex(time);
        }

        protected override double TransitionInterval
        {
            get
            {
                return (Inventory != null && Inventory is Tricycle) ? Settings.Bear_RidingMoveInterval : Settings.Bear_MoveInterval;
            }
        }

        public Interaction GetPossibleInteration(Entity obj)
        {
            if (obj is Honey)
            {
                if (Inventory == null)
                {
                    return new TakeHoney((Honey)obj);
                }
                else if (Inventory is Honey)
                {
                    return new EatHoney();
                }
                else if (Inventory is Tricycle)
                {
                    return new Daredevil();
                }
                else
                {
                    return null;
                }
            }
            else if (obj is Tricycle)
            {
                if (Inventory == null)
                {
                    return new RideTricycle((Tricycle)obj);
                }
                else if (Inventory is Honey)
                {
                    return new EatHoney();
                }
                else if (Inventory is Tricycle)
                {
                    return new GetOffTricycle();
                }
                else
                {
                    return null;
                }
            }
            else if (obj is Villager)
            {
                if (Inventory == null)
                {
                    return new Grab((Villager)obj);
                }
                else if (Inventory is Honey)
                {
                    var v = (Villager)obj;
                    if (!v.IsDead)
                    {
                        return new GiveHoney(v);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (Inventory is Tricycle)
                {
                    return new RunOver((Villager)obj);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (Inventory == null)
                {
                    return null;
                }
                else if (Inventory is Honey)
                {
                    return new EatHoney();
                }
                else if (Inventory is Tricycle)
                {
                    return new GetOffTricycle();
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void  UpdateSpriteIndex(GameTime gametime)
        {
               
            if (ActiveInteraction != null && ActiveInteraction is Grab)
            {
                spriteIndex = 64 + currentFrame;
            }
            else if (Inventory is Honey)
            {
                spriteIndex = (12 + (16 * (int)FacingDirection)) + currentFrame;
            }
            else if (Inventory is Tricycle)
            {
                spriteIndex = (8 + (16 * (int)FacingDirection)) + currentFrame;
            }
            else
            {
                spriteIndex = (16 * (int)FacingDirection) + currentFrame;
            }

            if ((gametime.TotalGameTime.TotalSeconds - lastAnimTime > frameSpeed) && (IsTransitioning))
            {
                lastAnimTime = gametime.TotalGameTime.TotalSeconds;
                currentFrame++;
                if (currentFrame >= 4)
                { currentFrame = 0; }
            }
        }
    }
}
