using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Malarkey
{
    class ElementManager
    {
        private static ElementManager manager = new ElementManager();

        public static ElementManager GetInstance()
        {
            return manager;
        }

        Hero playerHero;

        TextureManager textureManager = TextureManager.GetInstance();

        List<Entity> listOfEntities;
        List<Entity> listOfExplosions;
        List<Entity> listOfPowerUps;

        // entities marked for deletion. we may not need this.
        List<Entity> entitiesToDelete;

        // new entities get temporarily put in here:
        List<Entity> newEntities;

        private ElementManager()
        {
            listOfEntities = new List<Entity>();
            listOfExplosions = new List<Entity>();
            listOfPowerUps = new List<Entity>();
            entitiesToDelete = new List<Entity>();
            newEntities = new List<Entity>();

            /*
            worldFloor = new Floor(textureManager.GetTexture("TILE_JUNGLE"), Floor.DEFAULT_TILE_SIZE, Floor.DEFAULT_TILE_SIZE);

            // create the full-screen fader for fading in and out (how cinematic!)
            fader = new Fader(textureManager.GetTexture("BLACK_PIXEL"), fullScreen);

            fader.fadeIn(Fader.DEFAULT_FADE_SHIFT);

            playerHero = new Hero(textureManager.GetTexture("KNIGHT_SWORD"), playerCamera);
            listOfEntities.Add(playerHero);
            */

        }

        public void AddEntity(int id, double x, double y) {

            // switch based on id and add it

        }

        public Hero AddHero(int id, double x, double y, Camera camera) {
            // add a new player unit and tie it to the specified camera

            String textureString = "KNIGHT_SWORD";            // hard-coded right now. will be determined from a json file later.

            playerHero = new Hero(textureManager.GetTexture(textureString), camera);
            listOfEntities.Add(playerHero);

            return playerHero;
        }

        public void SendPlayerCommand(HeroCommand command, GameTime gameTime) {
            // FIXME: assumes the player exists
            playerHero.SendCommand(command, gameTime);
        }

        public void Draw(GameTime gameTime)
        {
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

        }

        public void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            foreach (Entity tmpEntity in newEntities)
            {
                listOfEntities.Add(tmpEntity);
            }

            newEntities.RemoveRange(0, newEntities.Count());

/*            // parse player's keypresses
            ParseInput(gameTime);
            */

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
        }

        public void CleanUp()
        {
            // remove any entities that have been marked for death:
            // have to do it this way, or the foreach code above will freak out

            foreach (Entity tmpEntity in entitiesToDelete)
            {
                listOfEntities.Remove(tmpEntity);
            }
        }

        private void ResolveCollisions(GameTime gameTime)
        {

            Rectangle MAP_BOUNDS = new Rectangle(0, 0, 16, 16);

            /*
            if (mapX <= 0.0) mapX = 0.0;
            if (mapY <= 0.0) mapY = 0.0;
            if (mapX >= 16.0) mapX = 16.0;
            if (mapY >= 16.0) mapY = 16.0;
            */

            foreach (Entity currentEntity in listOfEntities)
            {
                if (currentEntity.isAttemptingToMove)
                {
                    double attemptedX = currentEntity.attemptedMapX;
                    double attemptedY = currentEntity.attemptedMapY;

                    if (attemptedY > MAP_BOUNDS.Bottom) attemptedY = MAP_BOUNDS.Bottom;
                    if (attemptedY < MAP_BOUNDS.Top) attemptedY = MAP_BOUNDS.Top;
                    if (attemptedX > MAP_BOUNDS.Right) attemptedX = MAP_BOUNDS.Right;
                    if (attemptedX < MAP_BOUNDS.Left) attemptedX = MAP_BOUNDS.Left;

                    currentEntity.setMapCoords(attemptedX, attemptedY);
                }
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

                            // randomize the pitch of the explosion
                            // TODO: put this in a separate class
/*                            float pitchShift = -0.1f * (float)randomizer.Next(1, 5) - 0.5f;

                            explosion1.Play(0.20f, pitchShift, 0.0f); */
                        }
                    }
                }

                // power-up collisions:
                foreach (Entity tmpPowerUp in listOfPowerUps)
                {

                }

            } // end collision detection

        }

    }
}
