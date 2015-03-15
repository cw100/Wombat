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
    class Emiter
    {
        Texture2D particleTexture;
        Vector2 position;
        Vector2 velocity;
        float ttl;
        public int index;
        Color particleColor;
        Particle particle;
        List<Particle> particles;
        int maxParticles;
        int currentParticles;
        public void Initialize(Texture2D particletexture, float live, Vector2 intialposition, Color color, Vector2 intialvelocity, int max)
        {
            ttl = live;
            particleTexture = particletexture;
            particleColor = color;
            velocity = intialvelocity;
            position = intialposition;
            maxParticles = max;
            currentParticles = 0;
            particles = new List<Particle>();
        }
        public void AddParticle()
        {
            particle = new Particle();
            particle.Initialize(particleTexture, ttl, position, particleColor, velocity);
            particles.Add(particle);


        }
        public void KillParticles()
        {
            for (int i = 0; i < particles.Count; i++ )
            {
                if(particles[i].active == false)
                {
                    particles.RemoveAt(i);
                }
            }
        }
       public void Update(GameTime gameTime, Vector2 newPostition)
        {
           position =  newPostition;
           if(currentParticles < maxParticles)
           {
               AddParticle();
           }
           foreach(Particle updateParticles in particles)
           {
               updateParticles.Update(gameTime);
           }
           KillParticles();

        }
       public void Draw(SpriteBatch spriteBatch)
       {
           foreach (Particle drawParticles in particles)
           {
               drawParticles.Draw(spriteBatch);
           }
       }



    }
}
