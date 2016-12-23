using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Adventure
    {
        Player P1;
        List<Enemy> EL;
        String map;
        Random r;
        private float elapsedGameSec;
        //Randomizes enemy placement when 
        public Adventure(String map)
        {
            this.map = map;
            r = new Random();
            r.Next();
            P1 = new Player(); //Creates the player
            P1.setPMode(Player.pMode.Battle); //Sets the starting mode.
            EL = new List<Enemy>();
            for (int i = 0; i < 10; i++)
                EL.Add(new Enemy(new Vector2(r.Next(0, 500), r.Next(0, 500)))); //Random placement of enemies, may make this into method for entering rooms
            elapsedGameSec = 0f;
        }
        public void LoadContent()
        {
            P1.LoadContent();
            for (int i = 0; i < EL.Count; i++)
                EL[i].LoadContent();
        }
        public void Update(float gameTime)
        {
            elapsedGameSec = gameTime;
            P1.Update(elapsedGameSec);
            GameHolder.Player = P1;
            for (int i = 0; i < EL.Count; i++)
            {
                EL[i].Update(elapsedGameSec);
                for (int x = 0; x < P1.bulletList().Count; x++)
                    //If enemy is hit with bullet
                    if (EL[i].HitBox.Intersects(P1.bulletList()[x].getRec()))
                    {
                        EL[i].Hit(P1.bulletList()[x].getDam()); //Take away hp from enemy
                        //Removes bullet
                        P1.bulletList().RemoveAt(x);
                    }
                //Kill enemy
                if (EL[i].Health < 1)
                    EL.RemoveAt(i);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            P1.Draw(spriteBatch);
            for (int i = 0; i < EL.Count; i++)
                EL[i].Draw(spriteBatch);
        }
        public double getElapsedTime() { return elapsedGameSec; }
    }
}
