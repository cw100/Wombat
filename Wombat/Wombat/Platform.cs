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
    class Platform
    {
        Vector2 position;
        public Rectangle hitBox;
        Animation platformAnimation;
        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            platformAnimation.LoadContent(theContentManager, textureName);
            hitBox = new Rectangle((int)(position.X - platformAnimation.frameWidth / 2), (int)(position.Y - platformAnimation.frameHeight / 2), platformAnimation.frameWidth, platformAnimation.frameHeight);

        }
        public void Initialize(Vector2 pos)
        {
            platformAnimation = new Animation();
            position = pos;

            platformAnimation.Initialize(1, 1, position, 0f, Color.White);
            }
        public void Update(GameTime gameTime)
        {
            platformAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            platformAnimation.Draw(spriteBatch);
        }



    }
}
