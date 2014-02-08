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

        public Hero(Texture2D texture)
        {

            animIdleSouth = new List<Rectangle>
            {
                new Rectangle(234, 738, 66, 53)



            };

            // set up the rectangles for the ship's animation:
            // ideally this would be stored in an external file
/*            animFly = new List<Rectangle>
            {
                new Rectangle(0, 140, 23, 29),                  // flying far left
                new Rectangle(24, 140, 23, 29),                 // flying sort-of left
                new Rectangle(48, 140, 23, 29),                 // flying straight
                new Rectangle(72, 140, 23, 29),                 // flying sort-of right
                new Rectangle(96, 140, 23, 29),                 // flying far right
            }; */

            // these should be defined in an external file:
            health = 100;
            maxHealth = health;
            shield = 100;
            maxShield = shield;
            damage = 10;
            xPos = 400;
            yPos = 500;
            speed = 5;
            state = 0x00;

            team = Team.Player;

            sprite = new Sprite(texture, new Rectangle(234, 738, 66, 53), 1.0);

        }

        public override int GetMaxHealth()
        {
            return maxHealth;
        }

        // the user has issued a command
        // the ship will attempt to comply
//        public void SendCommand(PlayerCommand command, GameTime gameTime)
        public void SendCommand(HeroCommand command, GameTime gameTime)         // this probably needs to work differently
                                                                                // to deal with mouse & gamepad
        {
            float timeScale = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeScale = 1.0f;

            // default animation -- may get overwritten later in SendCommand:
//            currentAnim = ShipAnim.FlyStraight;

            switch (command)
            {
                case HeroCommand.MoveNorth:
                    {
                        break;
                    }
                case HeroCommand.MoveEast:
                    {
                        break;
                    }
                case HeroCommand.MoveSouth:
                    {
                        break;
                    }
                case HeroCommand.MoveWest:
                    {
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
                        yPos -= (int)(speed * timeScale);
                        if (yPos <= 0) yPos = 0;

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
            sprite.Draw(new Vector2(xPos, yPos), SpriteEffects.None);


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
