using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monopoles{
    public static class VectorMath{
        static float Pi = _G.Pi;

        public static float ToAngle(Vector2 v){
            if(v.X == 0)
                return v.Y >= 0 ? _G.Pi/2 : _G.Pi*3/2;

            if(v.Y == 0)
                return v.X < 0 ? _G.Pi : 0;

            float r = (float)Math.Atan(v.Y/v.X);
            return _G.NormalAngle(v.X < 0 ? _G.Pi+r : r);
        }

        public static float ToAngle(Vector2 v1, Vector2 v2){
	        return ToAngle(v2-v1);
        }
        
        public static Vector2 ToVector2(Vector3 v){return new Vector2(v.X,v.Y);}

        public static float NormalizeRotation(float rotation){
            return ((rotation+Pi)%(Pi*2))-Pi;
        }

        public static float LerpRotation(float r1,float r2,float dist){
            r1 = NormalizeRotation(r1);
            r2 = NormalizeRotation(r2);

            float r3 = r2-r1;

            return r1+(r3*dist);
        }

        public static Vector2 FromAngle(float angle){
	        return new Vector2((float)Math.Cos(angle),(float)Math.Sin(angle));
        }
        public static Vector2 FromAngle(float angle,float magnitude){
	        return new Vector2((float)Math.Cos(angle)*magnitude,(float)Math.Sin(angle)*magnitude);
        }

        public static Vector2 Rotate(Vector2 v,float rot){
            float lensq = v.LengthSquared();
            if(lensq == 0) return v;
            return FromAngle(ToAngle(v)+rot,(float)Math.Sqrt(lensq));
        }

        public static Vector2 BothAxesAs(float f){
            return new Vector2(f,f);
        }

        public static Vector2[] GetPointsOnRay(Vector2 v1,Vector2 v2,float interval){
            float mag = Vector2.Distance(v1,v2);
            int num = (int)(mag/interval)+1;
            Vector2[] ans = new Vector2[num];
            Vector2 unit = Vector2.Normalize(v2-v1)/num;

            for(int i = 0; i < num; i++){
                ans[i] = v1+unit*i;
            }

            return ans;
        }
        
        public static bool CircleOnLine(Vector2 p0, Vector2 p1, Vector2 circpos, float radius){
            if(_G.AngleDistance(VectorMath.ToAngle(p0,p1),VectorMath.ToAngle(p0,circpos)) > _G.HalfPi) return false;
            Vector2 a = GetRayIntersectionOnCircle(p0,p1,circpos,radius);
            return !float.IsNaN(a.X) && Vector2.DistanceSquared(p0,a) < Vector2.DistanceSquared(p0,p1);
            //p1 = p0-p1*2;
            //return (Math.Abs((circpos.X - p0.X) * (p1.Y - p0.Y) - (circpos.Y - p0.Y) * (p1.X - p1.Y))) / Vector2.Distance(p0,p1) <= radius;
            //return Math.Abs((p1.X-p0.X)*circpos.X + (p0.Y-p1.Y)*circpos.Y + (p0.X-p1.X)*p0.Y + p0.X*(p1.Y-p0.Y)) / Vector2.Distance(p0,p1) <= radius;
            /*
	        float dsq = Vector2.DistanceSquared(p0,p1);
            float d1sq = Vector2.DistanceSquared(p0,circpos);
            float radsq = radius*radius;

            if(d1sq < radsq || Vector2.DistanceSquared(p1,circpos) < radsq) return true;//If either point is in the circle, then it will always be true
            float d = (float)Math.Sqrt(dsq);
	        if(d < (float)Math.Sqrt(d1sq+radius)) return false;//If the distance between the two points is shorter than the distance to the circle, then it will always be false

            //Now some fancy geometry stuff to check if the line goes all the way through the circle
            //This is based on the rule that double the distance from a circle means half the angle from two opposite points on the edge of the circle
            float a = VectorMath.ToAngle(p0,p1)-VectorMath.ToAngle(p0,circpos);
	        return ((a < 0 ? -a : a) < _G.HalfPi*radius/d);*/
        }
        public static Vector2 GetRayIntersectionOnCircle(Vector2 p0,Vector2 p1,Vector2 circ,float radius){
            float lineangle = VectorMath.ToAngle(p0,p1);
            circ -= p0;

            circ = VectorMath.Rotate(circ,-lineangle);
            float y = -circ.Y/radius;

            Vector2 ans = circ+new Vector2(-radius*(float)Math.Sqrt(1-y*y),y*radius);
            ans = p0+VectorMath.Rotate(ans,lineangle);

            return ans;
            /*Unoptimized version:
            float lineangle = VectorMath.ToAngle(p0,p1);
            circ -= p0;

            circ = VectorMath.Rotate(circ,-lineangle);
            float y = -circ.Y;
            y = y/radius;

            float x = _G.Sqrt(1-y*y);//Using x^2+y^2 = r^2, where r = 1

            x *= radius;
            y *= radius;

            Vector2 ans = circ+new Vector2(-x,y);//negative x because it will be on the left side
            ans = p0+VectorMath.Rotate(ans,lineangle);

            return ans;
            */
        }

        public static bool PointInRectangle(Vector2 topleft,Vector2 botright,Vector2 point){
            return point.X > topleft.X && point.Y > topleft.Y && point.X < botright.X && point.Y < botright.Y;
        }
        public static bool PointInRectangle_FromCenter(Vector2 mid,Vector2 size,Vector2 point){
            return PointInRectangle(mid-size/2,mid+size/2,point);
        }
        static bool PointAboveLine(Vector2 p0,Vector2 p1,Vector2 point){
            float xdist = point.X-p0.X;
            return point.Y > p0.Y+xdist*(p1.Y-p0.Y)/(p1.X-p0.X);
        }
        public static bool RectangleOnLine(Vector2 p0,Vector2 p1,Vector2 rect0,Vector2 rect1){
            if(PointInRectangle(rect0,rect1,p0) || PointInRectangle(rect0,rect1,p1)) return true;

            if((p0.X < rect0.X && p1.X < rect0.X) || (p0.Y < rect0.Y && p1.Y < rect0.Y) || (p0.X > rect1.X && p1.X > rect1.X) || (p0.X > rect1.X && p1.X > rect1.X)) return false;

            bool a = PointAboveLine(p0,p1,rect0);
            bool b = PointAboveLine(p1,p1,rect1);
            if(a != b) return true;
            bool c = PointAboveLine(p0,p1,new Vector2(rect0.X,rect1.Y));
            bool d = PointAboveLine(p0,p1,new Vector2(rect1.X,rect0.Y));
            return (a != c) || (b != d) || (a != d) || (b != c);
        }
    }
}
