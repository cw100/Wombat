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
                PlayerIndex playerIndex = new PlayerIndex();
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
            AddPlatform(new Vector2(100, 800));
            AddPlatform(new Vector2(150, 700));
            AddPlatform(new Vector2(500, 900));
            AddPlatform(new Vector2(360, 700));

            InitializePlayers(4);
        }
       public void InitializeMainMenu()
       {
           menuButtons = new List<Button>();
           
           Button button= new Button();
           button.Initialize(new Vector2(736 + 454 / 2, 169 + 145/2), "Play", "play", 1);
           menuButtons.Add(button);
           button = new Button();
           button.Initialize(new Vector2(736 + 454 / 2, 467 + 145 / 2), "Options", "options", 2);
           menuButtons.Add(button);
            button = new Button();
            button.Initialize(new Vector2(736 + 454 / 2, 748 + 145 / 2), "Exit", "exit", 3);
           menuButtons.Add(button);
       }


        protected override void Initialize()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / (float)VirtualScreenWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / (float)VirtualScreenHeight;

            screenScale = new Vector3(scaleX, scaleY, 1.0f);
            mouseState = new MouseState();
            InitializeGameScreen();

            InitializeMainMenu();
            base.Initialize();

        }

        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (Player player in players)
            {
                player.LoadContent(Content, "wombatPlaceholder");
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

        public void UpdateGame(GameTime gameTime)
        {
            foreach (Platform platform in platforms)

                platform.Update(gameTime);
            
            foreach (Player player in players)
                player.Update(gameTime, platformHitBoxes, collisionManager);

        }
        public void MenuSelect(GameTime gameTime)
        {
            if(1 <= currentMenuItem && currentMenuItem <= menuButtons.Count)
            {
               
                if(true)
                {
           currentMenuItem -= (int)(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y);
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
                spriteBatch.Draw(rectangleTexture, player.bigHitBox, Color.Green);


                player.Draw(spriteBatch);
            }
        }
        public void DrawMenu(GameTime gameTime)
        {
            foreach (Button button in menuButtons)
            {
                button.Draw(spriteBatch);
            }

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
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
