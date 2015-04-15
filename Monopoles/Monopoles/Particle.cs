using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monopoles{
    public class Particle : WorldObject{
        public Particle(Vector2 pos,Vector2 vel,float height) : base(5){
            Position = pos;
            Velocity = vel;
            Height = height;
            ShakeOnHitGround = false;
            World.Particles.Add(this);
        }

        float life = .99f;
        public override void Update(){
            base.Update();
            if((life = (float)Math.Pow(life,1.5f)) < .1f)
                World.Particles.Remove(this);
        }

        public override void Draw(){
            Vector2 size = new Vector2(Velocity.Length()*5,5)*life;//Velocity*new Vector2(1,GraphicsDraw.YCOEFF)*5*life;
            GraphicsDraw.Batch.Draw(Assets.GetTex("Particle"),new Rectangle((int)(Position.X-size.X/2)+GraphicsDraw.ScreenOffsetX,(int)(_G.ScreenSize.Y/16+Position.Y*GraphicsDraw.YCOEFF-size.Y/2+Height*GraphicsDraw.ZCOEFF)+GraphicsDraw.ScreenOffsetY,(int)size.X,(int)size.Y),null,Color.White,VectorMath.ToAngle(Velocity),new Vector2(150,150),SpriteEffects.None,.75f-(Position.Y*GraphicsDraw.YCOEFF/_G.ScreenSize.Y)*.5f);
        }
    }
}
