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
        Vector2 Position;
        Texture2D projectileTexture;
        public int projectileDamage;
        float projectileSpeed;
        public bool active = true;
        public Rectangle hitBox;
        public PlayerIndex playerNum;

        public void Initialize(Texture2D projetiletexture, int damage, float speed, Vector2 position, Vector2 angle, PlayerIndex playernum)
        {
            playerNum = playernum;
            Position = position;
            direction = angle;
            if (direction.X == 0 && direction.Y == 0)
            {
                direction = new Vector2(0, 1);
            }
            direction.Normalize();

            projectileDamage = damage;
            projectileSpeed = speed;
            projectileTexture = projetiletexture;
            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)projectileTexture.Width, (int)projectileTexture.Height);
        }
        Vector2 velocity;
        public void Update(GameTime gametime)
        {
            velocity = new Vector2(direction.X, -direction.Y) * projectileSpeed * gametime.ElapsedGameTime.Milliseconds;
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
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (active)
                spritebatch.Draw(projectileTexture, Position, new Rectangle(0, 0, 10, 10), Color.Red, 0f, new Vector2(5, 5), 4f, SpriteEffects.None, 1f);




        }
    }
}
