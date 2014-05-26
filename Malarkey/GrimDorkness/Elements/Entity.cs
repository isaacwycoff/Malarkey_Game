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
    // flags for different entity states
    [Flags]
    public enum EntityState
    {
        Attacking = 0x01,               // currently attacking
        Dying = 0x02,                   
        Invulnerable = 0x04,            
        Farting = 0x08,
        Dead = 0x10
    }

    public enum Team
    {
        Player = 1,
        Enemy = 2,
        PowerUp = 3
    }

    // FIXME: this should perhaps be somewhere central
    public enum Direction
    {
        South = 0,
        SouthEast = 1,
        East = 2,
        NorthEast = 3,
        North = 4,
        NorthWest = 5,
        West = 6,
        SouthWest = 7
    }

    class Entity: Element, IComparable<Entity>
    {
        // common variables for all entities:
        public int health { get; protected set; }
        public int maxHealth { get; protected set; }

        public int shield { get; protected set; }
        public int maxShield { get; protected set; }

        public AnimationID currentAnim { get; protected set; }

        protected int damage;
        protected double speed;
        protected EntityState state;

        // big cooords- what X and Y in the grid?
        public double mapX { get; protected set; }
        public double mapY { get; protected set; }

        public double attemptedMapX { get; protected set; }
        public double attemptedMapY { get; protected set; }
        public Boolean isAttemptingToMove { get; protected set; }

        public int CompareTo(Entity other)
        {
            // If other is not a valid object reference, this instance is greater. 
            if (other == null) return 1;

            // The temperature comparison depends on the comparison of  
            // the underlying Double values.  
            return screenY.CompareTo(other.screenY);
        }


        public void setMapCoords(double x, double y)
        {
            this.mapX = x;
            this.mapY = y;
            this.isAttemptingToMove = false;
        }

        // collision size:
        public double sizeX { get; protected set; }
        public double sizeY { get; protected set; }

        protected Team team;

        protected Camera camera;

        public Entity()
        {
            // base.
            camera = null;
            health = 1;
            maxHealth = health;
            shield = 0;
            maxShield = 0;

            attemptedMapX = mapX;
            attemptedMapY = mapY;
            isAttemptingToMove = false;
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        override public Rectangle ScreenRect()
        {
            UpdateScreenCoords();
            Rectangle tmpRect = new Rectangle(screenX, screenY, sprite.GetWidth(), sprite.GetHeight());
            return tmpRect;
        }

        override public Vector2 ScreenPosition()
        {
            UpdateScreenCoords();
            Vector2 tmpPos = new Vector2((float)screenX, (float)screenY);
            return tmpPos;
        }


        public void UpdateScreenCoords()
        {
            if (camera == null) return;     // error silently

            double TILE_SIZE = 32.0;

            screenX = (int)((mapX - camera.mapX) * TILE_SIZE);
            screenY = (int)((mapY - camera.mapY) * TILE_SIZE);

        }

        public override void Draw(GameTime gameTime)
        {
            this.UpdateScreenCoords();

            sprite.Draw(new Vector2(screenX, screenY), SpriteEffects.None);
        }


        virtual public int GetHealth()
        {
            return health;
        }

        virtual public int GetMaxHealth()
        {
            return maxHealth;
        }

        public Team GetTeam()
        {
            return team;
        }

        public override bool isVisible()
        {
            if (health <= 0) return false;          // TODO: instead make a DEAD flag

            return base.isVisible();
        }

        virtual public Boolean TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                return false;
            }
            return true;
        }

        protected void MoveDirection(Direction direction, float timeScale)
        {
            const double COS_45 = 0.7071;
            double timeMult = timeScale / 128.0;

            switch (direction)
            {
                case Direction.East:
                    this.attemptedMapY = mapY;
                    this.attemptedMapX = mapX + (speed * timeMult);

                    break;
                case Direction.North:
                    this.attemptedMapY = mapX;
                    this.attemptedMapY = mapY - (speed * timeMult);

                    break;
                case Direction.NorthEast:
                    this.attemptedMapY = mapY - (speed * timeMult * COS_45);
                    this.attemptedMapX = mapX + (speed * timeMult * COS_45);

                    break;
                case Direction.NorthWest:
                    this.attemptedMapY = mapY - (speed * timeMult * COS_45);
                    this.attemptedMapX = mapX - (speed * timeMult * COS_45);

                    break;
                case Direction.West:
                    this.attemptedMapX = mapX - (speed * timeMult);
                    this.attemptedMapY = mapY;

                    break;
                case Direction.South:
                    this.attemptedMapY = mapY + (speed * timeMult);
                    this.attemptedMapX = mapX;

                    break;
                case Direction.SouthEast:
                    this.attemptedMapY = mapY + (speed * timeMult * COS_45);
                    this.attemptedMapX = mapX + (speed * timeMult * COS_45);

                    break;
                case Direction.SouthWest:
                    this.attemptedMapY = mapY + (speed * timeMult * COS_45);
                    this.attemptedMapX = mapX - (speed * timeMult * COS_45);

                    break;
                default:
                    // error
                    break;
            }
            // check bounds --
            // should also check collisions?
            
            this.isAttemptingToMove = true;
        }


        virtual public bool Collision()
        {
            if (markedForDeath || health <= 0) return false;

            return true;
        }



    }
}
