using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFrame
{
    class Animation
    {
        Texture2D animation;
        Rectangle sourceRect;
        Vector2 pos;

        float elapsed;
        float frameTime;
        int frames;
        int cFrame;
        int width;
        int height;
        int frameWidth;
        int frameHeight;
        bool looping;
        public Animation(string asset, float frameSpeed,int numOfFrames,bool looping)
        {
            frameTime = frameSpeed;
            frames = numOfFrames;
            this.looping = looping;
            animation = GameHolder.Game.Content.Load<Texture2D>(asset);
            frameWidth = (animation.Width / frames);
            frameWidth = (animation.Height / frames);
            pos = new Vector2(100,100);
        }
        public void PlayAnim()
        {
            //gameTime GameHolder.gameTime;
            elapsed += (float)GameHolder.gameTime.ElapsedGameTime.TotalMilliseconds;
            sourceRect = new Rectangle(cFrame * frameWidth, 0, frameWidth, frameHeight);
            if (elapsed >= frameTime)
            {
                if (cFrame >=frames-1)
                {
                    if (looping)
                        cFrame = 0;
                }
                else
                {
                    cFrame++;
                }
                elapsed = 0;
            }
        }
        public void Draw()
        {
            GameHolder.spriteBatch.Draw(animation,pos,sourceRect,Color.White,0f, new Vector2(0,0),1f,SpriteEffects.None,1f);
        }
    }
}
