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
        public Adventure(String map)
        {
            this.map = map;
            r = new Random();
            r.Next();
            P1 = new Player();
            P1.setPMode(Player.pMode.Battle);
            EL = new List<Enemy>();
            for (int i = 0; i < 5; i++)
                EL.Add(new Enemy(new Vector2(r.Next(0,500), r.Next(0, 500))));
        }
        public void LoadContent()
        {
            P1.LoadContent();
            for (int i = 0; i < EL.Count; i++)
                EL[i].LoadContent();
        }
        public void Update(GameTime gameTime)
        {
            P1.Update(gameTime);
            
            for (int i = 0; i < EL.Count; i++)
            {
                EL[i].Update();
                for (int x = 0; x < P1.getList().Count; x++)
                    if (EL[i].HitBox.Intersects(P1.getList()[x].getRec()))
                    {
                        EL[i].Hit(1);
                        P1.getList().RemoveAt(x);
                    }
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
    }
}
