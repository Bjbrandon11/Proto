using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class TileMap
    {
        private List<Collision> coll= new List<Collision>();
        int width, height;
        public List<Collision> collision { get { return coll; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public TileMap() { }

        public void Gen(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int num = map[y, x];
                    if (num > 0)
                        coll.Add(new Collision(num, new Rectangle(x * size, y * size, size, size)));
                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
        }
        public void Draw()
        {
            foreach (Collision tile in coll)
                tile.Draw();
        }
    }

}
