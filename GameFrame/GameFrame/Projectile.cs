using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Projectile
    {
        Vector2 origin, dest, slope, realLoc;
        double ang, speed;
        public Projectile(Vector2 origin, Vector2 dest, double speed)//needs origin , destination, speed
        {
            this.dest = dest;
            this.origin = origin;
            this.speed = speed;
            realLoc = origin;
            calcAng();
        }
        public Projectile(Vector2 origin, double ang, double speed)//needs origin , destination, speed
        {
            this.origin = origin;
            this.speed = speed;
            realLoc = origin;
            this.ang = ang;
        }
        private void calcAng()//calculates the angle and slope
        {
            ang = (double)Math.Atan2(dest.Y - origin.Y, dest.X - origin.X);
            slope = new Vector2((float)Math.Cos(ang), (float)Math.Sin(ang));

        }
        public void Update()
        {
            realLoc.X += (float)(slope.X * speed);
            realLoc.Y += (float)(slope.Y * speed);
        }
        public Vector2 getReal() { return realLoc; }
        public double getSpeed() { return speed; }
    }
}
