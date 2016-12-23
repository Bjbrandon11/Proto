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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Animation animation;
        Rectangle hBox; //Player HitBox
        KeyboardState oldKb;
        MouseState oldMouse;
        Texture2D ptxt;
        Texture2D blt;
        List<Bullet> bList;

        private int look;
        private int oldLook;
        private int sWidth, sHeight;//screen lengths
        private int mspeed;     //movement speed
        private int shRate;
        private int tmpRa;
        private double rX, rY;   //real locations (decimals)
        public enum pMode {Explore,Battle};

        private pMode mode;
        //public Vector2 playerDir;
        //public Vector2 playerFric;
        public Player()
        {
            mode = pMode.Explore;
            hBox = new Rectangle();
            mspeed = 6;//our movement speed
            shRate = 25; //our shooting rate
            animation = new Animation("proto_run_4", 50f, 10, true);
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
            tmpRa = shRate;//The rate we actually modify when we check for alt-fire rate.
            look = -1;
            oldLook = -1;
            //playerDir = new Vector2(0,0);
            //playerFric = playerDir;
        }
        public void LoadContent()
        {
            ptxt = GameHolder.Game.Content.Load<Texture2D>("wsquare");
            blt = GameHolder.Game.Content.Load<Texture2D>("wbullet");
        }
       public void UnloadContent()
        {

        }

        public void Update(float gameTime)    //will run everytime game Updates
        {
            KeyboardState kb = Keyboard.GetState(); //gets the state of keyboard , what keys are pressed , ect.
            MouseState mouse = Mouse.GetState();    // gets state of mouse , locations , if pressed ect.
            movementManager(kb);
            lookingManager(mouse);
            if (mode==pMode.Battle)
            {
                shootManager(mouse);
                // Console.WriteLine(looking(new Vector2(mouse.X,mouse.Y)));
                oldLook = look;
                oldMouse = mouse;
                oldKb = kb;
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
        //This method emulates static friction.
        public void staticFric()
        {
            
        }
        //This method emulates KINETIC friction.
        public void lookingManager(MouseState mouse)
        {
            look = looking(new Vector2(mouse.X, mouse.Y));
            if (look != oldLook)
            {
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
            //if left mouse is held, it creates a stream of bullet aiming to your mouse
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y), (int)Bullet.btype.normal));//bullet object takes in , Origin and Destination, in a vector
                                                                                                                                          // a vector2 is just coordinates
            }

            //if right mouse is held, it creates A STRONG BULLET
            if (mouse.RightButton == ButtonState.Pressed && altShootTimer())
            {
                bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y), (int)Bullet.btype.strong));//bullet object takes in , Origin and Destination, in a vector
            }
            /*
            //Adds in a semi-auto function to bullets, creates A SNIPER BULLET
            if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton != ButtonState.Pressed)
            {
                bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y), (int)Bullet.btype.sniper));//bullet object takes in , Origin and Destination, in a vector
            }
            */
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
            if (oldMouse.RightButton == ButtonState.Pressed && tmpRa == 0)//Has right mouse been pressed, and it is time?
            {
                tmpRa = shRate;
                return true;
            }
            else if (mouse.RightButton == ButtonState.Released)
            {
                tmpRa = shRate;
                return false;
            }
            else if (oldMouse.RightButton == ButtonState.Pressed && tmpRa > 0) //Is it too soon to shoot? Then reduce time and return false
            {
                tmpRa--;
                return false;
            }
            else return false;
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
            for (int i = 0; i < bList.Count(); i++)
            {
                if (bList[i].bt == Bullet.btype.strong) spriteBatch.Draw(blt, bList[i].getRec(), Color.Orange);
                else spriteBatch.Draw(blt, bList[i].getRec(), Color.Black);//draws bullet
            }
            spriteBatch.Draw(ptxt, hBox, Color.Transparent);//draws player
            animation.Draw();
        }
        public void setPMode(pMode m) { mode = m; }
        public List<Bullet> bulletList() { return bList; }
        public Rectangle getHBox() { return hBox; }
    }
}
