using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    public class Bullet
    {

        double speed;

        Projectile bullet;
        public enum btype { normal, sniper, strong }
        public btype bt;
        Rectangle hBox;
        Texture2D btxt;

        //if no type is given, make a basic bullet
        public Bullet(int x, int y, double ang)
        {
            bt = btype.normal;
            hBox = new Rectangle(x, y, 20, 20);
            speed = 10;
            bullet = new Projectile(new Vector2(x, y), ang, speed);

        }
        public Bullet(Vector2 origin, Vector2 dest, int type)
        {
            bt = (btype)type;
            hBox = new Rectangle((int)origin.X - 10, (int)origin.Y - 10, 20, 20);
            if (bt == btype.strong) speed = 5;
            else if (bt == btype.normal) speed = 8;
            else if (bt == btype.sniper) speed = 25;
            bullet = new Projectile(origin, dest, speed);
        }
        public Bullet(int x, int y, int ang, int type)
        {
            int side;
            bt = (btype)type;
            switch (bt)
            {
                default:
                    {
                        side = 20;
                        break;
                    }

            }
            hBox = new Rectangle(x - side / 2, y - side / 2, side, side);

        }
        public void Update()
        {
            bullet.Update();
            hBox.X = (int)bullet.getReal().X;
            hBox.Y = (int)bullet.getReal().Y;


        }
        public double getX() { return bullet.getReal().X; }
        public double getY() { return bullet.getReal().Y; }
        public Rectangle getRec() { return hBox; }
    }
}
