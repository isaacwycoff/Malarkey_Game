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
        
        // Sprite sprite;
        public AnimatedSprite(Texture2D newTexture, Rectangle newRect, double newScale)
        {
            animations = new List<Animation>();

            scale = newScale;
            // set up width & height based on scale:
            width = (int)(newRect.Width * scale);
            height = (int)(newRect.Height * scale);
            texture = newTexture;
            sourceRect = newRect;
        }

    }
}
