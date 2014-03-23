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
    class Level
    {
        String levelName;

        Tile[,] tiles;

        int height, width;

        public void DrawGround()
        {


        }

        public Level()
        {
            // TODO: these will be determined by the level we're loading in the future
            height = 64;
            width = 64;

            levelName = "Testing.";

            tiles = new Tile[width, height];

            // initialize tiles:
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        tiles[x, y].SetTile(1, false);
                    }
                    else
                    {
                        tiles[x, y].SetTile(0, true);
                    }
                }
            }

        }

        public Boolean isTileEmpty(int x, int y)
        {
            // check against bounds of the level:
            if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return false;
            }

            // then check against the actual tile:
            return tiles[x, y].isEmpty;
            
        }

        public String name()
        {
            return levelName;
        }

    }
}
