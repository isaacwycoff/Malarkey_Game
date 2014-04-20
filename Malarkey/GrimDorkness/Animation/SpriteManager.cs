using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Malarkey
{
    class SpriteManager
    {
        private static SpriteManager manager = new SpriteManager();

        TextureManager textureManager = TextureManager.GetInstance();

        List<AnimatedSprite> sprites;           // might want to implement this as a HashTable, not sure yet.

        enum SpriteIDs {
            BLACK_PIXEL = 1,
            KNIGHT_SWORD = 2            
        }

        private SpriteManager()
        {
            sprites = new List<AnimatedSprite>();
        }

        public SpriteManager GetInstance()
        {
            return manager;
        }

        public SpriteInstance NewSpriteInstance(int spriteID)
        {
            foreach (AnimatedSprite sprite in sprites) {

                if (sprite.GetSpriteID() == spriteID)
                {
                    return new SpriteInstance(sprite);
                }
            }
            
            // this is very very temporary --
            // we want a generalized way to load in sprites from a JSON or XML file
            AnimatedSprite tmpSprite;

            switch((SpriteIDs)spriteID) {
                case SpriteIDs.BLACK_PIXEL:
                    tmpSprite = new AnimatedSprite(spriteID, textureManager.GetTexture("BLACK_PIXEL"), new Rectangle(0, 0, 1, 1), 1.0);
                    break;
                case SpriteIDs.KNIGHT_SWORD:
                    tmpSprite = new AnimatedSprite(spriteID, textureManager.GetTexture("KNIGHT_SWORD"), new Rectangle(234, 738, 66, 53), 1.0);
                    break;
                default:
                    return null;            // TODO: exception handling here
            }

            sprites.Add(tmpSprite);
            return new SpriteInstance(tmpSprite);
        }

    }
}
