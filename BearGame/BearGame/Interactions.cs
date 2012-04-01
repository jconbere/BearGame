using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public class Interaction
    {
        public bool IsActive { get; protected set; }
        public int spritesheetIndex { get; protected set; }

        public virtual void OnBegin(Actor doer, GameTime time)
        {
            IsActive = true;
        }

        public virtual void OnEnd(Actor doer, GameTime time)
        {
            IsActive = false;
        }

        public virtual void Update(Actor doer, GameTime time)
        {
        }
    }

    public class TakeHoney : Interaction
    {
        public Honey Honey { get; private set; }

        public static RandomSound PickUpHoneySound;

        public TakeHoney(Honey honey)
        {
            spritesheetIndex = 1;
            Honey = honey;
        }

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);
            ((Bear)doer).Inventory = Honey;
            Honey.IsVisible = false;
            Honey.IsActive = false;
            PickUpHoneySound.Play();
        }
    }

    public class EatHoney : Interaction
    {
        public EatHoney()
        {
            spritesheetIndex = 2;
        }

        public static RandomSound EatHoneySound;

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);

            var bear = (Bear)doer;

            var honey = (Honey)bear.Inventory;

            bear.Inventory = null;

            bear.Health += bear.Settings.Bear_HealthGainForHoney;

            EatHoneySound.Play();
        }
    }

    public class AchievementUnlockedDaredevil : Interaction
    {
    }

    public class RideTricycle : Interaction
    {
        public static RandomSound GetOnSound;

        public Tricycle Tricycle { get; set; }

        public RideTricycle(Tricycle tri)
        {
            Tricycle = tri;
        }

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);
            ((Bear)doer).Inventory = Tricycle;
            Tricycle.IsVisible = false;
            GetOnSound.Play();
            IsActive = false;
        }
    }

    public class GetOffTricycle : Interaction
    {
        public static RandomSound GetOffSound;

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);
            var bear = (Bear)doer;
            bear.Inventory.c_position = doer.c_position;
            bear.Inventory.IsVisible = true;
            bear.Inventory = null;            
            GetOffSound.Play();
            IsActive = false;
        }
    }

    public class Grab : Interaction
    {
        public Villager Villager { get; private set; }

        public Grab(Villager villager)
        {
            Villager = villager;
        }

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);
            Villager.IsVisible = false;

            Villager.ChangeLove(doer.Settings.Person_LoveLossDuringHug, time);
            Villager.Health -= Villager.Settings.Person_HealthLossDuringHug;

            if (!Villager.IsDead)
            {
                ((Bear)doer).Health += Villager.Settings.Bear_HealthGainDuringHug;
            }
        }

        public override void OnEnd(Actor doer, GameTime time)
        {
            base.OnEnd(doer, time);
            Villager.IsVisible = true;
        }
    }

    public class GiveHoney : Interaction
    {
        public Villager Villager { get; private set; }

        public static RandomSound PersonThanksSound;

        public GiveHoney(Villager villager)
        {
            spritesheetIndex = 0;
            Villager = villager;
        }

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);

            var honey = (Honey)((Bear)doer).Inventory;

            if (Villager.IsDead || !Villager.IsActive)
            {
                //IsActive = false;
            }
            else
            {
                Villager.ChangeLove(doer.Settings.Person_LoveIncreaseForHoney, time);
                Villager.Health += doer.Settings.Person_HealthIncreaseForHoney;
                honey.IsActive = false;
                ((Bear)doer).Inventory = null;
                honey.IsVisible = false;
                PersonThanksSound.Play();
                //IsActive = false;
            }
        }
    }
    

    public class RunOver : Interaction
    {
    }
}

