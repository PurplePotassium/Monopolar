using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Monopoles
{
    public static class Rand
    {
        public static Random random;

        static Rand(){
            Initialize();
        }

        public static void Initialize(){
            Rand.Reseed((int)DateTime.Now.Ticks);
            random = new Random();
        }

        public static int GetOldSeed(){
            return Rand.IntMax();
        }

        public static void Reseed(int seed){
            random = new Random(seed);
        }

        public static bool Bool(){
            return Float() < .5f;
        }
        
        public static bool Chance(float chance){
            return Float() < chance;
        }

        public static bool Chance(int numerator,int denominator){
            return Rand.Int(denominator) < numerator;
        }

        public static int Sign(){
            return Float(1) < .5f ? -1 : 1;
        }

        public static int Int(int max){
            if(max <= 0)
                return 0;
            return random.Next(max);
        }
        public static int Int(int min,int max){
            return random.Next(min,max);
        }
        public static int IntMax(){
            return Int(Int32.MinValue,Int32.MaxValue);
        }
        
        public static double Double(){
            return random.NextDouble();
        }
        public static double Double(double max){
            return random.NextDouble()*max;
        }
        public static double Double(float min,float max){
            return min+random.NextDouble()*(max-min);
        }
        public static float Float(){
            return (float)random.NextDouble();
        }
        public static float Float(float max){
            return (float)random.NextDouble()*max;
        }
        public static float Float(float min,float max){
            return min+(float)random.NextDouble()*(max-min);
        }
        public static float Float_Mid(float min,float mid,float max){
            return Rand.Bool() ? Rand.Float(min,mid) : Rand.Float(mid,max);
        }

        public static float Angle(){
            return Float()*_G.Tau;
        }
        
        public static Vector2 DirectionVector(){
            return VectorMath.FromAngle(Angle());
        }
        public static Vector2 DirectionVector(float mlower,float mupper){
            return DirectionVector(Rand.Float(mlower,mupper));
        }
        public static Vector2 DirectionVector(float magnitude){
            return DirectionVector()*magnitude;
        }

        public static Vector2 Vector2(float max){
            return new Vector2(Float(-max,max),Float(-max,max));
        }
        public static Vector2 Vector2(Vector2 max){
            return new Vector2(Float(max.X),Float(max.Y));
        }
        
        public static int MutateInt(int num,int lowerbound,int upperbound,int delta,int iterations){
            int n = num;
            for(int i = 0; i < iterations; i++){
                n = n+Int(-delta,delta)*Sign();
            }

            n = _G.BindInt(n,lowerbound,upperbound);
            return n;
        }

        public static float MutateFloat(float num,float lowerbound,float upperbound,float delta,int iterations){
            for(int i = 0; i < iterations; i++){
                num = num+Float(delta)*Sign();
            }

            return _G.BindFloat(num,lowerbound,upperbound);
        }
        
        public static T Choose<T>(T[] obj){
            return obj[Rand.Int(obj.Length)];
        }
        public static T Choose<T>(T[] obj1,T[] obj2){
            return Rand.Int(obj1.Length+obj2.Length) < obj1.Length ? Choose<T>(obj1) : Choose<T>(obj2);
        }

        public static Color Color(){
            return new Color(Float(),Float(),Float());
        }
        public static Color Color(float totalbrightness){
            float r = Rand.Float(),g = Rand.Float(),b = Rand.Float();
            float max = totalbrightness*3/Math.Max(Math.Max(r,g),b);
            return new Color(r*max,g*max,b*max);
        }
    }
}