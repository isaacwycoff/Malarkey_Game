using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Malarkey
{
    class AnimatedSprite: Sprite
    {
        List<Animation> animations;

        int spriteID;
        
        // Sprite sprite;
        public AnimatedSprite(int spriteID, Texture2D newTexture, Rectangle newRect, double newScale)
        {
            this.spriteID = spriteID;

            animations = new List<Animation>();

            scale = newScale;
            // set up width & height based on scale:
            width = (int)(newRect.Width * scale);
            height = (int)(newRect.Height * scale);
            texture = newTexture;
            sourceRect = newRect;
        }

        public int GetSpriteID()
        {
            return spriteID;
        }

    }
}
