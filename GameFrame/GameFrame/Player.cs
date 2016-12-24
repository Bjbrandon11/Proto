using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameFrame
{
    
    public class Player
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Animation animation;
        private Rectangle hBox; //Player HitBox
        private KeyboardState oldKb;
        private MouseState oldMouse;
        private Texture2D ptxt;
        private Texture2D blt;
        private Texture2D hthMeterTxt;
        private Rectangle hthMeter;
        private Rectangle hthMeterSrc;
        private Rectangle dispSecShot;
        public List<Bullet> bList;

        private static int health, maxHealth;
        private int look, oldLook;
        private int sWidth, sHeight;//screen lengths
        private int mspeed;     //movement speed
        //Dimensions of health meter
        private float hthMeter_SX;
        private int hthMeter_SY;
        private float hthMeter_FF;//FF stands for filled factor, what percentage of health is there?
        //a set rate of shooting
        private float shRate;
        private float periodSecShot;
        private float elapsedGameSec;
        private double rX, rY;   //real locations (decimals)
        public enum pMode {Explore,Battle, Dead};
        public enum fireMode {Normal,Spread,Blast,Stream, Sniper};

        private pMode mode;
        private fireMode fMode;
        //public Vector2 playerDir;
        //public Vector2 playerFric;
        public Player()
        {
            maxHealth = 100;
            health = maxHealth;
            hthMeter_FF = 1f;
            hthMeter_SX = 200f;
            hthMeter_SY = 20;
            mspeed = 6;//our movement speed
            shRate = .5f; //our shooting rate
            periodSecShot = 0f;
            elapsedGameSec = 0f;
            mode = pMode.Explore;
            fMode = fireMode.Normal;
            hthMeter = new Rectangle(10,10, (int)(hthMeter_SX * hthMeter_FF), hthMeter_SY);
            hthMeterSrc = hthMeter;
            animation = new Animation("proto_run_4", 50f, 10, true);
            Head = GameHolder.Game.Content.Load<Texture2D>("Proto_Head_1_5");
            Body = GameHolder.Game.Content.Load<Texture2D>("Proto_Body_1_5");
            oldKb = new KeyboardState();
            oldMouse = new MouseState();
            bList = new List<Bullet>();
            sWidth = GameHolder.Game.Window.ClientBounds.Width;
            sHeight = GameHolder.Game.Window.ClientBounds.Height;
            //set the start coordinates and dimensions of hitbox
            //!!NOTE TO SELF: We need to make a new method that alters and returns the player coordinates!!
            hBox = new Rectangle(sWidth / 2 - 12, sHeight / 2 - 12, 24, 24); //(Width,height,x-coord,y-coord)  
            rX = hBox.X; //our hitbox's x-coordinate
            rY = hBox.Y; //our hitbox's y-coordinate
            look = -1;
            oldLook = -1;
            //playerDir = new Vector2(0,0);
            //playerFric = playerDir;
        }
        public void LoadContent()
        {
            ptxt = GameHolder.Game.Content.Load<Texture2D>("wsquare");
            blt = GameHolder.Game.Content.Load<Texture2D>("wbullet");
            hthMeterTxt = GameHolder.Game.Content.Load<Texture2D>("hthMeter");
        }
       public void UnloadContent()
        {

        }

        public void Update(float gameTime)    //will run everytime game Updates
        {
            if (mode != pMode.Dead)
            {
                elapsedGameSec = gameTime;
                hthMeter_FF = (float)health / maxHealth; //sets amount of health to fill
                KeyboardState kb = Keyboard.GetState(); //gets the state of keyboard , what keys are pressed , ect.
                MouseState mouse = Mouse.GetState();    // gets state of mouse , locations , if pressed ect.
                movementManager(kb);
                lookingManager(mouse);
                if (mode == pMode.Battle)
                {
                    shootManager(mouse);
                    oldLook = look;
                    oldMouse = mouse;
                    oldKb = kb;
                }
                deathChecker();
            }
            else if (mode == pMode.Dead)
            {
                Console.WriteLine("U R DED");
                animation.setVect(new Vector2(hBox.Center.X, hBox.Center.Y));
                animation.PlayAnim();
            }
        }
        public void movementManager(KeyboardState kb)
        {
            /*------------------------*
             *--------MOVEMENT--------*
             *------------------------*
             *------------------------*
             *changes direction based-*
             *on button pressess.-----*
             *--1=up        ----------*
             *--2=up right  ----------*
             *--3= right    ----------*
             *--4=down right----------*
             *--5=down      ----------*  
             *--6=down left ----------*
             *--7=left      ----------*
             *--8=up left   ----------*
             *------------------------*/
            if (kb.IsKeyDown(Keys.W) && hBox.Y > 0 && (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D)))
                movement(1);
            if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.D) && hBox.Y > 0 && hBox.X + hBox.Width < sWidth)
                movement(2);
            if (kb.IsKeyDown(Keys.D) && hBox.X + hBox.Width < sWidth && (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S)))
                movement(3);
            if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.D) && hBox.Y + hBox.Height < sHeight && hBox.X + hBox.Width < sWidth)
                movement(4);
            if (kb.IsKeyDown(Keys.S) && hBox.Y + hBox.Height < sHeight && (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D)))
                movement(5);
            if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.A) && hBox.Y + hBox.Height < sHeight && hBox.X > 0)
                movement(6);
            if (kb.IsKeyDown(Keys.A) && hBox.X > 0 && (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S)))
                movement(7);
            if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.A) && hBox.Y > 0 && hBox.X > 0)
                movement(8);

            /*---------------------------------------------------------------------------------------------------------*
             * Checks if we hit wall when we are going diagonally, then we make the player go in appropriate direction.*
             *------------------------------PREVENTS THE "STICKING" BUG------------------------------------------------*
             *---------------------------------------------------------------------------------------------------------*/
            //GOES UP: Up and left, on left border
            if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.A) && hBox.Y > 0 && hBox.X <= 0)
                movement(1);
            //GOES UP: Up and right, on right border
            if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.D) && hBox.Y > 0 && hBox.X + hBox.Width >= sWidth)
                movement(1);
            //GOES RIGHT: Up and right, on top border
            if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.D) && hBox.Y <= 0 && hBox.X + hBox.Width < sWidth)
                movement(3);
            //GOES RIGHT: Down and right, on bottom border
            if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.D) && hBox.Y + hBox.Height >= sHeight && hBox.X + hBox.Width < sWidth)
                movement(3);
            //GOES DOWN: Down and left, on left border
            if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.A) && hBox.Y + hBox.Height < sHeight && hBox.X <= 0)
                movement(5);
            //GOES DOWN: Down and right, on right border
            if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.D) && hBox.Y + hBox.Height < sHeight && hBox.X + hBox.Width >= sWidth)
                movement(5);
            //GOES LEFT: Up and left, on top border
            if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.A) && hBox.Y <= 0 && hBox.X > 0)
                movement(7);
            //GOES LEFT: Down and left, on bottom border
            if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.A) && hBox.Y + hBox.Height >= sHeight && hBox.X > 0)
                movement(7);

            //IF PLAYER IS NOT MOVING, DON'T MAKE THE SPRITE ANIMATE
            if (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.S) && kb.IsKeyUp(Keys.D))
                animation.setLoop(false);

            //Normal,Spread,Blast,Stream, Sniper
            if (kb.IsKeyDown(Keys.D1)) fMode = fireMode.Normal;
            else if (kb.IsKeyDown(Keys.D2)) fMode = fireMode.Spread;
            else if (kb.IsKeyDown(Keys.D3)) fMode = fireMode.Blast;
            else if (kb.IsKeyDown(Keys.D4)) fMode = fireMode.Stream;
            else if (kb.IsKeyDown(Keys.D5)) fMode = fireMode.Sniper;

            //sets the hitBoxes to the real location 
            //playerDir = new Vector2((float)rX-hBox.X, (float)rY-hBox.Y); //And update the player's direction
            //playerFric = -playerDir;
            //Console.WriteLine(playerDir);
            hBox.X = (int)rX;
            hBox.Y = (int)rY;
            
        }
        public void movement(int d)
        {
            switch(d)
            {
                case 1:
                    rY += -1 * mspeed;
                    break;
                case 2:
                    rX += .707 * mspeed;
                    rY += -.707 * mspeed;
                    break;
                case 3:
                    rX += 1 * mspeed;
                    break;
                case 4:
                    rX += .707 * mspeed;
                    rY += .707 * mspeed;
                    break;
                case 5:
                    rY += 1 * mspeed;
                    break;
                case 6:
                    rX += -.707 * mspeed;
                    rY += .707 * mspeed;
                    break;
                case 7:
                    rX += -1 * mspeed;
                    break;
                case 8:
                    rX += -.707 * mspeed;
                    rY += -.707 * mspeed;
                    break;
                default:

                    break;
            }
            animation.setLoop(true);
        }
        public void lookingManager(MouseState mouse)
        {
            look = looking(new Vector2(mouse.X, mouse.Y));
            if (look != oldLook)
            {
                /* switch (looking(new Vector2(mouse.X, mouse.Y)))
                 {
                     case 0:
                         animation = new Animation("proto_run_4", 50f, 10, false, SpriteEffects.FlipHorizontally);
                         break;
                     case 1:
                         animation = new Animation("proto_run_2", 50f, 10, false);
                         break;
                     case 2:
                         animation = new Animation("proto_run_2", 50f, 10, false);
                         break;
                     case 3:
                         animation = new Animation("proto_run_2", 50f, 10, false);
                         break;
                     case 4:
                         animation = new Animation("proto_run_4", 50f, 10, false);
                         break;
                     case 5:
                         animation = new Animation("proto_run_6", 50f, 10, false);
                         break;
                     case 6:
                         animation = new Animation("proto_run_6", 50f, 10, false);
                         break;
                     case 7:
                         animation = new Animation("proto_run_6", 50f, 10, false);
                         break;
                     default:
                         animation = new Animation("proto_run_6", 50f, 10, false);
                         break;
                 }*/
                switch (looking(new Vector2(mouse.X, mouse.Y)))
                {
                    case 0:
                        animation = new Animation("proto_run_4", 50f, 10, false, SpriteEffects.FlipHorizontally);
                        break;
                    case 1:
                        animation = new Animation("proto_run_2", 50f, 10, false);
                        break;
                    case 2:
                        animation = new Animation("proto_run_2", 50f, 10, false);
                        break;
                    case 3:
                        animation = new Animation("proto_run_2", 50f, 10, false);
                        break;
                    case 4:
                        animation = new Animation("proto_run_4", 50f, 10, false);
                        break;
                    case 5:
                        animation = new Animation("proto_run_6", 50f, 10, false);
                        break;
                    case 6:
                        animation = new Animation("proto_run_6", 50f, 10, false);
                        break;
                    case 7:
                        animation = new Animation("proto_run_6", 50f, 10, false);
                        break;
                    default:
                        animation = new Animation("proto_run_6", 50f, 10, false);
                        break;
                }
            }
            animation.setVect(new Vector2(hBox.Center.X, hBox.Center.Y));
            animation.PlayAnim();
        }
        public void shootManager(MouseState mouse)
        {
            /*----------------*
             *----SHOOTING----*
             *----------------*/
            //PRIMARY FIRE ONLY
            //Normal,Spread,Blast,Stream, Sniper
            //if left mouse is held, it creates a stream of bullet aiming to your mouse

            //Need to create vector for mouse.
            Vector2 mouseVec = new Vector2(mouse.X, mouse.Y);
            if (fMode == fireMode.Normal)
            {
                if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed)
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), mouseVec, Bullet.btype.normal));//bullet object takes in , Origin and Destination, in a vector
            }
            else if (fMode == fireMode.Spread)
            {
                shRate = .5f;
                if (mouse.LeftButton == ButtonState.Pressed && altShootTimer())
                {
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), Tools.rotateVec(mouseVec,50), Bullet.btype.spread));//left
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), mouseVec, Bullet.btype.spread));//center
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), Tools.rotateVec(mouseVec, -50), Bullet.btype.spread));//right
                }
            }
            else if (fMode == fireMode.Blast)
            {
                shRate = .4f;
                if (mouse.LeftButton == ButtonState.Pressed && altShootTimer())
                {
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y), Bullet.btype.blast));//bullet object takes in , Origin and Destination, in a vector
                }
            }
            else if (fMode == fireMode.Stream)
            {
                shRate = .1f;
                if (mouse.LeftButton == ButtonState.Pressed && altShootTimer() && bList.Count < 25)
                {
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y), Bullet.btype.normal));//bullet object takes in , Origin and Destination, in a vector
                }
            }
            else if (fMode == fireMode.Sniper)
            {
                shRate = .5f;
                //Adds in a semi-auto function to bullets, creates A SNIPER BULLET
                if (mouse.LeftButton == ButtonState.Pressed && altShootTimer())
                {
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y), Bullet.btype.sniper));//bullet object takes in , Origin and Destination, in a vector
                }
            }

            for (int i = 0; i < bList.Count; i++)//updates the bullets created
            {
                bList[i].Update();
                if (bList[i].getX() > sWidth || bList[i].getX() + bList[i].getRec().Width < 0 || bList[i].getY() > sHeight || bList[i].getY() + bList[i].getRec().Height < 0)//deletes all bullets that go off screen
                    bList.RemoveAt(i);
            }
        }
        //Creates the slow auto functionality to alt fire
        public bool altShootTimer()
        {
            MouseState mouse = Mouse.GetState();    // gets state of mouse , locations , if pressed ect.
            if (oldMouse.LeftButton == ButtonState.Pressed && periodSecShot >= shRate)//Has right mouse been pressed, and it is time?
            {
                periodSecShot = 0f;
                return true;
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                periodSecShot = 0f;
                return false;
            }
            else if (oldMouse.LeftButton == ButtonState.Pressed && periodSecShot < shRate) //Is it too soon to shoot? Then reduce time and return false
            {
                periodSecShot += elapsedGameSec;
                return false;
            }
            else return false;
        }
        public bool hitManager()
        {
            for(int i = 0; i < bList.Count; i++)//updates the bullets created
            {
                bList[i].Update();
                if (bList[i].getX() > sWidth || bList[i].getX() + bList[i].getRec().Width < 0 || bList[i].getY() > sHeight || bList[i].getY() + bList[i].getRec().Height < 0)//deletes all bullets that go off screen
                    bList.RemoveAt(i);
            }
            return false;
        }
        public int looking(Vector2 dest)
        {
            int ang = ((int)((Tools.radToDeg(Tools.calcRad(new Vector2((float)rX, (float)rY), dest)))));

            if (ang <= -112.5 && ang >= -157.5)
                return 1;
            if (ang >= -112.5 && ang <= -67.5)
                return 2;
            if (ang >= -67.5 && ang <= -22.5)
                return 3;
            if (ang >= -22.5 && ang <= 22.5)
                return 4;
            if (ang >= 22.5 && ang <= 67.5)
                return 5;
            if (ang >= 67.5 && ang <= 112.5)
                return 6;
            if (ang >= 112.5 && ang <= 157.5)
                return 7;
            if (ang >= 157.5  || ang <= -157.5)
                return 0;

            return -1;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            //Updates bullet
            for (int i = 0; i < bList.Count(); i++)
            {
                if (bList[i].bt == Bullet.btype.blast) spriteBatch.Draw(blt, bList[i].getRec(), Color.Orange);
                else spriteBatch.Draw(blt, bList[i].getRec(), Color.Black);//draws bullet
            }
            //Updates player
            if (health > 0) spriteBatch.Draw(ptxt, hBox, Color.Transparent);//draws player
            else spriteBatch.Draw(ptxt, hBox, Color.Transparent);
            animation.Draw();

            //Updates Hud Above all
            hthMeter = new Rectangle(10, 10, (int)(hthMeter_SX * hthMeter_FF), hthMeter_SY);
            hthMeterSrc = new Rectangle(0, 0, (int)(hthMeter_SX * hthMeter_FF), hthMeter_SY);
            spriteBatch.Draw(hthMeterTxt, hthMeter, hthMeterSrc,Color.ForestGreen,0,new Vector2(0,0), SpriteEffects.None,1f);
        }
        public void hit(int point)
        {
            if(health-point>=0)health -= point;
            else if (health < 0) health = 0;//sees if negative health, then corrects it.
        }
        public void heal(int point)
        {
            if (health + point < maxHealth) health += point;
            else if (health + point >= maxHealth) health = maxHealth;
        }
        public static int retHealth()
        {
            return health;
        }
        public void deathChecker()
        {
            if (health == 0)
            {
                mode = pMode.Dead;
                animation = new Animation("lnkDead", 50f, 3, true);
            }
        }
        public void setPMode(pMode m) { mode = m; }
        public pMode getPMode() { return mode; }
        public Rectangle getHBox() { return hBox; }
        public List<Bullet> bulletList() { return bList; }
    }
}
