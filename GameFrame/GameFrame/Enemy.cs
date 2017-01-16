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
        private Rectangle hBox;
        private Texture2D etxt;
        private Color c, cc;

        private float hit;
        private float wait;
        private float elapsedGameTime;
        private int health;
        private int speed;
        private int roamTime;
        private int roam;
        private int waitTime;
        List<Bullet> bList;
        Random r;
        enum AttackState { roam,normalShoot,charge,threeShot,burstShot,circleShot,plus,diag,wait}
        AttackState attack;
        public Enemy()
        {
            inst();
        }
        public Enemy(Vector2 loc)
        {
            inst();
            hBox = new Rectangle((int)loc.X, (int)loc.Y, 30, 30);
        }
        public void inst()
        {
            elapsedGameTime = 0;
            health = 400;
            hBox = new Rectangle(500, 100, 30, 30);
            speed = 5;
            hit = 30;
            wait = 0;
            waitTime = Tools.random.Next(1, 4);//Between 1 and 4 seconds
            c = Color.Green; //Set the normal color
            cc = c; //Change the current color to green.
            bList = new List<Bullet>();
            attack = AttackState.wait;
        }
        public void LoadContent()
        {
            etxt = GameHolder.Game.Content.Load<Texture2D>("wsquare"); //Loads the enemy's sprite
        }
        public void Update(float gameTime)
        {
            elapsedGameTime = gameTime;
            //Makes so player knows if enemy was hit, otherwise color is unseeable to player
            if (hit < .5)
                hit+= elapsedGameTime;
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
                    wait+=elapsedGameTime;
                    if(wait>waitTime)
                    {
                        wait = 0;
                        attack = (AttackState)Tools.random.Next(8);
                    }
                    break;
                case AttackState.threeShot:
                    bList.Add(new Bullet(hBox.Center.X, hBox.Center.Y, Tools.calcRad(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(GameHolder.Player.getHBox().Center.X-5, GameHolder.Player.getHBox().Center.Y-5))));
                    bList.Add(new Bullet(hBox.Center.X, hBox.Center.Y, Tools.calcRad(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(GameHolder.Player.getHBox().Center.X, GameHolder.Player.getHBox().Center.Y))));
                    bList.Add(new Bullet(hBox.Center.X, hBox.Center.Y, Tools.calcRad(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(GameHolder.Player.getHBox().Center.X+5, GameHolder.Player.getHBox().Center.Y+5))));
                    attack = AttackState.wait;
                    break;
                case AttackState.roam:
                    attack = AttackState.wait;
                    break;
                case AttackState.plus:
                    bList.Add(new Bullet(new Vector2(hBox.Center.X,hBox.Center.Y), new Vector2(hBox.Center.X, hBox.Center.Y+1),Bullet.btype.normal));
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X, hBox.Center.Y - 1), Bullet.btype.normal));
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X+1, hBox.Center.Y), Bullet.btype.normal));
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X-1, hBox.Center.Y), Bullet.btype.normal));
                    attack = AttackState.wait;
                    break;
                case AttackState.diag:
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X+1, hBox.Center.Y + 1), Bullet.btype.normal));
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X-1, hBox.Center.Y + 1), Bullet.btype.normal));
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X + 1, hBox.Center.Y-1), Bullet.btype.normal));
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(hBox.Center.X - 1, hBox.Center.Y-1), Bullet.btype.normal));
                    attack = AttackState.wait;
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
