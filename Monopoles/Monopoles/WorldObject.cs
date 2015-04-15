using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monopoles{
    public abstract class WorldObject{
        public Vector2 Position = Vector2.Zero,Velocity = Vector2.Zero;
        public float FallingSpeed,Height;
        public float Radius;
        public float Mass;

        public bool Collidable = true;
        public bool Magnetic = true;
        public bool IsNorth = true;
        public bool Anchored = false;
        public bool Active = true;
        public float Magneticness = 1;
        public bool YesFall = true;
        public bool ShakeOnHitGround = true;
        public bool AttractAll = false;

        public WorldObject(float radius){
            Radius = radius;
            Mass = Radius*Radius;
        }

        public void RecalculateMass(){
            Mass = Radius*Radius;
        }

        public void PlaySound(string path,float vol,float pitch){
            Assets.GetSound(path).Play(vol,pitch,_G.BindFloat(Position.X/_G.WorldSize.X,-1,1));
        }

        public virtual void HitBy(WorldObject other){}

        public virtual void Update(){
            if(YesFall && Height > 0){
                if((Height -= (FallingSpeed += 1)) < 0){
                    Height += FallingSpeed;
                    if(ShakeOnHitGround){
                        World.ShakeAmount += Math.Abs(FallingSpeed*Radius*.01f);
                        if(FallingSpeed > 1 && Radius > 1){
                            float a = (float)Math.Sqrt(1f/Radius);
                            if(Radius > 20)
                                PlaySound("Sounds/HitGroundBig",.2f,1f/FallingSpeed-(1-a));
                            else
                                PlaySound("Sounds/HitGround",(1-1f/FallingSpeed)*.05f,0);
                        }
                    }
                    if(Math.Abs(FallingSpeed *= -.5f) < 1){
                        Height = FallingSpeed = 0;
                    }
                }
                Active = Height < 30;
            }

            if(!Anchored){
                Position += Velocity;
                Velocity *= .97f;
                if(Position.X < Radius){
                    Position = new Vector2(Radius+1,Position.Y);
                    Velocity *= new Vector2(-1f,1);
                }else if(Position.X > _G.WorldSize.X-Radius){
                    Position = new Vector2(_G.WorldSize.X-Radius-1,Position.Y);
                    Velocity *= new Vector2(-1f,1);
                }
                if(Position.Y < Radius){
                    Position = new Vector2(Position.X,Radius);
                    Velocity *= new Vector2(1,-1);
                }else  if(Position.Y > _G.WorldSize.Y-Radius){
                    Position = new Vector2(Position.X,_G.WorldSize.Y-Radius-1);
                    Velocity *= new Vector2(1,-1f);
                }
            }
            /*if(Height > 0 && (Height -= (FallingSpeed += 1)) < 0 && Math.Abs(FallingSpeed *= -1f) < 1 && (Height = .1f) > 0)//Most amazing line of code I have every written
                Height = FallingSpeed = 0;*/
        }

        public abstract void Draw();
    }
}
