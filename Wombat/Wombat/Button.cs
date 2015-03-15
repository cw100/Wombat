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
    class Button
    {
        int menuNum;
        public bool selected =  false;
        public string buttonName;
        public string fileName;
        Vector2 position;
        public Rectangle hitBox;
        public Animation buttonAnimation;
        public bool pressed = false;
        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            buttonAnimation.LoadContent(theContentManager, textureName);
            hitBox = new Rectangle((int)(position.X - buttonAnimation.frameWidth / 2), (int)(position.Y - buttonAnimation.frameHeight / 2), buttonAnimation.frameWidth, buttonAnimation.frameHeight);

        }
        public void Initialize(Vector2 pos, string filename, string buttonname, int menunum)
        {
            menuNum = menunum;
            buttonAnimation = new Animation();
            fileName = filename;
            buttonName = buttonname;
            buttonAnimation.Initialize(1, 1, pos, 0f, Color.White,true);
            position = pos;
        }
        public bool CheckForClick(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                
                if(hitBox.Contains((int)mouseState.X, (int)mouseState.Y))
                {
                    pressed = true;
                return true;
            }

            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                if (selected)
                {
                    pressed = true;
                    return true;
                }
            }
            pressed = false;
            return false;

        }

        public void Update(GameTime gameTime, MouseState mouseState,int currentnum)
        {
            if (hitBox.Contains((int)mouseState.X, (int)mouseState.Y) || menuNum == currentnum)
            {
                selected = true;
            }
            else
            {
                selected = false;
            }
            if(selected == true)
            {
                buttonAnimation.scale = 1.2f;
                buttonAnimation.color = Color.Green;
            }
            else
            {
                buttonAnimation.scale = 1f;
                buttonAnimation.flip = SpriteEffects.None;
                buttonAnimation.color = Color.White;

            }

            buttonAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            buttonAnimation.Draw(spriteBatch);
        }



    }
}
