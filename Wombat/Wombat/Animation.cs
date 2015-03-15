using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Wombat
{
    class Animation
    {
        public float scale = 1f;
         public bool active = true;
        public SpriteEffects flip;
        public Texture2D spriteSheet;
        float time;
        float frameTime;
        public int frameIndex;
        int totalFrames;
        public int frameHeight;
        public int frameWidth;
        public Vector2 position;
        Vector2 origin;
        Rectangle source;
        float angle;
        public Color color;
        public bool isPaused = false;

        public void LoadContent(ContentManager theContentManager, string textureName)
        {

            spriteSheet = theContentManager.Load<Texture2D>(textureName);

            frameHeight = spriteSheet.Height;
            frameWidth = spriteSheet.Width / totalFrames;

        }
        public void Initialize(int totalframes, float animationlength, Vector2 startposition, float startangle, Color startcolor)
        {
            totalFrames = totalframes;
            frameTime = animationlength / totalframes;
            position = startposition;
            angle = startangle;
            color = startcolor;
        }
        public void Initialize(int totalframes, float animationlength, Vector2 startposition, float startangle, Color startcolor,bool paused)
        {
            isPaused = paused;
            totalFrames = totalframes;
            frameTime = animationlength / totalframes;
            position = startposition;
            angle = startangle;
            color = startcolor;
        }
        bool runOnce = false;
        public void Initialize( bool runonce,int totalframes, float animationlength, Vector2 startposition, float startangle, Color startcolor)
        {
            runOnce = runonce;
            totalFrames = totalframes;
            frameTime = animationlength / totalframes;
            position = startposition;
            angle = startangle;
            color = startcolor;
        }

        public void Update(GameTime gameTime)
        {

            frameHeight = spriteSheet.Height;
            frameWidth = spriteSheet.Width / totalFrames;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!isPaused)
            {
                while (time > frameTime)
                {
                    frameIndex++;


                    if (frameIndex == totalFrames)
                    {
                        if (!runOnce)
                        {
                            frameIndex = 0;
                        }
                        else
                        {
                            active = false;
                        }

                    }
                    time = 0f;
                }
            }
            if (frameIndex > totalFrames) frameIndex = 1;
            source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(frameWidth / 2.0f, frameHeight / 2.0f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {

                spriteBatch.Draw(spriteSheet, position, source, color, angle,
                  origin, scale, flip, 0f);
            }
        }
    }
}
