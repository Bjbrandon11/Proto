using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Tiles
    {
        public Texture2D texture;

        public Rectangle Rectangle{ get;set; }
        
        public void Draw()
        {
            GameHolder.spriteBatch.Draw(texture,Rectangle,Color.White);
        }
    }
    class Collision : Tiles
    {
        public Collision(int i, Rectangle rec)
        {
            texture = GameHolder.Game.Content.Load<Texture2D>("wsquare");
            this.Rectangle = rec;
        }
    }
}
