﻿using System;
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

    class Entity: Element
    {
        // common variables for all entities:
        protected int health;
        protected int maxHealth;

        protected int shield;
        protected int maxShield;

        protected int damage;
        protected double speed;
        protected EntityState state;

        // big cooords- what X and Y in the grid?
        public double mapX { get; protected set; }
        public double mapY { get; protected set; }

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

            switch (direction)
            {
                case Direction.East:
                    mapX += speed * timeScale / 128.0;
                    break;
                case Direction.North:
                    mapY -= speed * timeScale / 128.0;
                    break;
                case Direction.NorthEast:
                    mapY -= (speed * timeScale / 128.0) * COS_45;
                    mapX += (speed * timeScale / 128.0) * COS_45;
                    break;
                case Direction.NorthWest:
                    break;
                case Direction.West:
                    mapX -= speed * timeScale / 128.0;
                    break;
                case Direction.South:
                    mapY += speed * timeScale / 128.0;
                    break;
                case Direction.SouthEast:
                    mapY += (speed * timeScale / 128.0) * COS_45;
                    mapX += (speed * timeScale / 128.0) * COS_45;
                    break;
                case Direction.SouthWest:
                    mapY += (speed * timeScale / 128.0) * COS_45;
                    mapX -= (speed * timeScale / 128.0) * COS_45;
                    break;
                default:
                    // error
                    break;
            }
            // check bounds --
            // should also check collisions?
            if (mapX <= 0.0) mapX = 0.0;
            if (mapY <= 0.0) mapY = 0.0;
            if (mapX >= 16.0) mapX = 16.0;
            if (mapY >= 16.0) mapY = 16.0;

        }


        virtual public bool Collision()
        {
            if (markedForDeath || health <= 0) return false;

            return true;
        }



    }
}
