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
        public TakeHoney(Honey honey)
        {
        }
    }

    public class EatHoney : Interaction
    {
        public EatHoney()
        {
        }
    }

    public class AchievementUnlockedDaredevil : Interaction
    {
    }

    public class RideTricycle : Interaction
    {
        public RideTricycle(Tricycle tri)
        {
        }
    }

    public class GetOffTricycle : Interaction
    {
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

            Villager.Love -= Villager.Settings.Person_LoveLossDuringHug;
            Villager.Health -= Villager.Settings.Person_HealthLossDuringHug;

            if (Villager.Health < 1)
            {
                IsActive = false;
            }
            else
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
        public GiveHoney(Villager villager)
        {
        }
    }

    public class RunOver : Interaction
    {
    }
}

