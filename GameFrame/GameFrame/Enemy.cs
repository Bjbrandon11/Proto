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
        Color c, cc;
        int speed;
        int hit;
        public Enemy()
        {
            health = 400;
            hBox = new Rectangle(500, 100, 30, 30);
            speed = 5;
            hit = 30;
            c = Color.Green; //Set the normal color
            cc = c; //Change the current color to green.
        }
        public Enemy(Vector2 loc)
        {
            health = 400;
            hBox = new Rectangle((int)loc.X, (int)loc.Y, 30, 30);
            speed = 5;
            hit = 30;
            c = Color.Green;
            cc = c;
        }
        public void LoadContent()
        {
            etxt = GameHolder.Game.Content.Load<Texture2D>("wsquare"); //Loads the enemy's sprite
        }
        public void Update()
        {
            //Makes so player knows if enemy was hit, otherwise color is unseeable to player
            if (hit < 3)
                hit++;
            else
                cc = c; //Resets current color

        }
        public Rectangle HitBox
        {
            get { return hBox; }
            set { hBox = value; }
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
            if (cc.Equals(c))
            {
                cc = Color.Red; //Set the current color to red, shows if was hit to player
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(etxt, hBox, cc); //Draw enemy's sprite onscreen
        }
    }
}
