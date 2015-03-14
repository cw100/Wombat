using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Wombat
{
    class Powerup
    {
        Vector2 position;
        Texture2D pickupTex;
        public int pickupEffect;
       public  TimeSpan duration;
        public Rectangle hitBox;
        public bool active = true;
        public Color color=Color.Black;
        public void Initialize(Texture2D pickuptex, TimeSpan dur, Vector2 pos, int pickupeffect)

        {
            pickupEffect = pickupeffect;
            pickupTex = pickuptex;
            duration = dur;
                position =pos;
                hitBox = new Rectangle((int)(pos.X - pickuptex.Width / 2), (int)(pos.Y - pickuptex.Height / 2), pickuptex.Height, pickuptex.Width);
                color = Color.Black;
        }
        public void Initialize(TimeSpan dur, int pickupeffect, Color col)
        {
            color = col;
            pickupEffect = pickupeffect;
            duration = dur;
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(pickupTex, position, Color.White);
            }
        }


    }
}
