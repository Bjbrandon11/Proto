using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class DBox
    {
        Rectangle box;
        int prog;
        string text;
        string ctxt;
        int fpc;//frames per character
        public DBox(string text)
        {
            fpc = 10;
            this.text = text;
            box = new Rectangle(50,500,500,300);
        }
        public void nextLett()
        {
            ctxt = text.Substring(0, 1+(prog%fpc));
            prog++;

        }
        public void Draw()
        {
            //draw box
            //draw ctxt
        }
    }
}
