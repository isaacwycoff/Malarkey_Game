using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Newtonsoft.Json.Linq;

namespace Malarkey
{

    /// <summary>
    /// GAME: Codename MALARKEY
    /// -----------------------
    /// 
    /// Stealth 'em Up
    /// 
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        // FIXME: these shouldn't be constant
        const int SCREEN_WIDTH = 1024;      // 800
        const int SCREEN_HEIGHT = 768;      // 600

        const Boolean IS_FULL_SCREEN = false;

        enum GameStatus
        {
            startMenu,
            game,
            dead,
            pauseMenu,
            restart,
            exit
        };

        Fader fader;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont devFont;

        GameTime aliveTime;

        SettingsManager settingsManager = SettingsManager.GetInstance();

        TextureManager textureManager = TextureManager.GetInstance();

        Floor worldFloor;
        
        ElementManager elementManager = ElementManager.GetInstance();

        // special reference to our player entity, so we don't have to search for it
        Hero playerHero;


        SoundEffect explosion1;
        SoundEffect gunShot1;
        SoundEffect engineLoop;

        Random randomizer;      // random number generator used throughout the class

        Rectangle fullScreen;   // rectangle used to determine the dimensions of the screen for drawing

        GameStatus gameState;

        HealthBar hudHealthBar;

        DialogPortrait tmpPortrait;


        Camera playerCamera;

        public Game()
        {
            Console.WriteLine("Tada");

            randomizer = new Random();

            gameState = GameStatus.startMenu;

            graphics = new GraphicsDeviceManager(this);

            this.graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            this.graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            this.graphics.IsFullScreen = IS_FULL_SCREEN;

            Content.RootDirectory = "Content";

            fullScreen = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);

            playerCamera = new Camera();        // FIXME - tie this to the player

        }

        // parse the user's input --
        // will be sent to a Player class or a UI class
        public void ParseInput(GameTime gameTime)
        {
            if (gameState == GameStatus.startMenu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gameState = GameStatus.game;
                    aliveTime = new GameTime();
                }
            }

            if (gameState == GameStatus.dead)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameStatus.restart;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                graphics.IsFullScreen = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                graphics.IsFullScreen = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameState = GameStatus.exit;
                Exit();
                return;
            }

            HeroCommand command = HeroCommand.Idle;

            // consider non-movement 1st, so movement states don't get over-written
            // incorrectly:
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
            {
                command = command | HeroCommand.AttackMelee;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                // noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.0)
            {
                command = command | HeroCommand.MoveNorth;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.0)
            {
                command = command | HeroCommand.MoveSouth;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.0)
            {
                command = command | HeroCommand.MoveEast;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.0)
            {                
                command = command | HeroCommand.MoveWest;
            }

            // actually, we might want to send an idel signal as well
            if (command != HeroCommand.Idle)
            {
                elementManager.SendPlayerCommand(command, gameTime);
                playerHero.SendCommand(command, gameTime);

            }

        }

    
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.InitClass(spriteBatch);

            textureManager.SetContentManager(Content);

            devFont = Content.Load<SpriteFont>("Fonts/devFont");

            // TODO: use this.Content to load your game content here            
            textureManager.AddTextures();

            explosion1 = Content.Load<SoundEffect>("Sfx/explosion1");
            gunShot1 = Content.Load<SoundEffect>("Sfx/magnum1");
            engineLoop = Content.Load<SoundEffect>("Sfx/engine_loop");

            // song to loop:
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Content.Load<Song>("Music/trim_loop2"));
            MediaPlayer.Volume = 0.0f;          // FIXME: get this from Settings        

            worldFloor = new Floor(textureManager.GetTexture("TILE_JUNGLE"), Floor.DEFAULT_TILE_SIZE, Floor.DEFAULT_TILE_SIZE, playerCamera);

            // create the full-screen fader for fading in and out (how cinematic!)
            fader = new Fader(textureManager.GetTexture("BLACK_PIXEL"), fullScreen);

            fader.fadeIn(Fader.DEFAULT_FADE_SHIFT);

            playerHero = elementManager.AddHero(0, 5.0, 5.0, playerCamera);
            elementManager.AddEntity(1, 2.0, 2.0, playerCamera);

            elementManager.AddEntity(1, 3.0, 7.6, playerCamera);

            hudHealthBar = new HealthBar(textureManager.GetTexture("HEALTH_TICK"), playerHero);
            tmpPortrait = new DialogPortrait(textureManager.GetTexture("PORTRAITS"));
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // SWITCH: what mode are we in?
            // should maybe instead just point to a different function

            this.UpdateGame(gameTime);

            switch (gameState)
            {
                case GameStatus.startMenu:
                {

                    break;
                }
                case GameStatus.pauseMenu:
                {

                    break;
                }
            }

            base.Update(gameTime);
        }
        // end Update()

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float timeScale = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeScale = 1.0f;

            // clear every cycle or we can get graphical artifacts
            // not strictly necessary as we tend to fill up the whole screen every frame
            // GraphicsDevice.Clear(Color.CornflowerBlue);

            // begin drawing - XNA 4.0 code -
            spriteBatch.Begin(SpriteSortMode.Deferred,          // TODO: Research
                            BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                            SamplerState.PointClamp,            // turn off magnification blurring
                            DepthStencilState.Default,          //
                            RasterizerState.CullNone);          // TODO: Research

            // draw the floor:
            worldFloor.Draw(gameTime);

            // draw elements:
            elementManager.Draw(gameTime);

            // draw clouds

            // draw UI

            // draw debugging overlay

            // debugging text -- FIXME: put this in its own function
            string output = "";

            // Find the center of the string
            Vector2 FontOrigin = devFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(devFont, output, new Vector2(21, 501), Color.DarkBlue);          // y 571
            spriteBatch.DrawString(devFont, output, new Vector2(20, 500), Color.WhiteSmoke);        // y 570
            hudHealthBar.Draw(gameTime);

            // tmpPortrait.Draw(gameTime);

            // draw overlays:
            switch (gameState)
            {
                case GameStatus.startMenu:
                    {                        
                        break;
                    }
                case GameStatus.dead:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            // draw fader overlay
            fader.Draw(gameTime);      // fader overlay for fading in and out

            // end drawing:
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateGame(GameTime gameTime)
        {
            fader.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            elementManager.Update(gameTime);

            // parse player's keypresses
            ParseInput(gameTime);

            playerCamera.Update();

            elementManager.CleanUp();
        }

    }


}
