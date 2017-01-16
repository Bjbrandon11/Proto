using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    static class RectangleHelper
    {
        public static bool TouchTopOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom >= r2.Top - 1 && r1.Bottom <= r2.Top + (r2.Height / 2) && r1.Right >= r2.Left + r2.Width / 5 && r1.Left <= r2.Right - r2.Width / 5);
        }
    }
}
