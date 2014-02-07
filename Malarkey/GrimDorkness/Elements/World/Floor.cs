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

        int[,] textures;
        int[,] brightness;

//        string[,] names = new string[5, 4];

        public Floor(Texture2D texture, int newTileWidth, int newTileHeight)
        {
            textures = new int[32, 32];
            brightness = new int[32, 32];

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    textures[x, y] = x % 4 + 1;
                    brightness[x, y] = (x + 2) * 16;

                }
            }
            
            tileWidth = newTileWidth;
            tileHeight = newTileHeight;

            Rectangle textureRect = new Rectangle(0, 0, tileWidth, tileHeight);

            sprite = new Sprite(texture, textureRect, 2.0);

        }


        public override void Draw(GameTime gameTime)
        {
            sprite.UpdateRect(new Rectangle(32, 0, 32, 32));

            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    int spriteNumber = textures[x, y];
                    int spriteY = spriteNumber / spritesPerRow;
                    int spriteX = spriteNumber % spritesPerRow;
                    sprite.UpdateRect(new Rectangle(tileWidth * spriteX, tileHeight * spriteY, tileWidth, tileHeight));

                    Color tint = new Color(brightness[x, y], brightness[x, y], brightness[x, y], 255);

                    sprite.Draw(new Vector2(64 * x, 64 * y), SpriteEffects.None, tint);
                }
            }

            sprite.UpdateRect(new Rectangle(32, 0, 32, 32));
            sprite.Draw(new Vector2(0.0f, 0.0f), SpriteEffects.None);
        }
    }
}
