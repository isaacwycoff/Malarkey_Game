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
        // consts
        const int MAX_ENEMIES = 16;
        const int SCREEN_WIDTH = 800;
        const int SCREEN_HEIGHT = 600;

        enum GameStatus
        {
            startMenu,
            game,
            dead,
            pauseMenu,
            restart,
            exit,
            shmUp               // secret shoot-em-up level
        };

        Fader fader;


        const int ZEPPELIN_FREQUENCY = 100;     // FIXME: this should be controlled elsewhere

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
        PlayerShip player;
        Hero playerHero;


        List<Entity> listOfEntities;
        List<Entity> listOfExplosions;
        List<Entity> listOfPowerUps;

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

                if (player.CanFire())
                {
                    // FIXME: the player class should be dealing with this
                    player.SendCommand(PlayerCommand.Fire, gameTime);

                    // randomize the gun noises a bit so they don't all sound the same:
                    float randomPitch = 0.25f + (-0.1f * randomizer.Next(0, 5));
                    gunShot1.Play(0.25f, randomPitch, 0.0f);

                    this.AddNewBullet(BulletType.FireBall, Direction.North, Team.Player, player.GetBarrelPosition());

                    noInput = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                player.SendCommand(PlayerCommand.AltFire, gameTime);
                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.0)
            {
                player.SendCommand(PlayerCommand.FlyForward, gameTime);
                playerHero.SendCommand(HeroCommand.MoveNorth, gameTime);
                noInput = false;

            }



            if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.0)
            {
                playerHero.SendCommand(HeroCommand.MoveSouth, gameTime);
                player.SendCommand(PlayerCommand.FlyBackward, gameTime);
                noInput = false;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.0)
            {
                playerHero.SendCommand(HeroCommand.MoveEast, gameTime);
                player.SendCommand(PlayerCommand.FlyRight, gameTime);

                noInput = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.0)
            {
                playerHero.SendCommand(HeroCommand.MoveWest, gameTime);
                player.SendCommand(PlayerCommand.FlyLeft, gameTime);

                noInput = false;
            }

            // no input given -- go idle
            if (noInput)
            {
                player.SendCommand(PlayerCommand.Idle, gameTime);
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

            // create our player's ship and assign it:
            player = new PlayerShip(myTexture);

            listOfEntities.Add(player);         // add the player entity to our global list of entities


            playerHero = new Hero(knightSwordTexture);

            listOfEntities.Add(playerHero);

            hudHealthBar = new HealthBar(healthTickTexture, player.GetHealth(), player.GetMaxHealth());

            PowerUp tmpPowerUp = new PowerUp(powerUpsTexture, PowerUpType.repair, new Vector2(400, 100));

            listOfPowerUps.Add(tmpPowerUp);

        }

        // create a new bullet!
        // called by entities when they want to shoot shit
        public void AddNewBullet(BulletType bulletType, Direction direction, Team team, Vector2 position)
        {
            Bullet tmpBullet = new Bullet(projectileTexture, bulletType, direction, team, position);

            newEntities.Add(tmpBullet);

            // play the sound effect!
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

            this.UpdateShmup(gameTime);

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
                case GameStatus.shmUp:
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // begin drawing - XNA 4.0 code -
            spriteBatch.Begin(SpriteSortMode.Deferred,          // TODO: Research
                            BlendState.AlphaBlend,              // blend alphas - i.e., transparencies
                            SamplerState.PointClamp,            // turn off magnification blurring
                            DepthStencilState.Default,          //
                            RasterizerState.CullNone);          // TODO: Research

            // FIXME: create separate cloud-overlay class
            // begin janky cloud-drawing code: (should put this off in its own class)

            Rectangle tmpBackgroundRect = new Rectangle(0, cloudsOffset, SCREEN_WIDTH, SCREEN_HEIGHT);

            spriteBatch.Draw(cloudsTexture, tmpBackgroundRect, Color.White);

            tmpBackgroundRect = new Rectangle(0, cloudsOffset - SCREEN_HEIGHT, SCREEN_WIDTH, SCREEN_HEIGHT);

            spriteBatch.Draw(cloudsTexture, tmpBackgroundRect, Color.White);

            cloudsOffset += 1;
            if (cloudsOffset > 600) cloudsOffset -= SCREEN_HEIGHT;

            // end janky cloud-drawing code

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


            // FIXME: this should be in its own crowd-drawing class
            // foreground clouds -- janky as fuck --- TODO: put this in a separate class

            Rectangle tmpForegroundRect = new Rectangle(0, fastCloudsOffset, SCREEN_WIDTH, SCREEN_HEIGHT);

            // FIXME: don't redefine this every frame! that is DUMB.
            Color transparentColor = new Color(0.25f, 0.25f, 0.25f, 0.25f);         // XNA 4.0 version

            spriteBatch.Draw(cloudsTexture, tmpForegroundRect, transparentColor);

            tmpForegroundRect = new Rectangle(0, fastCloudsOffset - SCREEN_HEIGHT, SCREEN_WIDTH, SCREEN_HEIGHT);

            spriteBatch.Draw(cloudsTexture, tmpForegroundRect, transparentColor);

            fastCloudsOffset += 2;
            if (fastCloudsOffset > SCREEN_HEIGHT) fastCloudsOffset -= SCREEN_HEIGHT;


            // debugging text -- FIXME: put this in its own function
            string output = "";

            // Draw debugging text:
            if (gameState == GameStatus.game)
            {
 //               GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed

                GamePadThumbSticks sticks = GamePad.GetState(PlayerIndex.One).ThumbSticks;

                output = "L(X: " + sticks.Left.X + ", Y: " + sticks.Left.Y + ") R(X: " + sticks.Right.X + ", Y: " + sticks.Right.Y + ")";

//                output = "Number of power ups: " + listOfPowerUps.Count();
            }
            else
            {
                output = "Welcome to Malarkey and Bunkum.\nNewline Test.";

            }

            // Find the center of the string
            Vector2 FontOrigin = devFont.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(devFont, output, new Vector2(21, 501), Color.DarkBlue);          // y 571

            spriteBatch.DrawString(devFont, output, new Vector2(20, 500), Color.WhiteSmoke);        // y 570

            hudHealthBar.DrawHealthTicks(spriteBatch, player.GetHealth(), player.GetMaxHealth());

            // draw overlays:

            switch (gameState)
            {
                case GameStatus.startMenu:
                    {

                        
                        spriteBatch.Draw(mainMenuTexture, fullScreen, Color.White);

                        Rectangle knightRect = new Rectangle(0, 0, 1024, 1024);

                        spriteBatch.Draw(knightSwordTexture, knightRect, Color.White);


                        break;
                    }
                case GameStatus.dead:
                    {
                        spriteBatch.Draw(deathScreenTexture, fullScreen, Color.White);

                        string finalScore = "You took " + totalKills + " souls with you.";

                        // Find the center of the string
                        int scoreXPos = 400 - ((int)devFont.MeasureString(finalScore).X / 2);

                        // Draw the string
                        spriteBatch.DrawString(devFont, finalScore, new Vector2(scoreXPos, 561), Color.DarkBlue);

                        spriteBatch.DrawString(devFont, finalScore, new Vector2(scoreXPos, 560), Color.WhiteSmoke);

                        break;
                    }
                default:
                    {

                        break;
                    }


            }

            fader.Draw(gameTime);      // fader overlay for fading in and out

            // end drawing:
            spriteBatch.End();

            base.Draw(gameTime);
        }


        void UpdateShmup(GameTime gameTime)
        {


            fader.Update(gameTime);

            if (gameState == GameStatus.startMenu)
            {
                ParseInput(gameTime);           // fixme
                return;
            }

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

            // this code should be off in its own method
            // add more planes:
            if (numberOfEnemies < MAX_ENEMIES && randomizer.Next(10) < 1)
            {

                Vector2 tmpVector = new Vector2(randomizer.Next(10, 790), -50);     // FIXME: get rid of magic numbers

                ShipType tmpShipType = (ShipType)randomizer.Next(0, 3);

                AIType tmpAIType = (AIType)randomizer.Next(0, 4);

                // FIXME: this should be controlled differently
                if (enemiesPassed % ZEPPELIN_FREQUENCY == (ZEPPELIN_FREQUENCY - 1))
                {
                    ZeppelinBoss tmpZepp = new ZeppelinBoss(zeppelinTexture, new Vector2(400, -250), this);
                    listOfEntities.Add(tmpZepp);
                }
                else
                {

                    EnemyShip tmpEnemyShip = new EnemyShip(enemyTexture, tmpShipType, tmpAIType, tmpVector, this);
                    listOfEntities.Add(tmpEnemyShip);
                }
                ++numberOfEnemies;
                ++enemiesPassed;


            }

            // parse player's keypresses
            ParseInput(gameTime);

            List<Entity> entitiesToDelete = new List<Entity>();

            // go thru each entity and update its AI:
            foreach (Entity tmpEntity in listOfEntities)
            {
                tmpEntity.Update(gameTime);

                // if an entity is off the screen, mark it for death!
                if (!tmpEntity.isVisible()) entitiesToDelete.Add(tmpEntity);
            }

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
                                    Entity tmpPowerUp = new PowerUp(powerUpsTexture, PowerUpType.repair, compareEntity1.CurrentPosition());
                                    listOfPowerUps.Add(tmpPowerUp);

                                }

                            }

                            // randomize the pitch of the explosion
                            // TODO: put this in a separate class
                            float pitchShift = -0.1f * (float)randomizer.Next(1, 5) - 0.5f;

                            explosion1.Play(0.20f, pitchShift, 0.0f);

                            Vector2 tmpPosition = listOfEntities[compare_index2].CurrentPosition();

                            // create the physical explosion:
                            Explosion tmpExplosion = new Explosion(explosionsTexture, ExplosionType.Basic, tmpPosition);

                            listOfExplosions.Add(tmpExplosion);
                        }
                    }
                }

                // power-up collisions:
                foreach (Entity tmpPowerUp in listOfPowerUps)
                {
                    if (player.ScreenRect().Intersects(tmpPowerUp.ScreenRect()))
                    {
                        tmpPowerUp.Destroy();

                        // FIXME: this should be dealt with by the the powerup + player
                        player.Repair(1);

                    }
                }

                // FIXME: this is messy! should probably be split off
                if (player.GetHealth() <= 0 && gameState == GameStatus.game)
                {
                    gameState = GameStatus.dead;

                    /*                    MediaPlayer.IsRepeating = false;
                                        MediaPlayer.Play(Content.Load<Song>("Music/taps"));
                                        MediaPlayer.Volume = 1.0f; */

                }
            }
            // end collision detection

            // remove any entities that have been marked for death:
            // have to do it this way, or the foreach code above will freak out
            foreach (Entity tmpEntity in entitiesToDelete)
            {
                listOfEntities.Remove(tmpEntity);
                if (tmpEntity is EnemyShip) --numberOfEnemies;
            }

            // power-ups next
            // update & delete ones that are finished       --- TODO: put this all in a separate function that gets called repeatedly
            List<Entity> powerUpsToDelete = new List<Entity>();

            foreach (Entity tmpPowerUp in listOfPowerUps)
            {
                tmpPowerUp.Update(gameTime);
                if (!tmpPowerUp.isVisible()) powerUpsToDelete.Add(tmpPowerUp);
            }

            foreach (Entity tmpPowerUp in powerUpsToDelete)
            {
                listOfPowerUps.Remove(tmpPowerUp);
            }


            // explosions now!
            // update and delete ones that are finished
            List<Entity> explosionsToDelete = new List<Entity>();

            foreach (Entity tmpExplosion in listOfExplosions)
            {
                tmpExplosion.Update(gameTime);
                if (!tmpExplosion.isVisible()) explosionsToDelete.Add(tmpExplosion);
            }

            foreach (Entity tmpExplosion in explosionsToDelete)
            {
                listOfExplosions.Remove(tmpExplosion);
            }

        }

    }


}
