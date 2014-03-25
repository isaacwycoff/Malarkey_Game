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
    class Floor: Element
    {
        public const int DEFAULT_TILE_SIZE = 32;

        public const int DEFAULT_SPRITES_PER_ROW = 16;

        int spritesPerRow = DEFAULT_SPRITES_PER_ROW;
        int tileWidth = DEFAULT_TILE_SIZE;
        int tileHeight = DEFAULT_TILE_SIZE;

        int mapWidth = 64;
        int mapHeight = 64;

        int[,] textures;
        int[,] brightness;

        Camera camera;

        public Floor(Texture2D texture, int tileWidth, int tileHeight, Camera camera)
        {
            textures = new int[mapWidth, mapHeight];
            brightness = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    textures[x, y] = x % 4 + 1;
                    brightness[x, y] = (x + 2) * 16;

                }
            }
            
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.camera = camera;

            Rectangle textureRect = new Rectangle(0, 0, this.tileWidth, this.tileHeight);

            sprite = new Sprite(texture, textureRect, 2.0);

        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }



        public override void Draw(GameTime gameTime)
        {
            // TODO: if camera hasn't been set, error out

            // sprite.UpdateRect(new Rectangle(32, 0, 32, 32));

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    int spriteNumber = textures[x, y];
                    int spriteY = spriteNumber / spritesPerRow;
                    int spriteX = spriteNumber % spritesPerRow;
                    sprite.UpdateRect(new Rectangle(tileWidth * spriteX, tileHeight * spriteY, tileWidth, tileHeight));

                    Color tint = new Color(brightness[x, y], brightness[x, y], brightness[x, y], 255);

                    // int screenX = (int)((mapX - camera.mapX) * TILE_SIZE);
                    // int screenY = (int)((mapY - camera.mapY) * TILE_SIZE);

                    // FIXME: these calculations are all fucked up

                    int screenX = (int)(64 * (x - (camera.mapX / 2)));
                    int screenY = (int)(64 * (y - (camera.mapY / 2)));

                    sprite.Draw(new Vector2(screenX, screenY), SpriteEffects.None, tint);
                }
            }

            sprite.UpdateRect(new Rectangle(32, 0, 32, 32));
            sprite.Draw(new Vector2(0.0f, 0.0f), SpriteEffects.None);
        }
    }
}
