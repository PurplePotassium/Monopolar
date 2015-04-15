using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monopoles {
    public static class _G{
        public static float Pi = (float)Math.PI;
        public static float HalfPi = Pi/2;
        public static float Tau = Pi*2;

        public static Vector2 ScreenSize;
        public static Vector2 WorldSize;

        /////
        

        public static float BindFloat(float num,float lower,float upper){
            return num > upper ? upper : num < lower ? lower : num;
        }
        public static int BindInt(int num,int lower,int upper){
            return num > upper ? upper : num < lower ? lower : num;
        }
        public static Vector2 BindVector2(Vector2 num,Vector2 lower,Vector2 upper){
            return new Vector2(BindFloat(num.X,lower.X,upper.X),BindFloat(num.Y,lower.Y,upper.Y));
        }
        
        public static float PositiveMod(float a,float b){
            a = a%b;
            return a < 0 ? a+b : a;
        }
        public static int PositiveMod(int a,int b){
            a = a%b;
            return a < 0 ? a+b : a;
        }

        public static float NormalAngle(float a){
            a = a%Tau;
            return a < 0 ? a+Tau : a;
        }

        public static float AngleDistance(float r0,float r1){
            r0 = NormalAngle(r0);
            r1 = NormalAngle(r1);
            float dist = Math.Abs(r0-r1);
            return dist > _G.Pi ? _G.Tau-dist : dist;
        }

        public static float LerpAngle(float r0,float r1,float lerp){
            r0 = NormalAngle(r0);
            r1 = NormalAngle(r1);
            if(r0 < r1 && r1-r0 > Pi)
                return NormalAngle((r0+Tau)*(1-lerp)+r1*lerp);
            else if(r0 > r1 && r0-r1 > Pi)
                return NormalAngle(r0*(1-lerp)+(r1+Tau)*lerp);
            return r0*(1-lerp)+r1*lerp;
        }
    }
}
