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
        const int SCREEN_WIDTH = 800;
        const int SCREEN_HEIGHT = 600;

        enum GameStatus
        {
            startMenu,
            game,
            dead,
            pauseMenu,
            restart,
            exit
            // shmUp               // secret shoot-em-up level
        };

        Fader fader;


        // const int ZEPPELIN_FREQUENCY = 100;     // FIXME: this should be controlled elsewhere

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont devFont;

        GameTime aliveTime;

        // FIXME: cleanup this code -- rethink and reorganize
        int totalKills = 0;

        int numberOfEnemies = 0;

        int enemiesPassed = 0;

        // special reference to our player ship, so we don't have to search for it
        // every frame:
        // PlayerShip player;
        Hero playerHero;


        List<Entity> listOfEntities;
        List<Entity> listOfExplosions;
        List<Entity> listOfPowerUps;

        List<Entity> entitiesToDelete;

        // new entities get temporarily put in here:
        List<Entity> newEntities;

        // all of our textures:
        Texture2D myTexture;
        Texture2D enemyTexture;
        Texture2D projectileTexture;
        Texture2D cloudsTexture;
        Texture2D explosionsTexture;
        Texture2D zeppelinTexture;
        Texture2D blackPixelTexture;
        Texture2D mainMenuTexture;
        Texture2D deathScreenTexture;
        Texture2D healthTickTexture;
        Texture2D powerUpsTexture;
        Texture2D akimboGirlTexture;
        Texture2D knightSwordTexture;
        Texture2D jungleTexture;


        // list reference to our textures. not currently used:
        List<Texture2D> listOfTextures;


        SoundEffect explosion1;
        SoundEffect gunShot1;
        SoundEffect engineLoop;

        Random randomizer;      // random number generator used throughout the class

        Rectangle fullScreen;       // rectangle used to determine the dimensions of the screen for drawing

        GameStatus gameState;

        HealthBar hudHealthBar;

        Floor worldFloor;

        int cloudsOffset = 0;   // FIXME: for janky cloud code. should be in a separate class
        int fastCloudsOffset = 0;

        public Game()
        {
            randomizer = new Random();

            gameState = GameStatus.startMenu;

            graphics = new GraphicsDeviceManager(this);

            this.graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            this.graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            // set this later:
//            this.graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            fullScreen = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);



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

            bool noInput = true;

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

            // consider non-movement 1st, so movement states don't get over-written
            // incorrectly:
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
            {
                playerHero.SendCommand(HeroCommand.AttackMelee, gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                // player.SendCommand(PlayerCommand.AltFire, gameTime);
                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.0)
            {
                // player.SendCommand(PlayerCommand.FlyForward, gameTime);
                playerHero.SendCommand(HeroCommand.MoveNorth, gameTime);
                noInput = false;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.0)
            {
                playerHero.SendCommand(HeroCommand.MoveSouth, gameTime);
                noInput = false;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.0)
            {
                playerHero.SendCommand(HeroCommand.MoveEast, gameTime);

                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.0)
            {
                playerHero.SendCommand(HeroCommand.MoveWest, gameTime);

                noInput = false;
            }

            // no input given -- go idle
            if (noInput)
            {

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

            devFont = Content.Load<SpriteFont>("Fonts/devFont");

            // TODO: use this.Content to load your game content here

            myTexture = Content.Load<Texture2D>("Graphics/player_ship");
            enemyTexture = Content.Load<Texture2D>("Graphics/enemy_ship");
            projectileTexture = Content.Load<Texture2D>("Graphics/projectiles");
            cloudsTexture = Content.Load<Texture2D>("Graphics/clouds1");
            explosionsTexture = Content.Load<Texture2D>("Graphics/explosions");
            zeppelinTexture = Content.Load<Texture2D>("Graphics/zeppelin");
            blackPixelTexture = Content.Load<Texture2D>("Graphics/BlackPixel");
            mainMenuTexture = Content.Load<Texture2D>("Graphics/startScreen");
            deathScreenTexture = Content.Load<Texture2D>("Graphics/endScreen");      // FIXME
            healthTickTexture = Content.Load<Texture2D>("Graphics/healthTick");
            powerUpsTexture = Content.Load<Texture2D>("Graphics/powerups");
            akimboGirlTexture = Content.Load<Texture2D>("Graphics/akimbogirlstand");
            knightSwordTexture = Content.Load<Texture2D>("Graphics/knight_sword");
            jungleTexture = Content.Load<Texture2D>("Graphics/tile_jungle");

            explosion1 = Content.Load<SoundEffect>("Sfx/explosion1");
            gunShot1 = Content.Load<SoundEffect>("Sfx/magnum1");
            engineLoop = Content.Load<SoundEffect>("Sfx/engine_loop");

            // song to loop:
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Content.Load<Song>("Music/trim_loop2"));
            MediaPlayer.Volume = 0.0f;          // FIXME: get this from Settings        

            listOfEntities = new List<Entity>();

            listOfExplosions = new List<Entity>();

            listOfPowerUps = new List<Entity>();

            entitiesToDelete = new List<Entity>();

            newEntities = new List<Entity>();



            worldFloor = new Floor(jungleTexture, Floor.DEFAULT_TILE_SIZE, Floor.DEFAULT_TILE_SIZE);

            // this isn't necessary right now, but having a List later, esp. when file-names
            // are drawn from external data, will be very useful. probably want to have a separate
            // class to deal with texture data & names
            listOfTextures = new List<Texture2D> {
                myTexture,
                enemyTexture,
                projectileTexture,
                cloudsTexture,
                explosionsTexture,
                zeppelinTexture,
                blackPixelTexture,
                healthTickTexture,
                powerUpsTexture,
                akimboGirlTexture,
                knightSwordTexture
            };

            // create the full-screen fader for fading in and out (how cinematic!)
            fader = new Fader(blackPixelTexture, fullScreen);
            fader.fadeIn(Fader.DEFAULT_FADE_SHIFT);

            playerHero = new Hero(knightSwordTexture);

            listOfEntities.Add(playerHero);

            hudHealthBar = new HealthBar(healthTickTexture, playerHero.GetHealth(), playerHero.GetMaxHealth());
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

        // FIXME: might want to have a separate class for dealing with score
        public void addPoints(int points)
        {

            totalKills += points;
        }

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

            // new code for the top-down
            // should split off the old drawing code for the SHMUP
            worldFloor.Draw(gameTime);

            // spit out all the entities:
            foreach (Entity tmpEntity in listOfEntities)
            {
                tmpEntity.Draw(gameTime);
            }

            // spit out powerups
            foreach (Entity tmpPowerUp in listOfPowerUps)
            {
                tmpPowerUp.Draw(gameTime);
            }

            // now for explosions
            foreach (Entity tmpExplosion in listOfExplosions)
            {
                tmpExplosion.Draw(gameTime);
            }

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
            hudHealthBar.DrawHealthTicks(spriteBatch, playerHero.GetHealth(), playerHero.GetMaxHealth());

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

            // TODO: Add your update logic here
            foreach (Entity tmpEntity in newEntities)
            {
                listOfEntities.Add(tmpEntity);
            }

            newEntities.RemoveRange(0, newEntities.Count());

            // parse player's keypresses
            ParseInput(gameTime);

            // go thru each entity and update its AI:
            foreach (Entity tmpEntity in listOfEntities)
            {
                tmpEntity.Update(gameTime);

                // might not need to add this to a list - instead we might just cycle through the entities and delete them
                if (tmpEntity.IsMarkedForDeath()) entitiesToDelete.Add(tmpEntity);

                // if an entity is off the screen, mark it for death!
                // if (!tmpEntity.isVisible()) entitiesToDelete.Add(tmpEntity);
            }

            ResolveCollisions(gameTime);

            FrameCleanUp();


        }

        private void ResolveCollisions(GameTime gameTime)
        {

            // collision detection -- FIXME: this should be its own function at the very least.
            for (int compare_index1 = 0; compare_index1 < listOfEntities.Count; compare_index1++)
            {
                for (int compare_index2 = compare_index1 + 1; compare_index2 < listOfEntities.Count; compare_index2++)
                {
                    Entity compareEntity1 = listOfEntities[compare_index1];
                    Entity compareEntity2 = listOfEntities[compare_index2];

                    if (compareEntity1.Collision() &&
                       compareEntity2.Collision() &&
                       (compareEntity1.GetTeam() != compareEntity2.GetTeam())
                       )
                    {
                        Rectangle rect1 = compareEntity1.ScreenRect();
                        Rectangle rect2 = compareEntity2.ScreenRect();


                        if (rect1.Intersects(rect2))
                        {
                            // FIXME: this should take data from the individual ships
                            compareEntity1.TakeDamage(5);
                            compareEntity2.TakeDamage(5);

                            // FIXME
                            if (!compareEntity1.isVisible() || !compareEntity2.isVisible())
                            {
                                if (randomizer.Next(0, 10) < 1)
                                {
                                    // Entity tmpPowerUp = new PowerUp(powerUpsTexture, PowerUpType.repair, compareEntity1.CurrentPosition());
                                    // listOfPowerUps.Add(tmpPowerUp);

                                }

                            }

                            // randomize the pitch of the explosion
                            // TODO: put this in a separate class
                            float pitchShift = -0.1f * (float)randomizer.Next(1, 5) - 0.5f;

                            explosion1.Play(0.20f, pitchShift, 0.0f);

                            Vector2 tmpPosition = listOfEntities[compare_index2].CurrentPosition();
                        }
                    }
                }

                // power-up collisions:
                foreach (Entity tmpPowerUp in listOfPowerUps)
                {

                }

            } // end collision detection


        }

        private void FrameCleanUp()
        {
            // remove any entities that have been marked for death:
            // have to do it this way, or the foreach code above will freak out
            foreach (Entity tmpEntity in entitiesToDelete)
            {
                listOfEntities.Remove(tmpEntity);
                // if (tmpEntity is EnemyShip) --numberOfEnemies;
            }

        }



    }


}
