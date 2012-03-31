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

        public virtual void Begin(Actor doer, GameTime time)
        {
            IsActive = true;
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
        public Grab(Villager villager)
        {
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

