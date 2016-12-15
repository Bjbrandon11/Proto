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
        String asset;
        float elapsed;
        float frameTime;
        int frames;
        int cFrame;
        int width;
        int height;
        int frameWidth;
        int frameHeight;
        bool looping;
        float size,rotation;
        SpriteEffects se;
        public Animation(string asset, float frameSpeed,int numOfFrames,bool looping)
        {
            frameTime = frameSpeed;
            frames = numOfFrames;
            this.asset = asset;
            this.looping = looping;
            animation = GameHolder.Game.Content.Load<Texture2D>(asset);
            frameWidth = (animation.Width / frames);
            frameHeight = (animation.Height);
            se = SpriteEffects.None;
            pos = new Vector2(100,100);
            size = .5f;
            rotation = 0f;
        }
        public Animation(string asset, float frameSpeed, int numOfFrames, bool looping,SpriteEffects se)
        {
            frameTime = frameSpeed;
            frames = numOfFrames;
            this.looping = looping;
            animation = GameHolder.Game.Content.Load<Texture2D>(asset);
            frameWidth = (animation.Width / frames);
            frameHeight = (animation.Height);
            this.se = se;
            pos = new Vector2(100, 100);
            size = .5f;
            rotation = 0f;
        }
        public void PlayAnim()
        {
            //gameTime GameHolder.gameTime;
            elapsed += (float)GameHolder.gameTime.ElapsedGameTime.TotalMilliseconds;
            sourceRect = new Rectangle(cFrame * frameWidth, 0, frameWidth, frameHeight);
            if (elapsed >= frameTime)
            {
                //if (cFrame >=frames-1)
                if(cFrame==0)
                {
                    if (looping)
                        cFrame = 1;
                }
                else
                {
                    if (cFrame < frames-1)
                        cFrame++;
                    else
                        cFrame = 0;
                }
                elapsed = 0;
            }
        }
        public void setVect(Vector2 temp) { pos = new Vector2(temp.X-frameWidth*size/2,temp.Y-frameHeight*size/2); }
        public void setLoop(bool lp)
        {
            looping = lp;
            if(!lp)
            cFrame = 0;
        }
        public bool Equals(Animation temp)
        {
            return (se == temp.se && asset.Equals(temp.asset) && frameTime == temp.frameTime && looping == temp.looping);
        }
        public void setRotate(float r)
        {
            rotation =r;
        }
        public void Draw()
        {
            
            GameHolder.spriteBatch.Draw(animation,pos,sourceRect,Color.White,rotation, new Vector2(0,0),size,se,1f);
        }
    }
}
