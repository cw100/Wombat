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
    class Player
    {
        Animation playerAnimation;
        public bool active = true;
        float health;
        public Vector2 playerPosition;
        Vector2 preJumpPosition;
        Vector2 jumpHeight;
        public Vector2 velocity;
        Vector2 lastVelocity;
        Vector2 gravity;
        Vector2 lastPosition;
        Texture2D bulletTexture;
        public Rectangle bigHitBox;
        public PlayerIndex playerNumber;
        float playerSpeed;
        public  int damageMultiplyer =1;
        State currentState;
        enum State
        {
            Standing,
            Walking,
            Running,
            Jumping,
            Flying,
            Falling
        }
        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            playerAnimation.LoadContent(theContentManager, textureName);
            bigHitBox = new Rectangle((int)(playerPosition.X - playerAnimation.frameWidth / 2), 
                (int)(playerPosition.Y - playerAnimation.frameHeight / 2), 
                playerAnimation.frameWidth, playerAnimation.frameHeight);

            
         
        }
       public  Powerup currentPowerup;
        public void Initialize(float playerspeed, float starthealth,
            Vector2 startvelocity, Vector2 startgravity,
            Vector2 startposition, PlayerIndex playernumber,
            Vector2 jumpheight)
        {
            bullets = new List<Projectile>();
            playerAnimation = new Animation();
            playerAnimation.Initialize(1, 1, startposition, 0f, Color.White);
            health = starthealth;
            gravity = startgravity;
            velocity = startvelocity;
            playerPosition = startposition;
            currentState = State.Falling;
            playerNumber = playernumber;
            playerSpeed = playerspeed;
            jumpHeight = jumpheight;
            ready = false;
            currentPowerup = new Powerup();
            currentPowerup.Initialize(TimeSpan.FromDays(1), 1,Color.White);

        }
        public bool ready = false;
        bool flipped;
        public void DirectionCheck()
        {
            if (GamePad.GetState(playerNumber).ThumbSticks.Left.X != 0)
            {
                if (GamePad.GetState(playerNumber).ThumbSticks.Left.X < 0)
                {
                    playerAnimation.flip = SpriteEffects.FlipHorizontally;
                    flipped = true;
                }
                else
                {

                    playerAnimation.flip = SpriteEffects.None;
                }
            }

        }
        List<Projectile> bullets;
        Vector2 gunAngle;
        Vector2 angle ;
        Projectile bullet ;
        float bulletSpeed = 1.5f;
        TimeSpan previousFireTime;
        TimeSpan shootSpeed =TimeSpan.FromSeconds(0.2);
        TimeSpan shootSpeedRocket = TimeSpan.FromSeconds(1);
       public  TimeSpan previousTime;
        public void CheckPowerup(GameTime gameTime)

        {
            if (gameTime.TotalGameTime - previousTime > currentPowerup.duration)
            {

                currentPowerup = new Powerup();
                currentPowerup.Initialize(TimeSpan.FromDays(1), 1, Color.White);
                
                previousTime = gameTime.TotalGameTime; 
                damageMultiplyer = 1;
            }
        }
        public void AddProjectile()
        {
            bullet = new Projectile();


            bullet.Initialize(bulletTexture, 5 * currentPowerup.pickupEffect, bulletSpeed, playerPosition, angle, playerNumber, false, Game1.particle);

            Game1.AllBullets.Add(bullet);
        }
        public void AddRocket()
        {
            bullet = new Projectile();


            bullet.Initialize(rocketTexture, 50 * currentPowerup.pickupEffect, 0.5f, playerPosition, angle, playerNumber, true, Game1.particle);
            bullet.color = Color.Black;
            Game1.AllBullets.Add(bullet);
        }

        public void Shoot(GameTime gameTime)
        {
            if (GamePad.GetState(playerNumber).ThumbSticks.Right.X != 0 || GamePad.GetState(playerNumber).ThumbSticks.Right.Y != 0)
            {
               angle = GamePad.GetState(playerNumber).ThumbSticks.Right;
            }
            if (gameTime.TotalGameTime - previousFireTime > shootSpeed)
            {
                if (GamePad.GetState(playerNumber).Triggers.Right >= 0.5f)
                {
                   
                    previousFireTime = gameTime.TotalGameTime;
                    AddProjectile();
                }
            }
            if (gameTime.TotalGameTime - previousFireTime > shootSpeedRocket)
            {
                if (GamePad.GetState(playerNumber).Triggers.Left >= 0.5f)
                {
                    previousFireTime = gameTime.TotalGameTime;
                    AddRocket();
                }
            }
        }
        

        public void ScreenCollision()
        {
            playerPosition.X = MathHelper.Clamp(playerPosition.X, 0 + (playerAnimation.frameWidth / 2), 1920 - (playerAnimation.frameWidth / 2));

            playerPosition.Y = MathHelper.Clamp(playerPosition.Y, 0 + (playerAnimation.frameHeight / 2), 1080 - (playerAnimation.frameHeight / 2));

        }


        public void ControllerMove(GameTime gameTime)
        {


            velocity.X += GamePad.GetState(playerNumber).ThumbSticks.Left.X * playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity.X = MathHelper.Clamp(velocity.X, -playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);


        }
        public void ApplyFriction(GameTime gameTime)
        {
            velocity.X *= 0.8f;
        }
        public void ApplyGravity(GameTime gameTime)
        {

            velocity.Y += gravity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;



        }
        public void Jump(GameTime gameTime)
        {
            if (currentState != State.Jumping && currentState != State.Falling)
            {
                if (GamePad.GetState(playerNumber).IsButtonDown(Buttons.A))
                {

                    preJumpPosition = playerPosition;
                    currentState = State.Jumping;
                    velocity.Y = -(float)Math.Sqrt((2 * jumpHeight.Y) * (gravity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds));
                }

            }
            if (currentState == State.Jumping)
            {

                if (playerPosition.Y < preJumpPosition.Y - jumpHeight.Y)
                {

                    currentState = State.Falling;
                }

            }
        }
        public void StateManager()
        {
            if (playerPosition.Y + (playerAnimation.frameHeight / 2) >= 1080)
            {
                currentState = State.Standing;
            }
        }
       
        Texture2D gunTex;
       

        Texture2D rocketTexture;

        public void Update(GameTime gameTime, List<Rectangle> platformRectangles, Collision collisionManager,
            Texture2D bullettexture, Texture2D gun, Texture2D rockettexture)
        {

            if (active)
            {
                gunTex = gun;
                
                rocketTexture = rockettexture;
                bulletTexture = bullettexture;
                
                lastVelocity = velocity;
                lastPosition = playerPosition;

                ApplyFriction(gameTime);
                ControllerMove(gameTime);
                DirectionCheck();

                StateManager();

                ApplyGravity(gameTime);
                Jump(gameTime);

                playerPosition += velocity;

                ScreenCollision();

                bigHitBox.X = (int)((playerPosition.X - playerAnimation.frameWidth / 2));
                bigHitBox.Y = (int)((playerPosition.Y - playerAnimation.frameHeight / 2));
                
                Shoot(gameTime);
                foreach (Projectile collisionBullet in Game1.AllBullets)
                {
                    if (collisionBullet.playerNum != playerNumber)
                    {
                        if (collisionBullet.hitBox.Intersects(bigHitBox))
                        {

                            if(collisionBullet.explosive)
                            {
                                Game1.AddExplosion(collisionBullet.Position);

                                Vector2 direction = new Vector2(Game1.explosionHitBoxes[Game1.explosionHitBoxes.Count - 1].Center.X,
                                    Game1.explosionHitBoxes[Game1.explosionHitBoxes.Count - 1].Center.Y) 
                                    - new Vector2(bigHitBox.Center.X, bigHitBox.Center.Y);
                                float distance = Vector2.Distance(new Vector2(Game1.explosionHitBoxes[Game1.explosionHitBoxes.Count - 1].Center.X,
                                    Game1.explosionHitBoxes[Game1.explosionHitBoxes.Count - 1].Center.Y), playerPosition);
                                
                                direction.X = -direction.X;
                                direction.Y = -direction.Y;
                                velocity += (50 / (1 + distance) * direction);
                            }
                            collisionBullet.active = false;
                            
                                health -= collisionBullet.projectileDamage ;
                            

                        }
                    }
                }

                CheckPowerup(gameTime);
                if(health<=0)
                {
                    Game1.AddExplosion(playerPosition, 4f);
                    Game1.deadCount += 1;
                    active = false;
                }
                foreach (Rectangle platform in platformRectangles)
                {
                    if (collisionManager.RectangleCollisionLeft(bigHitBox, platform, velocity))
                    {
                        playerPosition.X = platform.Left - playerAnimation.frameWidth / 2;
                        velocity.X = 0;
                        bigHitBox.X = (int)(playerPosition.X - playerAnimation.frameWidth / 2);
                        bigHitBox.Y = (int)(playerPosition.Y - playerAnimation.frameHeight / 2);
                    }


                    if (collisionManager.RectangleCollisionRight(bigHitBox, platform, velocity))
                    {
                        playerPosition.X = platform.Right + playerAnimation.frameWidth / 2;
                        velocity.X = 0;
                        bigHitBox.X = (int)(playerPosition.X - playerAnimation.frameWidth / 2);
                        bigHitBox.Y = (int)(playerPosition.Y - playerAnimation.frameHeight / 2);

                    }
                    if (collisionManager.RectangleCollisionTop(bigHitBox, platform, velocity))
                    {
                        playerPosition.Y = (platform.Top - playerAnimation.frameHeight / 2);
                        velocity.Y = 0;
                        bigHitBox.X = (int)(playerPosition.X - playerAnimation.frameWidth / 2);
                        bigHitBox.Y = (int)(playerPosition.Y - playerAnimation.frameHeight / 2);
                        if (currentState != State.Jumping || currentState != State.Falling)
                        {
                            currentState = State.Standing;
                        }
                    }

                    if (collisionManager.RectangleCollisionBottom(bigHitBox, platform, velocity))
                    {
                        playerPosition.Y = (platform.Bottom + playerAnimation.frameHeight / 2);
                        velocity.Y = 0;
                        bigHitBox.X = (int)(playerPosition.X - playerAnimation.frameWidth / 2);
                        bigHitBox.Y = (int)(playerPosition.Y - playerAnimation.frameHeight / 2);

                    }



                }
               
                playerAnimation.color = color;
                playerAnimation.position = new Vector2((int)playerPosition.X, (int)playerPosition.Y);
                playerAnimation.Update(gameTime);
                
            }
        }
        public Color color= Color.SandyBrown;

        public int colorNum = 1;
        public void DrawGun(SpriteBatch spriteBatch)
        {
            if (active)
            {
                if (gunTex != null)
                {
                    spriteBatch.Draw(gunTex, playerPosition, new Rectangle(0, 0, gunTex.Width, gunTex.Height),
                        currentPowerup.color, (float)(Math.Atan2(angle.X, angle.Y) - Math.PI / 2), new Vector2(11, 11), 1f, SpriteEffects.None, 1f);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                
                playerAnimation.Draw(spriteBatch);
            
            }
        }


    }
}
