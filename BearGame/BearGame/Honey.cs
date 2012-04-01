using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearGame
{
    public class Honey : Prop
    {
        public Honey(World world)
            : base(world)
        {
            spriteIndex = 0;
        }

        protected override void UpdateSpriteIndex()
        {
            spriteIndex = 0;
        }
    }
}
