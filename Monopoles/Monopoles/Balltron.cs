using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monopoles{
    public class Balltron : WorldObject{
        public Balltron(bool north,bool large) : base(large ? 25 : 10){
            IsNorth = north;
        }

        public override void Draw(){
            GraphicsDraw.Draw(this,"Mag",Position,Radius,IsNorth ? new Color(1f,.4f,.4f) : new Color(.4f,.4f,1));
        }
    }

    public class Vacuum : WorldObject{
        public Vacuum() : base(35){
            Magnetic = true;
            AttractAll = true;
        }

        public override void HitBy(WorldObject other){
            base.HitBy(other);
            if(other.GetType() == typeof(Balltron) || other.GetType() == typeof(HealthPack)){
                World.Objects.Remove(other);
                Radius += (float)Math.Sqrt(other.Radius);
                RecalculateMass();
                PlaySound("Sounds/PickupCollectible",.17f,_G.BindFloat(25f/other.Radius,-1,1));
                //PlaySound("Sounds/Nom",.3f,_G.BindFloat(1f/other.Radius,-1,1));
            }
        }

        bool removing;
        public override void Update(){
            base.Update();
            Radius = (float)Math.Pow(Radius,.999);
            RecalculateMass();
            if(Radius < 10){
                removing = true;
            }
            if(removing){
                FallingSpeed -= 2f;
                Height += 1;
            }
        }

        public override void Draw(){
            GraphicsDraw.Draw(this,"Mag",Position,Radius,Color.Black);
        }
    }
}
