using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monopoles {
    public static class GraphicsDraw{
        public const float ZCOEFF = -1;
        public const float YCOEFF = .5f;

        public static Vector2 ScreenOffset = Vector2.Zero;
        public static int ScreenOffsetX,ScreenOffsetY;

        public static SpriteBatch Batch;
        public static void BeforeDraw(SpriteBatch batch){
            Batch = batch;
        }
        
        public static void Draw(WorldObject obj,string tex,Vector2 center,float radius,Color color,string shadowtex = "Shadow"){
            int size = (int)radius*2;
            float a = 100f/(obj.Height+100);
            int size2 = (int)(obj.Radius*2*a);
            float zindex = .75f-(obj.Position.Y*YCOEFF/_G.ScreenSize.Y)*.5f;
            Batch.Draw(Assets.GetTex(shadowtex),new Rectangle((int)center.X-size2/2+ScreenOffsetX,(int)(_G.ScreenSize.Y/16+center.Y*YCOEFF+size2/4)-size2+ScreenOffsetY,size2,size2/2),null,color*(a),0,Vector2.Zero,SpriteEffects.None,zindex+.001f);
            Batch.Draw(Assets.GetTex(tex),new Rectangle((int)center.X-size/2+ScreenOffsetX,(int)(_G.ScreenSize.Y/16+center.Y*YCOEFF-size/2+obj.Height*ZCOEFF)-size2+ScreenOffsetY,size,size),null,color,0,Vector2.Zero,SpriteEffects.None,zindex);
        }

        public static void Draw(string tex,Vector2 center,Vector2 size,Color color,float ypos){
            Batch.Draw(Assets.GetTex(tex),new Rectangle((int)(center.X-size.X/2)+ScreenOffsetX,(int)(center.Y-size.Y/2)+ScreenOffsetY,(int)size.X,(int)size.Y),null,color,0,Vector2.Zero,SpriteEffects.None,1-ypos/_G.ScreenSize.Y);
        }

        public static void Draw(string tex,Vector2 center,Vector2 size,Color color,float rot,float ypos){
            Texture2D t = Assets.GetTex(tex);
            Batch.Draw(t,new Rectangle((int)(center.X-size.X)+ScreenOffsetX,(int)(center.Y-size.Y)+ScreenOffsetY,(int)size.X,(int)size.Y),null,color,rot,new Vector2(t.Width,t.Height)/2,SpriteEffects.None,1-ypos/_G.ScreenSize.Y);
        }
    }
}
