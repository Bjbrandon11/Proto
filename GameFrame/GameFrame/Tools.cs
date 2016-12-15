using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Tools
    {
        public static double calcRad(Vector2 origin, Vector2 dest)
        {
            return (double)Math.Atan2(dest.Y - origin.Y, dest.X - origin.X);
        }
        public static double radToDeg(double rad)
        {
            return rad * (180 / Math.PI);
        }
        
    }
}
