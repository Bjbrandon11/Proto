﻿using System;
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

        Rectangle hBox; //Player HitBox
        KeyboardState oldKb;
        MouseState oldMouse;
        Texture2D ptxt;
        Texture2D blt;

        List<Bullet> bList;

        int sWidth, sHeight;//screen lengths
        int mspeed;     //movement speed
        double rX,rY;   //real locations (decimals)
        public enum pMode {Explore,Battle};

        pMode mode;
        public Player()
        {
            mode = pMode.Explore;
            hBox = new Rectangle();
            mspeed = 6;
            oldKb = new KeyboardState();
            oldMouse = new MouseState();
            bList = new List<Bullet>();
            sWidth = GameHolder.Game.Window.ClientBounds.Width;
            sHeight = GameHolder.Game.Window.ClientBounds.Height;
            hBox = new Rectangle(sWidth / 2 - 12, sHeight / 2 - 12, 24, 24);   //set the location and lengths of hitbox
            rX = hBox.X;
            rY = hBox.Y;

        }
        public void LoadContent()
        {
            ptxt = GameHolder.Game.Content.Load<Texture2D>("wsquare");
        }
       public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)    //will run everytime game Updates
        {
            KeyboardState kb = Keyboard.GetState(); //gets the state of keyboard , what keys are pressed , ect.
            MouseState mouse = Mouse.GetState();    // gets state of mouse , locations , if pressed ect.
            if (mode==pMode.Battle)
            {
                ///<summary>
                ///changes direction based off button press
                ///1=up
                ///2=up right
                ///3= right
                ///4=down right
                ///5=down
                ///6=down left
                ///7=left
                ///8=up left
                /// </summary>
                if (kb.IsKeyDown(Keys.W) && hBox.Y > 0 && (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D)))
                    movement(1);
                if (kb.IsKeyDown(Keys.S) && hBox.Y + hBox.Height < sHeight && (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D)))
                    movement(5);
                if (kb.IsKeyDown(Keys.A) && hBox.X > 0 && (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S)))
                    movement(7);
                if (kb.IsKeyDown(Keys.D) && hBox.X + hBox.Width < sWidth && (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.S)))
                    movement(3);
                if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.A))
                    movement(8);
                if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.A))
                    movement(6);
                if (kb.IsKeyDown(Keys.W) && kb.IsKeyDown(Keys.D))
                    movement(2);
                if (kb.IsKeyDown(Keys.S) && kb.IsKeyDown(Keys.D))
                    movement(4);
                hBox.X = (int)rX;//sets the hitBox to the real location 
                hBox.Y = (int)rY;
                if (mouse.LeftButton == ButtonState.Pressed)//if left mouse is clicked , it creates a bullet aiming to your mouse
                {
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y)));//bullet object takes in , Origin and Destination, in a vector
                    // a vector2 is just coordinates
                }
                if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton != ButtonState.Pressed)//if left mouse is clicked , it creates a bullet aiming to your mouse
                {
                    bList.Add(new Bullet(new Vector2(hBox.Center.X, hBox.Center.Y), new Vector2(mouse.X, mouse.Y)));//bullet object takes in , Origin and Destination, in a vector
                    // a vector2 is just coordinates
                }
                for (int i = 0; i < bList.Count; i++)//updates the bullets created
                {
                    bList[i].Update();
                    if (bList[i].getX() > sWidth || bList[i].getX() + bList[i].getRec().Width < 0 || bList[i].getY() > sHeight || bList[i].getY() + bList[i].getRec().Height < 0)//deletes all bullets that go off screen
                        bList.RemoveAt(i);
                }
                oldMouse = mouse;
                oldKb = kb;
            }
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
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bList.Count(); i++)
                spriteBatch.Draw(ptxt, bList[i].getRec(), Color.Black);//draws bullet
            spriteBatch.Draw(ptxt, hBox, Color.Red);//draws player
        }
        public void setPMode(pMode m) { mode = m; }
        public List<Bullet> getList() { return bList; }
    }
}