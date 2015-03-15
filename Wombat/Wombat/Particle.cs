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
    class Particle
    {
        Texture2D particleTexture;
        Vector2 position;
        Vector2 velocity;
        float ttl;
        float timeAlive = 0;
        public bool active = true;
        public Color particleColor;
        
        public void Initialize(Texture2D particletexture, float live, Vector2 intialposition, Color color, Vector2 intialvelocity)
        {
            ttl = live;
            particleTexture = particletexture;
            particleColor = color;
            velocity = intialvelocity;
            position = intialposition;
        }
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                
                particleColor.A = (Byte)(255 - ((255 / ttl) * timeAlive));
                position += velocity * gameTime.ElapsedGameTime.Milliseconds;
                timeAlive += gameTime.ElapsedGameTime.Milliseconds;
                if(timeAlive> ttl)
                {
                    active = false;
                }
            }

        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
               
                spriteBatch.Draw(particleTexture, position, particleColor);
            }
        }
    }
}
