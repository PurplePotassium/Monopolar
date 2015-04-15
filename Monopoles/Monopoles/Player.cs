using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Monopoles{
    public class Player : WorldObject{
        const float MOVESPEED = 8;
        const float BURSTCOOLDOWN = 3;

        public static Keys[,] Controls;
        static Player(){
            Controls = new Keys[2,5];
            Controls[0,0] = Keys.W;
            Controls[0,1] = Keys.A;
            Controls[0,2] = Keys.S;
            Controls[0,3] = Keys.D;
            Controls[0,4] = Keys.Space;

            Controls[1,0] = Keys.Up;
            Controls[1,1] = Keys.Left;
            Controls[1,2] = Keys.Down;
            Controls[1,3] = Keys.Right;
            Controls[1,4] = Keys.RightControl;
        }

        public int PlayerIndex;

        Vector2 MovementVector;
        int texindex;

        bool bursting;

        public float Health = 1;

        public Player(int playerindex) : base(16){
            PlayerIndex = playerindex;
            Magnetic = true;
            Height = 0;
            YesFall = false;
            resetburst();
            IsNorth = PlayerIndex == 0;
        }

        void resetburst(){
            Magneticness = 1;
            bursting = false;
        }

        float damaged;
        float healing;
        public void ChangeHealth(float amount){
            if(Health <= 0) return;
            if(amount < 0)
                damaged = .995f;
            if((Health += amount) <= 0){
                World.StartTheEnd();
                PlaySound("Sounds/Lose",.25f,0);
            }else if(Health > 1){
                healing = 0;
                Health = 1;
            }
        }

        public override void HitBy(WorldObject other){
            if(other as HealthPack != null){
                healing = .4f;
                World.Objects.Remove(other);
                //PlaySound("Sounds/PickupCollectible",.17f,0);
                PlaySound("Sounds/Nom",.5f,0);
            }else if(IsNorth != other.IsNorth || other.AttractAll){
                ChangeHealth(-other.Radius/250*(bursting ? 2 : 1));
            }
        }

        bool f_up,f_down,f_left,f_right;
        float floatcycle;
        float dyinganim = 1;
        public override void Update(){
            base.Update();

            if(Health <= 0){
                Health = 0;
                dyinganim *= .9f;
                for(int i = 0; i < 10; i++)
                    new Particle(Position+new Vector2(0,1),Velocity+Rand.DirectionVector(0,20),Height+Rand.Float(Radius*2));
                return;
            }
            
            MovementVector = Vector2.Zero;
            GamePadState state = GamePad.GetState(PlayerIndex == 0 ? Microsoft.Xna.Framework.PlayerIndex.One : Microsoft.Xna.Framework.PlayerIndex.Two);
            if(state.IsConnected){
                MovementVector = state.ThumbSticks.Left+state.ThumbSticks.Right;
                /*if(Rand.Chance(.1f))
                    throw new ArgumentException(""+MovementVector);*/
                if(MovementVector != Vector2.Zero){
                    Vector2 normal = Vector2.Normalize(MovementVector)*new Vector2(1,-1);
                    MovementVector = normal*_G.BindFloat(MovementVector.Length(),0,1);
                    f_up = normal.Y < -.5f;
                    f_down = normal.Y > .5f;
                    f_left = normal.X < -.5f;
                    f_right = normal.X > .5f;
                }
            }else{
                if(f_up = PlayerMouse.KeyDown(Controls[PlayerIndex,0])){
                    MovementVector += new Vector2(0,-1);
                }
                if(f_left = PlayerMouse.KeyDown(Controls[PlayerIndex,1])){
                    MovementVector += new Vector2(-1,0);
                }
                if(f_down = PlayerMouse.KeyDown(Controls[PlayerIndex,2])){
                    MovementVector += new Vector2(0,1);
                }
                if(f_right = PlayerMouse.KeyDown(Controls[PlayerIndex,3])){
                    MovementVector += new Vector2(1,0);
                }
                if(MovementVector != Vector2.Zero)
                    MovementVector.Normalize();
            }

            if(MovementVector != Vector2.Zero){
                //texindex = (int)((VectorMath.ToAngle(MovementVector)/_G.Tau)*8);
                if(f_down){
                    if(f_left)
                        texindex = 3;
                    else if(f_right)
                        texindex = 1;
                    else
                        texindex = 2;
                }else if(f_up){
                    if(f_left)
                        texindex = 5;
                    else if(f_right)
                        texindex = 7;
                    else
                        texindex = 6;
                }else if(f_left)
                    texindex = 4;
                else if(f_right)
                    texindex = 0;
            }

            if(PlayerMouse.KeyDown(Controls[PlayerIndex,4]) ||
                state.IsConnected && (state.Buttons.A == ButtonState.Pressed || state.Buttons.B == ButtonState.Pressed || state.Buttons.X == ButtonState.Pressed || state.Buttons.Y == ButtonState.Pressed || state.Triggers.Right > .5f || state.Triggers.Left > .5f)){
                Magneticness = 10;
                bursting = true;
                PlaySound("Sounds/Electric",.05f,1);
            }else{
                resetburst();
            }

            Velocity = Velocity*.8f+MovementVector*MOVESPEED*.2f;
            floatcycle += 1+MovementVector.Length();
            damaged *= damaged;

            if(healing > 0){
                healing -= .01f;
                ChangeHealth(.01f);
            }
        }

        public override void Draw(){
            Height = 6+(float)Math.Sin(floatcycle*.2f)*3;
            if(Health <= 0){
                float a = dyinganim*dyinganim;
                GraphicsDraw.Draw(this,"Player"+PlayerIndex+"/"+texindex,Position,Radius*2,new Color(a,a,a));
            }else{
                GraphicsDraw.Draw(this,"Player"+PlayerIndex+"/"+texindex,Position,Radius*2,Color.White);
            }
            if(bursting){
                GraphicsDraw.Draw("Burst"+PlayerIndex,Position*new Vector2(1,.5f)+new Vector2(0,20),new Vector2(741,391)*.1f*Rand.Float(1,1.5f),Color.White,1);
            }

            Vector2 v = new Vector2(0,32);

            float d = damaged/2;
            GraphicsDraw.Draw("Frame",Position*new Vector2(1,.5f)-v,new Vector2(60,6),new Color(.5f+d,.5f-d,.5f-d),_G.ScreenSize.Y-3);
            if(healing > 0)
                GraphicsDraw.Draw("Frame",Position*new Vector2(1,.5f)-v,new Vector2(60*(Health+healing),6),Color.LightGreen,_G.ScreenSize.Y-2);
            GraphicsDraw.Draw("Frame",Position*new Vector2(1,.5f)-v,new Vector2(60*Health,6),bursting ? (PlayerIndex == 0 ? Color.Red : Color.Blue) : Color.Orange,_G.ScreenSize.Y-1);
            GraphicsDraw.Draw("Frame",Position*new Vector2(1,.5f)-v-new Vector2(30*Health-.5f,3),new Vector2(1,10),Color.Black,_G.ScreenSize.Y-0);
            GraphicsDraw.Draw("Frame",Position*new Vector2(1,.5f)-v+new Vector2(30*Health-.5f,3),new Vector2(1,10),Color.Black,_G.ScreenSize.Y-0);
            GraphicsDraw.Draw("FrameFront",Position*new Vector2(1,.5f)-v,new Vector2(60,6),Color.White,_G.ScreenSize.Y);
        }
    }
}
