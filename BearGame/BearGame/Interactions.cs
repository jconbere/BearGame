using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BearGame
{
    public static class Interactions
    {
        public static Interaction GetAvailableInteration(Bear bear, Entity obj)
        {
            
            if (obj is Honey)
            {
                if (bear.Inventory == null)
                {
                    return new TakeHoney((Honey)obj);
                }
                else if (bear.Inventory is Honey)
                {
                    return new EatHoney();
                }
                else if (bear.Inventory is Tricycle)
                {
                    return new AchievementUnlockedDaredevil();
                }
            }
            else if (obj is Tricycle)
            {
                if (bear.Inventory == null)
                {
                    return new RideTricycle((Tricycle)obj);
                }
                else if (bear.Inventory is Honey)
                {
                    return new EatHoney();
                }
                else if (bear.Inventory is Tricycle)
                {
                    return new GetOffTricycle();
                }
            }
            else if (obj is Villager)
            {
            }
            else
            {
            }

            throw new NotImplementedException();
        }
    }

    public class Interaction
    {
        public bool IsActive { get; protected set; }

        public virtual void Activate(Bear bear, GameTime time)
        {
            IsActive = true;
        }

        public virtual void Update(Bear bear, GameTime time)
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
    }

    public class GiveHoney : Interaction
    {
    }

    public class RunOver : Interaction
    {
    }
}

