#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Wombat
{

    public class Game1 : Game
    {

        enum GameScreen
        {
            MainMenu,
            SelectScreen,
            GameRunning,
            GameOver

        }
        Texture2D playerIcon;
        Texture2D backgroundGame;
        Song mainMenuMusic;
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
        static public int deadCount;
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
        List<Vector2> SpawnList = new List<Vector2>();
        public void InitializePlayers(int numOfPlayers)
        {
            SpawnList.Add(new Vector2(2 + 150, 350));
            SpawnList.Add(new Vector2(2 + 150, 850));
            SpawnList.Add(new Vector2(1610 + 150, 850));
            SpawnList.Add(new Vector2(1612 + 150, 400));
            for (int i = 0; i < numOfPlayers; i++)
            {
                Player player;
                PlayerIndex playerIndex;
                playerIndex = (PlayerIndex)i;
                player = new Player();
                player.Initialize(playerSpeed, playerStartHealth, new Vector2(0, 0), globalGravity,
                  SpawnList[i], playerIndex, playerJumpHeight);
                players.Add(player);

            }

        }
        int numOfPlayers = 4;
       public void InitializeGameScreen()
        {
            InitializePowerups();
            explosionHitBoxes = new List<Rectangle>();
            explosions = new List<Animation>();
            globalGravity = new Vector2(0, 100f);
            playerSpeed = 750;
            playerJumpHeight = new Vector2(0, 300f);
            playerStartHealth = 100;
            players = new List<Player>();
            platforms = new List<Platform>();
            platformHitBoxes = new List<Rectangle>();
            AddPlatform(new Vector2(2+150, 469));
            AddPlatform(new Vector2(2 + 150, 930));
            AddPlatform(new Vector2(777 + 150, 368));
            AddPlatform(new Vector2(489 + 150, 563));

            AddPlatform(new Vector2(189 + 150, 724));

            AddPlatform(new Vector2(1078 + 150, 563));

            AddPlatform(new Vector2(1378 + 150, 724));

            AddPlatform(new Vector2(789 + 150, 889));
            AddPlatform(new Vector2(1610 + 150, 931));

            AddPlatform(new Vector2(1612+150, 469));

            AllBullets = new List<Projectile>();
            InitializePlayers(numOfPlayers);
        }
       public void InitializeMainMenu()
       {
           menuButtons = new List<Button>();

           //MediaPlayer.Play(mainMenuMusic);
           Button button= new Button();
           button.Initialize(new Vector2((1920)/ 2, 500 + 140 / 2), "Playbuttonnew", "play", 1);
           menuButtons.Add(button);
           //button = new Button();
           //button.Initialize(new Vector2(730 + 420 / 2, 540 + 140 / 2), "OptionsButton", "options", 2);
           //menuButtons.Add(button);
            button = new Button();
            button.Initialize(new Vector2((1920)/ 2, 700 + 140 / 2), "ExitButtonnew", "exit", 2);
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
       List<Texture2D> playerIcons;
        public void InitializeSelectScreen()
       {
           playerIcon = Content.Load<Texture2D>("wombat");
           playerColors = new List<Color>();
           playerIcons = new List<Texture2D>();
           playerColors.Add(Color.White);
           playerColors.Add(Color.Black);
           playerColors.Add(Color.Blue);
           playerColors.Add(Color.Red);
           playerColors.Add(Color.SandyBrown);
           playerColors.Add(Color.Green);
           playerColors.Add(Color.Yellow);
            
            playerIcons.Add(playerIcon);
            playerIcons.Add(playerIcon);
            playerIcons.Add(playerIcon);
            playerIcons.Add(playerIcon);


       }
        public void UpdateSelectScreen(GameTime gameTime)
        {
            
          
                for (int i = 0; i < players.Count; i++)
                {
                    if (!players[i].ready)
                    {
                    ColorSelect(gameTime, i);
                }
                    if (GamePad.GetState((PlayerIndex)i).Buttons.A == ButtonState.Pressed&&!players[i].ready)
                    {
                        players[i].ready = true;
                        readyPlayers += 1;
                    }
                    if (GamePad.GetState((PlayerIndex)i).Buttons.B == ButtonState.Pressed && players[i].ready)
                    {
                        players[i].ready = false;
                        readyPlayers -= 1;
                    }
                }
            if(readyPlayers == 4)
            {
                currentGameScreen = GameScreen.GameRunning;
            }

                
            
        }
        public void DrawIcons(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].ready)
                {
                    spriteBatch.Draw(playerIcons[i], new Vector2((1920-150)/2, 300+100 * i), players[i].color);
                }
                else
                {
                    spriteBatch.Draw(playerIcons[i], new Vector2(100 + (1920 - 150) / 2, 300 + 100 * i), players[i].color);
            }
            }
        }
        int colorNum;
        List<Color> playerColors;
        public void ColorSelect(GameTime gameTime, int num)
        {

            if (1 <= players[num].colorNum && players[num].colorNum <= playerColors.Count)
            {

                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedTime > menuTime)
                {
                    players[num].colorNum += (int)(GamePad.GetState(players[num].playerNumber).ThumbSticks.Left.X);
                    elapsedTime = 0;
                }

            }


            if (1 > players[num].colorNum)
            {
                players[num].colorNum = 1;
            }
            if (players[num].colorNum > playerColors.Count)
            {
                players[num].colorNum = playerColors.Count;
            }
            players[num].color = playerColors[players[num].colorNum-1];
         
        }
        protected override void Initialize()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / (float)VirtualScreenWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / (float)VirtualScreenHeight;
            
            screenScale = new Vector3(scaleX, scaleY, 1.0f);
            mouseState = new MouseState();
            readyPlayers = 0;
            InitializeGameScreen();
            InitializeSelectScreen();
            InitializeMainMenu();
            base.Initialize();

        }
        int readyPlayers;
        static public Texture2D particle;
        Texture2D gun;
        Texture2D rocketTexture;
        Texture2D MenuBackground;
        Texture2D playerOneWin, playerTwoWin, playerThreeWin, playerFourWin, winningTex;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            powerupTexure = Content.Load<Texture2D>("powerup");
            playerOneWin = Content.Load<Texture2D>("playerOneWin");

            playerTwoWin = Content.Load<Texture2D>("playerTwoWins");

            playerThreeWin = Content.Load<Texture2D>("playerThreeWins");

            playerFourWin = Content.Load<Texture2D>("playerFourWins"); 
            particle = Content.Load<Texture2D>("particle");
            bulletTexture = Content.Load<Texture2D>("bullet");
            rocketTexture = Content.Load<Texture2D>("Rocket");
            gun = Content.Load<Texture2D>("gun");
            explosionTex = Content.Load<Texture2D>("explosion");
            backgroundGame = Content.Load<Texture2D>("BackgroundGame");
            MenuBackground = Content.Load<Texture2D>("BackgroundMenu");
            foreach (Player player in players)
            {
                player.LoadContent(Content, "Wombat");
            }

            foreach (Platform platform in platforms)
            {
                platform.LoadContent(Content, "Plank");
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
        public void ResetGame()
        {
            deadCount = 0;
            Initialize();
        }
        int winningPlayer;
        public void GameOverCheck()
        {
            if (deadCount == (numOfPlayers - 1))
            {
                foreach(Player player in players)
                {
                    if(player.active)
                    {
                        winningPlayer = (int)player.playerNumber;
                        switch (winningPlayer)
                        {
                            case (int)PlayerIndex.One:
                                winningTex = playerOneWin;
                                break;
                            case (int)PlayerIndex.Two:
                                winningTex = playerTwoWin;
                                break;
                            case (int)PlayerIndex.Three:
                                winningTex = playerThreeWin;
                                break;
                            case (int)PlayerIndex.Four:
                                winningTex = playerFourWin;
                                break;
                        }
                        currentGameScreen = GameScreen.GameOver;
                    }
                }

             }
        }
        protected override void UnloadContent()
        {
        }
        Random random;
        List<Powerup> powerups;
        public void InitializePowerups()
        {

            powerupSpawns = new List<Vector2>();
            powerups = new List<Powerup>();
            powerupSpawns.Add(new Vector2(1920/2, 500));
            powerupSpawns.Add(new Vector2((1920/3), 400));
                powerupSpawns.Add(new Vector2((1920/3)*2, 400));
            powerupSpawns.Add(new Vector2((1920/3), 800));
                powerupSpawns.Add(new Vector2((1920/3)*2, 800));
        }
        List<Vector2> powerupSpawns;
        Texture2D powerupTexure;
        public void AddPowerup()
        {
            random = new Random();
            int rnd = random.Next(5);
            Powerup powerup = new Powerup();
            powerup.Initialize(powerupTexure,TimeSpan.FromSeconds(10),powerupSpawns[rnd],100);
            powerups.Add(powerup);

        }
        public void UpdatePowerups(GameTime gameTime)
        {
            for(int i = 0; i<players.Count; i++)
            {
                for (int j = 0; j < powerups.Count; j++)
                {
                    if (players[i].bigHitBox.Intersects(powerups[j].hitBox))
                    {
                        players[i].currentPowerup = powerups[j];
                        players[i].previousTime = gameTime.TotalGameTime;
                        powerups[j].active = false;
                        powerups.RemoveAt(j);
                    }
                }
            }
                }
        
        public void DrawPowerups(SpriteBatch spriteBatch)
        {
            foreach(Powerup pwerup in powerups)
            {
                pwerup.Draw(spriteBatch);
            }
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
        TimeSpan lastTime;
        
        public void UpdateGame(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastTime > TimeSpan.FromSeconds(10))
            {
                lastTime = gameTime.TotalGameTime;
                AddPowerup();
            }
            UpdatePowerups(gameTime);
            foreach (Platform platform in platforms)
                platform.Update(gameTime);
            ApplyExplosive();
            foreach (Player player in players)
                player.Update(gameTime, platformHitBoxes, collisionManager, bulletTexture,  gun, rocketTexture);
            GameOverCheck();
            UpdateBullets(gameTime);
            UpdateExplosions(gameTime);

        }
        public void UpdateGameOverScreen()
        {
           if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed )
           {
               currentGameScreen = GameScreen.MainMenu;
               ResetGame();
           }
            
            
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
        public void MenuSelect(GameTime gameTime, PlayerIndex num)
        {
            if(1 <= currentMenuItem && currentMenuItem <= menuButtons.Count)
            {

                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                 if (elapsedTime > menuTime)
                {
                    currentMenuItem -= (int)(GamePad.GetState(num).ThumbSticks.Left.Y);
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
            MenuSelect(gameTime, PlayerIndex.One);
            foreach (Button button in menuButtons)
            {
                button.Update(gameTime,mouseState, currentMenuItem);
                if(button.CheckForClick(mouseState))
                {

                    if(button.buttonName =="play")
                    {
                        currentGameScreen = GameScreen.SelectScreen;
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
            if (currentGameScreen == GameScreen.SelectScreen)
            {
                UpdateSelectScreen(gameTime);
            }
            if (currentGameScreen == GameScreen.GameRunning)
            {
                UpdateGame(gameTime);
            }
            if (currentGameScreen == GameScreen.MainMenu)
            {
                UpdateMenu(gameTime);
            }
            if (currentGameScreen == GameScreen.GameOver)
            {
                UpdateGameOverScreen();
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
            spriteBatch.Draw(backgroundGame, new Vector2(0, 0), Color.White);
           
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
            DrawPowerups(spriteBatch);
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
        public void DrawGameOver()
        {
            spriteBatch.Draw(backgroundGame, new Vector2(0, 0), Color.White);
           
            spriteBatch.Draw(winningTex, new Vector2((1920 - winningTex.Width) / 2, (1080 - winningTex.Height) / 2), Color.Green);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (currentGameScreen == GameScreen.GameRunning)
            {
                DrawGame(gameTime);
            }
            if (currentGameScreen == GameScreen.SelectScreen)
            {
                spriteBatch.Draw(backgroundGame, new Vector2(0, 0), Color.White);
                DrawIcons(spriteBatch);
            }
            if (currentGameScreen == GameScreen.MainMenu)
            {
                DrawMenu(gameTime);
            }
            if (currentGameScreen == GameScreen.GameOver)
            {
                DrawGameOver();
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
