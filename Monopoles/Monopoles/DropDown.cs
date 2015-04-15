using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monopoles{
    public class DropDown{
        WorldObject Object;
        static Vector2 size = new Vector2(100,50);
        Vector2 Position;
        public DropDown(WorldObject obj,Vector2 pos){
            Object = obj;
            Position = pos;
        }

        int cycle;
        public void Update(){
            if(++cycle == 120){
                Object.Position = Position;
                Object.Height = _G.ScreenSize.Y+Rand.Float(100,500);
                World.Objects.Add(Object);
                World.Drops.Remove(this);
            }
        }

        public void Draw(){
            //if(cycle%30 < 15)
            float a = 1.5f-(float)(cycle%30)/30;
            a = a*a*a*a*a*a;
            GraphicsDraw.Batch.Draw(Assets.GetTex("EXCLAIM"),new Rectangle((int)(Position.X-size.Y/2)+GraphicsDraw.ScreenOffsetX,(int)(_G.ScreenSize.Y/16+Position.Y*GraphicsDraw.YCOEFF-size.X/2-35)+GraphicsDraw.ScreenOffsetY,(int)size.Y,(int)size.X),null,Color.White*a,0,Vector2.Zero,SpriteEffects.None,.75f-(Position.Y*GraphicsDraw.YCOEFF/_G.ScreenSize.Y)*.5f-.01f);
            GraphicsDraw.Batch.Draw(Assets.GetTex("Danger"),new Rectangle((int)(Position.X-size.X/2)+GraphicsDraw.ScreenOffsetX,(int)(_G.ScreenSize.Y/16+Position.Y*GraphicsDraw.YCOEFF-size.Y/2)+GraphicsDraw.ScreenOffsetY,(int)size.X,(int)size.Y),null,Color.White*a,0,Vector2.Zero,SpriteEffects.None,.75f-(Position.Y*GraphicsDraw.YCOEFF/_G.ScreenSize.Y)*.5f);
        }
    }
}
