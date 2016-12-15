using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Enemy
    {
        int health;
        Rectangle hBox;
        Texture2D etxt;
        Color c,cc;
        int speed;
        int hit;
        public Enemy()
        {
            health = 20;
            hBox = new Rectangle(500,100,30,30);
            speed = 5;
            hit = 30;
            c = Color.Green;
            cc = c;
        }
        public Enemy(Vector2 loc)
        {
            health = 400;
            hBox = new Rectangle((int)loc.X,(int)loc.Y, 30, 30);
            speed = 5;
            hit = 30;
            c = Color.Green;
            cc = c;
        }
        public void LoadContent()
        {
            etxt = GameHolder.Game.Content.Load<Texture2D>("wsquare");
        }
        public void Update()
        {
            if (hit < 3)
                hit++;
            else
                cc = c;

        }
        public Rectangle HitBox
        {
            get { return hBox; }
            set { hBox=value; }
        }
        public int Health
        { 
            get { return health; }
            set { health = value; }
        }
        public void Hit(int dam)
        {
            hit = 0;
            health -= dam;
            if(cc.Equals(c))
            {
                cc = Color.Red;

            }       
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(etxt, hBox, cc);
        }
    }
}
