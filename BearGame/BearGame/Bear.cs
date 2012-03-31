using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BearGame
{
    public class Bear : Actor
    {
        public float Health { get; set; }

        public Prop Inventory { get; set; }

        public Bear(World world)
            : base(world)
        {
            this.Health = Settings.Bear_HealthDefault;
            FacingDirection = Direction.Down;
        }

        double _lastInteractionBeginTime = 0;

        public Interaction PossibleInteraction { get; private set; }

        public bool IsHugging { get { return ActiveInteraction != null && ActiveInteraction is Grab; } }

        public override void Update(GameTime time)
        {
            var now = time.TotalGameTime.TotalSeconds;

            //
            // Movement input
            //
            var keyState = Keyboard.GetState();            

            if ((now - LastMoveTime) > Settings.Bear_MoveInterval)
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
                if (other != null && other != this)
                {
                    var inter = GetPossibleInteration(other);
                    if (inter != null)
                    {
                        PossibleInteraction = inter;
                    }
                }
            }
            else
            {
                PossibleInteraction = null;
            }

            //
            // Interaction input
            //
            if (((now - _lastInteractionBeginTime) > Settings.Bear_InteractionInterval) &&
                (keyState.IsKeyDown(Keys.Space)) &&
                (PossibleInteraction != null)) {

                BeginInteraction(time, PossibleInteraction);
                PossibleInteraction = null;
            }

            //
            // The ever-decreasing health
            //
            Health -= (float)(time.ElapsedGameTime.TotalSeconds * Settings.Bear_HealthDecreaseRate);

            if (Health < 0)
            {
                Health = 0;
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
                    return new AchievementUnlockedDaredevil();
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
                    return new GiveHoney((Villager)obj);
                }
                else if (Inventory is Tricycle)
                {
                    return new RunOver();
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

        protected override void  UpdateSpriteIndex()
        {
            if (ActiveInteraction != null && ActiveInteraction is Grab)
            {
                spriteIndex = 64;
            }
            else
            {
                spriteIndex = 16 * (int)FacingDirection;
            }
        }
    }
}
