#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Wombat
{

    public class Game1 : Game
    {

        enum GameScreen
        {
            MainMenu,
            GameRunning,

        }


        static public List<Rectangle> explosionHitBoxes;
        static public List<Projectile> AllBullets;
        Texture2D bulletTexture;
        int currentMenuItem= 1;
        GameScreen currentGameScreen = GameScreen.MainMenu;
        List<Platform> platforms;
        List<Button> menuButtons;
        List<Rectangle> platformHitBoxes;
        MouseState mouseState;
        Collision collisionManager = new Collision();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D t;
        Vector2 globalGravity;
        Vector2 playerJumpHeight;
        float playerSpeed;
        float playerStartHealth;
        List<Player> players;
       
        int VirtualScreenWidth = 1920;
        int VirtualScreenHeight = 1080;
        Vector3 screenScale;


        public Game1()
            : base()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;

            graphics.PreferredBackBufferWidth = 1920;


            Window.IsBorderless = true;
        }

        public void AddPlatform(Vector2 position)
        {
            Platform platform = new Platform();
            platform.Initialize(position);
            platforms.Add(platform);
        }
        
        public void InitializePlayers(int numOfPlayers)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                Player player;
                PlayerIndex playerIndex;
                playerIndex = (PlayerIndex)i;
                player = new Player();
                player.Initialize(playerSpeed, playerStartHealth, new Vector2(0, 0), globalGravity,
                   new Vector2(0 + i * 300, 0), playerIndex, playerJumpHeight);
                players.Add(player);

            }

        }
       public void InitializeGameScreen()
        {
            globalGravity = new Vector2(0, 100f);
            playerSpeed = 1000;
            playerJumpHeight = new Vector2(0, 400f);
            playerStartHealth = 100;
            players = new List<Player>();
            platforms = new List<Platform>();
            platformHitBoxes = new List<Rectangle>();
            AddPlatform(new Vector2(150, 300));
            AddPlatform(new Vector2(200, 550));
            AddPlatform(new Vector2(350, 800));
            AddPlatform(new Vector2(850, 470));
            AllBullets = new List<Projectile>();
            InitializePlayers(4);
        }
       public void InitializeMainMenu()
       {
           menuButtons = new List<Button>();
           
           Button button= new Button();
           button.Initialize(new Vector2(134 + 420 / 2, 540 + 140 / 2), "PlayButton", "play", 1);
           menuButtons.Add(button);
           button = new Button();
           button.Initialize(new Vector2(730 + 420 / 2, 540 + 140 / 2), "OptionsButton", "options", 2);
           menuButtons.Add(button);
            button = new Button();
            button.Initialize(new Vector2(1336 + 420 / 2, 540 + 140 / 2), "ExitButton", "exit", 3);
           menuButtons.Add(button);
       }
       static Texture2D explosionTex;
       static List<Animation> explosions;
       static public void AddExplosion(Vector2 position)
       {
           Animation explosion = new Animation();


           explosion.spriteSheet = explosionTex;
           explosion.Initialize(true, 10, 0.25f, position, 0f, Color.White);
           explosion.scale = 2f;
           explosionHitBoxes.Add(new Rectangle((int)position.X, (int)position.Y, explosion.frameWidth * 2, explosion.frameHeight * 2));
           explosions.Add(explosion);
       }
       static public void AddExplosion(Vector2 position,float scale)
       {
           Animation explosion = new Animation();

           
           explosion.spriteSheet = explosionTex;
           explosion.Initialize(true,10, 0.25f, position, 0f, Color.White);
           explosion.scale = scale;
           explosionHitBoxes.Add(new Rectangle((int)position.X, (int)position.Y, explosion.frameWidth*2, explosion.frameHeight*2));
           explosions.Add(explosion);
       }
        protected override void Initialize()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / (float)VirtualScreenWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / (float)VirtualScreenHeight;
            explosionHitBoxes = new List<Rectangle>();
            explosions = new List<Animation>();
            screenScale = new Vector3(scaleX, scaleY, 1.0f);
            mouseState = new MouseState();
            
            InitializeGameScreen();

            InitializeMainMenu();
            base.Initialize();

        }

        static public Texture2D particle;
        Texture2D gun;
        Texture2D rocketTexture;
        Texture2D MenuBackground;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            particle = Content.Load<Texture2D>("particle");
            bulletTexture = Content.Load<Texture2D>("bullet");
            rocketTexture = Content.Load<Texture2D>("Rocket");
            gun = Content.Load<Texture2D>("gun");
            explosionTex = Content.Load<Texture2D>("explosion");

            MenuBackground = Content.Load<Texture2D>("MainScreen");
            foreach (Player player in players)
            {
                player.LoadContent(Content, "Wombat");
            }

            foreach (Platform platform in platforms)
            {
                platform.LoadContent(Content, "platform");
                platformHitBoxes.Add(platform.hitBox);
            }
            foreach(Button button in menuButtons)
            {
                button.LoadContent(Content, button.fileName);
            }
            
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { Color.White });


        }

       
        protected override void UnloadContent()
        {
        }
        public void ApplyExplosive()
        {
            foreach(Player ply in players)
            { 
                foreach(Rectangle explo in explosionHitBoxes)
            {
                if(explo.Intersects(ply.bigHitBox))
                {
                    Vector2 direction = new Vector2(explo.Center.X, explo.Center.Y) - new Vector2( ply.bigHitBox.Center.X,ply.bigHitBox.Center.Y);
                    float distance = Vector2.Distance(new Vector2(explo.Center.X, explo.Center.Y), ply.playerPosition);
                    
                    direction.X = -direction.X;
                    direction.Y = -direction.Y;
                    ply.velocity += (10/(1+distance) *  direction);
                }
            }
            }
        }
        public void UpdateGame(GameTime gameTime)
        {
            foreach (Platform platform in platforms)

                platform.Update(gameTime);
            ApplyExplosive();
            foreach (Player player in players)
                player.Update(gameTime, platformHitBoxes, collisionManager, bulletTexture,  gun, rocketTexture);
            
            UpdateBullets(gameTime);
            UpdateExplosions(gameTime);

        }
        public void UpdateExplosions(GameTime gameTime)
        {
            for (int i= 0; i < explosions.Count;i++ )
            {
                explosions[i].Update(gameTime);
                if(!explosions[i].active )
                {

                    explosionHitBoxes.RemoveAt(i);
                    explosions.RemoveAt(i);
                }
            }
        }
        float elapsedTime;
        float menuTime = 100; 
        public void MenuSelect(GameTime gameTime)
        {
            if(1 <= currentMenuItem && currentMenuItem <= menuButtons.Count)
            {

                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                 if (elapsedTime > menuTime)
                {
                    currentMenuItem += (int)(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X);
                    elapsedTime = 0;
                }
                 
            }

            
            if (1 > currentMenuItem)
            {
                currentMenuItem = 1;
            }
            if (currentMenuItem > menuButtons.Count)
            {
                currentMenuItem = menuButtons.Count;
            }
        }
        public void PlatformBulletCollision()
        {
            foreach (Rectangle platform in platformHitBoxes)
            {
                foreach (Projectile projectile in AllBullets)
                {
                    if(projectile.hitBox.Intersects(platform))
                    {
                        if (projectile.explosive)
                        {
                            Game1.AddExplosion(projectile.Position);
                        }
                        projectile.active = false;
                        
                    }
                }
            }
        }
        public void RemoveBullets()
        {
            for (int i = 0; i < AllBullets.Count; i++)
            {
                if (AllBullets[i].active == false)
                {
                    AllBullets.RemoveAt(i);
                }
            }
        }
        public void UpdateBullets(GameTime gameTime)
        {
            PlatformBulletCollision();
            foreach (Projectile projectile in AllBullets)
            {
                projectile.Update(gameTime);
            }
            RemoveBullets();
        }
        public void UpdateMenu(GameTime gameTime)
        {
            MenuSelect(gameTime);
            foreach (Button button in menuButtons)
            {
                button.Update(gameTime,mouseState, currentMenuItem);
                if(button.CheckForClick(mouseState))
                {

                    if(button.buttonName =="play")
                    {
                        currentGameScreen = GameScreen.GameRunning;
                    }
                    if (button.buttonName == "exit")
                    {
                        Exit();
                    }
                }
            }

        }
        
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseState = Mouse.GetState();
            if (currentGameScreen == GameScreen.GameRunning)
            {
                UpdateGame(gameTime);
            }
            if (currentGameScreen == GameScreen.MainMenu)
            {
                UpdateMenu(gameTime);
            }
            base.Update(gameTime);
        }
        public void DrawExplosions(SpriteBatch spriteBatch)
        {
            foreach(Animation expl in explosions)
            {
                expl.Draw(spriteBatch);
            }
        }
        public void DrawGame(GameTime gameTime)
        {
            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            Color[] color = new Color[1 * 1];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.White;
            }
            rectangleTexture.SetData(color);

            foreach (Platform platform in platforms)
            {
                platform.Draw(spriteBatch);
            }

            foreach (Player player in players)
            {
               // spriteBatch.Draw(rectangleTexture, player.bigHitBox, Color.Green);


                player.Draw(spriteBatch);
               }
            
            foreach (Projectile projectile in AllBullets)
            {
                projectile.Draw(spriteBatch);
            }
            foreach (Player player in players)
            {
         player.DrawGun(spriteBatch);
            }
            DrawExplosions(spriteBatch);
        }
        public void DrawMenu(GameTime gameTime)
        {
            
                spriteBatch.Draw(MenuBackground, new Vector2(0, 0), Color.White);
            foreach (Button button in menuButtons)
            
            {
                button.Draw(spriteBatch);
            }

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (currentGameScreen == GameScreen.GameRunning)
            {
                DrawGame(gameTime);
            }

            if (currentGameScreen == GameScreen.MainMenu)
            {
                DrawMenu(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
