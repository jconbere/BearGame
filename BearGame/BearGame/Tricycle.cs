using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearGame
{
    public class Tricycle : Prop
    {
        public Tricycle(World world)
            : base (world)
        {
            spriteIndex = 3;
        }

        protected override void UpdateSpriteIndex()
        {
            spriteIndex = 3;
        }
    }
}
