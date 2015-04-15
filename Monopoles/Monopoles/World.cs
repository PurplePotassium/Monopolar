using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Monopoles{
    public static class World{
        public const float MAGNETICFORCE = .05f;
        
        public static List<WorldObject> Objects;
        public static List<DropDown> Drops;
        public static List<Particle> Particles;

        public static float ShakeAmount;

        static int nextdrop;
        static int dropindex;

        static SoundEffectInstance Music;

        public static void Initialize(){
            if(Music == null){
                Music = Assets.GetSound("Sounds/Music").CreateInstance();
                Music.Volume = .2f;
            }

            Objects = new List<WorldObject>();
            Drops = new List<DropDown>();
            Particles = new List<Particle>();
            
            Player p = new Player(0);
            p.Position = new Vector2(_G.WorldSize.X*.2f,_G.WorldSize.Y/2);
            Objects.Add(p);
            p = new Player(1);
            p.Position = new Vector2(_G.WorldSize.X*.8f,_G.WorldSize.Y/2);
            Objects.Add(p);

            nextdrop = 60;
            dropindex = 0;

            Game1.Instance.TargetElapsedTime = Game1.DefaultTargetElapsedTime;
            
            /*for(int i = 0; i < 30; i++){
                Balltron b = new Balltron();
                b.Position = Rand.Vector2(_G.WorldSize);
                b.Height = Rand.Float(500,1000);
                Objects.Add(b);
            }*/
        }

        static int ending;
        public static void StartTheEnd(){
            if(ending != 0) return;
            ShakeAmount += 100;
            Game1.Instance.TargetElapsedTime = TimeSpan.FromMilliseconds(50);
            ending = 30;
        }

        public static void Update(){
            if(Music.State != SoundState.Playing)
                Music.Play();

            if(ending != 0){
                if(--ending == 0)
                    Initialize();
            }

            Vector2 newshakepos = Rand.DirectionVector(ShakeAmount)*new Vector2(1,GraphicsDraw.ZCOEFF);
            ShakeAmount *= .9f;
            GraphicsDraw.ScreenOffset = GraphicsDraw.ScreenOffset*.5f+newshakepos*.5f;
            GraphicsDraw.ScreenOffsetX = (int)GraphicsDraw.ScreenOffset.X;
            GraphicsDraw.ScreenOffsetY = (int)GraphicsDraw.ScreenOffset.Y;
            /*if(GraphicsDraw.ScreenOffsetX != 0)
                throw new ArgumentException();*/

            if(--nextdrop <= 0){
                Assets.GetSound("Sounds/Fall").Play(.25f,Rand.Float(.8f,1),0);
                DropDown b;
                for(int i = 0; i < Rand.Float(2,2+dropindex/2); i++){
                    bool large = dropindex > 0 && Rand.Chance(.1f*dropindex);
                    b = new DropDown(new Balltron(false,large),Rand.Vector2(_G.WorldSize));
                    Drops.Add(b);
                    b = new DropDown(new Balltron(true,large),Rand.Vector2(_G.WorldSize));
                    Drops.Add(b);
                    if(large && Rand.Int(dropindex) == 0) break;
                }
                if(dropindex > 3){
                    b = new DropDown(new Vacuum(),Rand.Vector2(_G.WorldSize));
                    Drops.Add(b);
                }
                if(dropindex > 0){// && Rand.Chance(.4f)){
                    b = new DropDown(new HealthPack(),Rand.Vector2(_G.WorldSize));
                    Drops.Add(b);
                    Drops.Add(new DropDown(new Bumper(true), Rand.Vector2(_G.WorldSize)));
                }

                nextdrop = 480;//Rand.Int(500,1000);
                ++dropindex;
            }
            
            for(int i = 0; i < Drops.Count; i++)
                Drops[i].Update();
            for(int i = 0; i < Particles.Count; i++)
                Particles[i].Update();
            for(int i = 0; i < Objects.Count; i++){
                WorldObject obj = Objects[i];
                for(int j = i+1; j < Objects.Count; j++){
                    if(!obj.Active) break;
                    WorldObject hit = Objects[j];
                    float dist = Vector2.Distance(obj.Position,hit.Position)-obj.Radius-hit.Radius+1;
                    if(Vector2.Distance(obj.Position,hit.Position) <= obj.Radius+hit.Radius){
                        if(hit.Position == obj.Position){
                            obj.Position += Rand.DirectionVector(1);
                            hit.Position += Rand.DirectionVector(1);
                        }

                        Vector2 point = (hit.Position+obj.Position)/2;

    		            float thismass = obj.Mass;
    		            float pmass = hit.Mass;
    		            float totalmass = thismass+pmass;
    		
    		            float totalvel = (obj.Anchored ? 0 : obj.Velocity.Length())+(hit.Anchored ? 0 : hit.Velocity.Length());
                        if(totalvel < .1f)
                            totalvel = .1f;
    		
                        Vector2 normalizedcollisionvector = Vector2.Normalize(obj.Position-hit.Position);

                        if(!obj.Anchored)
    		                obj.Velocity += normalizedcollisionvector*totalvel*pmass/totalmass;
                        if(!hit.Anchored)
    		                hit.Velocity += -normalizedcollisionvector*totalvel*thismass/totalmass;

                        if(!obj.Anchored)
                            obj.Position = hit.Position+Vector2.Normalize(obj.Position-hit.Position)*(hit.Radius+obj.Radius+.0001f)/2;
                        
                        obj.HitBy(hit);
                        hit.HitBy(obj);
                        obj.PlaySound("Sounds/Tick"+Rand.Int(4),.1f,1);
                        new Particle((obj.Position+hit.Position)*.5f,Rand.DirectionVector(10),(obj.Height+hit.Height+obj.Radius+hit.Radius)*.25f);
                        /*if(!hit.Anchored)
                            hit.Position = hit.Position-Vector2.Normalize(obj.Position-hit.Position)*(hit.Radius+obj.Radius+.0001f)/2;*/
                    }else if(hit.Magnetic && obj.Magnetic){
                        if(obj.AttractAll){
                            Vector2 force = Vector2.Normalize(hit.Position-obj.Position)*obj.Mass*hit.Mass*MAGNETICFORCE*obj.Magneticness/(dist);//*dist);
                            obj.Velocity -= force/obj.Mass;
                            hit.Velocity -= force/hit.Mass;
                        }else if(hit.AttractAll){
                            Vector2 force = Vector2.Normalize(hit.Position-obj.Position)*obj.Mass*hit.Mass*MAGNETICFORCE*obj.Magneticness/(dist);//*dist);
                            obj.Velocity += force/obj.Mass;
                            hit.Velocity += force/hit.Mass;
                        }else{
                            Vector2 force = Vector2.Normalize(hit.Position-obj.Position)*obj.Mass*hit.Mass*MAGNETICFORCE*(obj.IsNorth == hit.IsNorth ? 1 : -1)*obj.Magneticness/(dist);//*dist);
                            obj.Velocity -= force/obj.Mass;
                            hit.Velocity += force/hit.Mass;
                        }
                    }
                }
                obj.Update();
            }
        }

        public static void Draw(){
            for(int i = 0; i < Drops.Count; i++)
                Drops[i].Draw();
            for(int i = 0; i < Particles.Count; i++)
                Particles[i].Draw();
            for(int i = 0; i < Objects.Count; i++)
                Objects[i].Draw();
            GraphicsDraw.Draw("Map",_G.ScreenSize/2,new Vector2(1920,1080),new Color(0,.5f,0),0);
        }
    }
}
