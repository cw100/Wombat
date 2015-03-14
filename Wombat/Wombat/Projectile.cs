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
    public class Projectile
    {
        Vector2 direction;
        public Vector2 Position;
        Texture2D projectileTexture;
        public int projectileDamage;
        float projectileSpeed;
        public bool active = true;
        public Rectangle hitBox;
        public PlayerIndex playerNum;
        public bool explosive;
        List<Emiter> emiters;
        Texture2D particle;
        public void Initialize(Texture2D projetiletexture, int damage, float speed, Vector2 position, Vector2 angle, PlayerIndex playernum, bool Explosive, Texture2D fire)
        {

            particle = fire;
            playerNum = playernum;
            Position = position;
            direction = angle;
            if (direction.X == 0 && direction.Y == 0)
            {
                direction = new Vector2(0, 1);
            }
            direction.Normalize();
            explosive = Explosive;
            projectileDamage = damage;
            projectileSpeed = speed;
            projectileTexture = projetiletexture;
            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)projectileTexture.Width, (int)projectileTexture.Height);
            emiters = new List<Emiter>();
            if(explosive)
            {
                AddEmiters(Position);
            }
        }
        Vector2 velocity;
        public void Update(GameTime gameTime)
        {
            velocity = new Vector2(direction.X, -direction.Y) * projectileSpeed * gameTime.ElapsedGameTime.Milliseconds;
            Position += velocity;
            hitBox.X = (int)(Position.X + velocity.X);
            hitBox.Y = (int)(Position.Y + velocity.Y);
            if (Position.X > 1920)
            {
                active = false;
            }
            if (Position.Y > 1080)
            {
                active = false;
            }
            if (Position.X + projectileTexture.Width < 0)
            {
                active = false;
            }
            if (Position.Y + projectileTexture.Height < 0)
            {
                active = false;
            }

            UpdateEmiters(gameTime);
        }
        public void AddEmiters(Vector2 pos)
        {

            Emiter emiter = new Emiter();
            emiter.Initialize(particle, 100, pos - new Vector2(projectileTexture.Width/2, projectileTexture.Height/2), Color.Black, new Vector2(-direction.X, direction.Y), 10);
            
            emiters.Add(emiter);

        }
        public void UpdateEmiters(GameTime gameTime)
        {
            for (int i = 0; i < emiters.Count; i++)
            {

                emiters[i].Update(gameTime, Position - new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2));

            }
        }
        public void DrawEmiters(SpriteBatch spriteBatch)
        {
            foreach (Emiter emiter in emiters)
            {
                emiter.Draw(spriteBatch);

            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                DrawEmiters(spriteBatch);
                
                spriteBatch.Draw(projectileTexture, Position, new Rectangle(0, 0, projectileTexture.Width, projectileTexture.Height), Color.Red,
                    (float)(Math.Atan2(direction.X, direction.Y) - Math.PI / 2), new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2), 1f, SpriteEffects.None, 1f);
           

            }

        }
    }
}
