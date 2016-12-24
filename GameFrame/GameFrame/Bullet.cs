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

        private double speed;
        private int damage;
        Projectile bullet;
        
        public enum btype { normal, spread, blast,stream,sniper }
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
            damage = 1;
        }
        public Bullet(Vector2 origin, Vector2 dest, btype type)
        {
            bt = type;
            hBox = new Rectangle((int)origin.X - 10, (int)origin.Y - 10, 20, 20);
            initializeVal();
            bullet = new Projectile(origin, dest, speed);
        }
        public Bullet(int x, int y, double ang, btype type)
        {
            bt = type;
            hBox = new Rectangle(x, y, 20, 20);
            initializeVal();//Initialize bullet values
            bullet = new Projectile(new Vector2(x, y), ang, speed);
            Console.WriteLine("Speed: "+speed+"   Damage:"+damage);
        }
        public void initializeVal()
        {
            //Normal,Spread,Blast, Sniper
            if (bt == btype.normal)
            {
                speed = 8;
                damage = 10;
            }
            else if (bt == btype.spread)
            {
                speed = 6;
                damage = 8;
            }
            else if (bt == btype.blast)
            {
                speed = 4;
                damage = 25;
            }
            else if (bt == btype.stream)
            {
                speed = 8;
                damage = 5;
            }
            else if (bt == btype.sniper)
            {
                speed = 20;
                damage = 15;
            }
        }
        public void Update()
        {
            bullet.Update();
            hBox.X = (int)bullet.getReal().X;
            hBox.Y = (int)bullet.getReal().Y;
        }
        public double getX() { return bullet.getReal().X; }
        public double getY() { return bullet.getReal().Y; }
        public int getDam() { return damage; }
        public Rectangle getRec() { return hBox; }

        internal int getDam()
        {
            return 10;
        }
    }
}
