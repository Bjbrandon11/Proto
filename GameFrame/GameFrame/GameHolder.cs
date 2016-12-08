using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    public static class GameHolder
    {
        public static Game Game { get; set; }
        public static GameTime gameTime { get; set; }
        public static SpriteBatch spriteBatch { get; set; }
    }
}
