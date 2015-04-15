using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoles
{
    public class Bumper : WorldObject
    {

        public Bumper(bool large)
            : base(large ? 32 : 15)
        {
            Anchored = true;
            Magnetic = false;
        }

        public override void Draw()
        {
            GraphicsDraw.Draw(this, "Test_Bumper", Position, Radius, new Color(1f, .1f, 1f));
        }


    }
}