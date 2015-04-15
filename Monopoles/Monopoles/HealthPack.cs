using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monopoles{
    public class HealthPack : WorldObject{
        public HealthPack() : base(50){
            Magnetic = false;
            ShakeOnHitGround = false;
        }

        public override void Update(){
            base.Update();
            Velocity *= .8f;
        }

        public override void Draw(){
            GraphicsDraw.Draw(this,"Health",Position+new Vector2(0,Radius),Radius,Color.White,"HealthShadow");
        }
    }
}
