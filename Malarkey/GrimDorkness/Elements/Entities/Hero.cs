using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Malarkey
{
    public enum HeroCommand
    {
        Idle = 0x0,
        MoveWest = 0x1,
        MoveEast = 0x2,
        MoveNorth = 0x4,
        MoveSouth = 0x8,
        AttackMelee = 0x10,
        AttackRanged = 0x20,
        DrinkPotion = 0x40
    }

    
    class Hero: Entity
    {

        List<Rectangle> animIdleSouth;
        List<Rectangle> animIdleWest;
        List<Rectangle> animIdleEast;
        List<Rectangle> animIdleNorth;

        List<Rectangle> animMoveSouth;
        List<Rectangle> animMoveWest;
        List<Rectangle> animMoveEast;
        List<Rectangle> animMoveNorth;

        public Hero(Texture2D texture, Camera camera, double x, double y)
        {
            this.camera = camera;
            // TODO: this is temporary

            camera.SetFocus(this);

            animIdleSouth = new List<Rectangle>
            {
                new Rectangle(234, 738, 66, 53)
            };

            animIdleNorth = new List<Rectangle>
            {
                new Rectangle(25, 395, 62, 62)
            };

            /*
            animIdleWest = new List<Rectangle>
            {
                new Rectangle(
            };
            */

            animMoveSouth = new List<Rectangle>
            {
                new Rectangle(30, 669, 66, 53),
                new Rectangle(96, 669, 66, 53),
                new Rectangle(160, 669, 66, 53)

            };
            

            // these should be defined in an external file:
            this.health = 100;
            this.maxHealth = health;
            this.shield = 100;
            this.maxShield = shield;
            this.damage = 10;

            this.setMapCoords(x, y);            // FIXME: this should NOT be set here

            this.UpdateScreenCoords();

            // speed is represented as 1/128th of a tile
            this.speed = 13.0;
            this.state = 0x00;

            team = Team.Player;

            sprite = new Sprite(texture, new Rectangle(234, 738, 66, 53), 1.0);
        }

        public override int GetMaxHealth()
        {
            return maxHealth;
        }

        // the user has issued a command
        // the ship will attempt to comply
        public void SendCommand(HeroCommand command, GameTime gameTime)         // this probably needs to work differently
                                                                                // to deal with mouse & gamepad
        {
            float timeScale = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeScale = 1.0f;

            // FIXME: this is super-ugly. is there a better way to deal with this?
            if (command.HasFlag(HeroCommand.MoveNorth))
            {
                if (command.HasFlag(HeroCommand.MoveEast))
                {
                    MoveDirection(Direction.NorthEast, timeScale);
                }
                else if (command.HasFlag(HeroCommand.MoveWest))
                {
                    MoveDirection(Direction.NorthWest, timeScale);
                }
                else if (command.HasFlag(HeroCommand.MoveSouth))
                {
                    // don't move!
                }
                else
                {
                    MoveDirection(Direction.North, timeScale);
                }
            }
            else if (command.HasFlag(HeroCommand.MoveSouth))
            {
                if (command.HasFlag(HeroCommand.MoveEast))
                {
                    MoveDirection(Direction.SouthEast, timeScale);
                }
                else if (command.HasFlag(HeroCommand.MoveWest))
                {
                    MoveDirection(Direction.SouthWest, timeScale);
                }
                else
                {
                    MoveDirection(Direction.South, timeScale);
                }
            }
            else if (command.HasFlag(HeroCommand.MoveWest))
            {
                MoveDirection(Direction.West, timeScale);
            }
            else if (command.HasFlag(HeroCommand.MoveEast))
            {
                MoveDirection(Direction.East, timeScale);
            }

        }
        
        public override void Draw(GameTime gameTime)
        {
            sprite.UpdateRect(animMoveSouth[1]);
            // sprite.UpdateRect(animIdleNorth[0]);

            base.Draw(gameTime);


            // FIXME: where do we deal with this animation stuff?
            // should be dealt with in the sprite, methinks

/*             animTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (animTimeElapsed >= animDelay)
            {
                animTimeElapsed = 0;

                if (currentAnim == ShipAnim.FlyLeft)
                {
                    --currentFrame;
                    if (currentFrame < FlyingAnimFrame.FarLeft) currentFrame = FlyingAnimFrame.FarLeft;
                }
                else if (currentAnim == ShipAnim.FlyRight)
                {
                    ++currentFrame;
                    if (currentFrame > FlyingAnimFrame.FarRight) currentFrame = FlyingAnimFrame.FarRight;
                }
                else
                {
                    // if we're not moving left or right, tilt back towards the center:
                    if (currentFrame > FlyingAnimFrame.Straight) --currentFrame;
                    else if (currentFrame < FlyingAnimFrame.Straight) ++currentFrame;
                } 

            } */

//            sprite.UpdateRect(animFly[(int)currentFrame]);


        }

        public override void Update(GameTime gameTime)
        {
            // manage behavior cooldowns -- delays on certain actions to prevent them from being spammed

/*            if (fireCooldownRemaining > 0)
            {
                fireCooldownRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
                if (fireCooldownRemaining < 0) fireCooldownRemaining = 0;
            } */

            base.Update(gameTime);
        }

/*        public bool CanFire()
        {
            if (fireCooldownRemaining <= 0.0f) return true;
            return false;
        } */

        public bool Repair(int damage)
        {
            if (health >= maxHealth) return false;

            health += damage;

            if (health > maxHealth) health = maxHealth;


            // TODO
            return true;
        }

        public bool Invulnerability(float time)
        {

            // TODO
            return true;
        }


    }
}
