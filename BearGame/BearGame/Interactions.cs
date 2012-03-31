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
        Villager _v;

        public Grab(Villager villager)
        {
            _v = villager;
        }

        public override void OnBegin(Actor doer, GameTime time)
        {
            base.OnBegin(doer, time);
            _v.IsVisible = false;
        }

        public override void Update(Actor doer, GameTime time)
        {
            base.Update(doer, time);

            var dt = time.ElapsedGameTime.TotalSeconds;

            _v.LoveValue -= (float)(dt * _v.Settings.Person_LoveLossDuringHugRate);
            _v.Health -= (float)(dt * _v.Settings.Person_HealthLossDuringHugRate);

            ((Bear)doer).Health += (float)(dt * _v.Settings.Person_HealthLossDuringHugRate);
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

