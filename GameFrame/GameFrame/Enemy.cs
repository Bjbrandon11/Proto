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
        int wait;
        int waitTime;
        List<Bullet> bList;
        Random r;
        enum AttackState { roam,normalShoot,charge,threeShot,burstShot,circleShot,wait}
        AttackState attack;
        public Enemy()
        {
            health = 400;
            hBox = new Rectangle(500, 100, 30, 30);
            speed = 5;
            hit = 30;
            r = new Random();
            r.Next();
            wait = 0;
            waitTime = r.Next(120,360);
            c = Color.Green; //Set the normal color
            cc = c; //Change the current color to green.
            bList = new List<Bullet>();
            attack = AttackState.wait;
        }
        public Enemy(Vector2 loc)
        {
            health = 400;
            hBox = new Rectangle((int)loc.X, (int)loc.Y, 30, 30);
            speed = 5;
            hit = 30;
            r = new Random();
            r.Next();
            c = Color.Green;
            cc = c;
            wait = 0;
            waitTime = r.Next(120,360);
            bList = new List<Bullet>();
            attack = AttackState.wait;
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
            foreach (Bullet b in bList) //updates bullets
                b.Update();
            attackUpdate(); //updates the AI
        }
        public void attackUpdate()
        {
            switch(attack)
            {
                case AttackState.normalShoot:
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(GameHolder.Player.getHBox().Center.X, GameHolder.Player.getHBox().Center.Y), (int)Bullet.btype.normal));
                    attack = AttackState.wait;
                    break;
                case AttackState.wait:
                    wait++;
                    if(wait>waitTime)
                    {
                        wait = 0;
                        attack = (AttackState)r.Next(7);
                    }
                    break;
                default:
                    attack = AttackState.wait;
                    break;
            }
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
            foreach (Bullet b in bList)
                GameHolder.spriteBatch.Draw(etxt,b.getRec(),Color.Black);
        }
    }
}
