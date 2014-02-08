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
    public enum HeroCommand
    {
        Idle = 0,
        MoveWest = 1,
        MoveEast = 2,
        MoveNorth = 3,
        MoveSouth = 4,
        AttackMelee = 5,
        AttackRanged = 6,
        DrinkPotion = 7
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

        public Hero(Texture2D texture, Camera camera)
        {
            this.camera = camera;

            animIdleSouth = new List<Rectangle>
            {
                new Rectangle(234, 738, 66, 53)
            };

            // these should be defined in an external file:
            this.health = 100;
            this.maxHealth = health;
            this.shield = 100;
            this.maxShield = shield;
            this.damage = 10;

            this.mapX = 6.0;
            this.mapY = 6.0;

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

            switch (command)
            {
                case HeroCommand.MoveNorth:
                    {
                        MoveDirection(Direction.North, timeScale);
//                        currentAnim =
                        break;
                    }
                case HeroCommand.MoveEast:
                    {
                        MoveDirection(Direction.East, timeScale);
                        break;
                    }
                case HeroCommand.MoveSouth:
                    {
                        MoveDirection(Direction.South, timeScale);
                        break;
                    }
                case HeroCommand.MoveWest:
                    {
                        MoveDirection(Direction.West, timeScale);
                        break;
                    }

/*                case PlayerCommand.Idle:
                    {
                        // no commands from the user
                        break;
                    }
                case PlayerCommand.FlyLeft:
                    {
                        xPos -= (int)(speed * timeScale);
                        if (xPos <= 0) xPos = 0;

//                        currentAnim = ShipAnim.FlyLeft;

                        break;
                    }
                case PlayerCommand.FlyRight:
                    {
                        xPos += (int)(speed * timeScale);
                        if (xPos >= 700) xPos = 700;


//                        currentAnim = ShipAnim.FlyRight;

                        break;
                    }
                case PlayerCommand.FlyForward:
                    {

                        break;
                    }
                case PlayerCommand.FlyBackward:
                    {
                        yPos += (int)(speed * timeScale);
                        if (yPos >= 500) yPos = 500;       // TODO: get rid of magic numbers


                        break;
                    }
                case PlayerCommand.Fire:
                    {
//                        fireCooldownRemaining = fireDelay;

                        break;
                    }
                case PlayerCommand.AltFire:
                    {

                        break;
                    } */
                default:
                    {

                        break;
                    }
            }


        }
        
        public override void Draw(GameTime gameTime)
        {

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
