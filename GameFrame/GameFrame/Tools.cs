using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Tools
    {
        public static Random random = new Random();
        public static double calcRad(Vector2 origin, Vector2 dest)
        {
            //Console.WriteLine(Math.Atan2(dest.Y - origin.Y, dest.X - origin.X));
            return Math.Atan2(dest.Y - origin.Y, dest.X - origin.X);
        }
        public static double radToDeg(double rad)
        {
            return rad * 180 / Math.PI;
        }
        public static Vector2 rotateVec(Vector2 a, double ang)
        {
            double x = a.X * Math.Cos(ang) - a.Y * Math.Sin(ang);
            double y = a.X * Math.Sin(ang) + a.Y * Math.Cos(ang);
            return new Vector2((float)x, (float)y);
        }
    }
}
