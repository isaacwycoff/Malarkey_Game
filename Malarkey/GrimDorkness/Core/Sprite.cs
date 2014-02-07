﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    class Sprite
    {
        static SpriteBatch spriteBatch;     // must be initialized before Draw can be used!

        Texture2D texture;          // reference to our texture
        Rectangle sourceRect;       // source rectangle from texture
        double scale;               // scaling - 1.0 is no scaling
        int width, height;          // destination dimensions

        public static void InitClass(SpriteBatch globalSpriteBatch)
        {
            spriteBatch = globalSpriteBatch;
        }

        // constructor
        public Sprite(Texture2D newTexture, Rectangle newRect, double newScale)
        {
            scale = newScale;
            // set up width & height based on scale:
            width = (int)(newRect.Width * scale);
            height = (int)(newRect.Height * scale);
            texture = newTexture;
            sourceRect = newRect;
        }

        public void UpdateRect(Rectangle newRect)
        {
            sourceRect = newRect;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }


        // get current scale - 1.0 is no scaling.
        public double getScale()
        {
            return scale;
        }

        // sets scale and updates width & height:
        public void setScale(double newScale)
        {
            scale = newScale;
            width = (int)(sourceRect.Width * scale);
            height = (int)(sourceRect.Height * scale);
        }

        public void setDimensions(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
        }

        // Draw (draws to screen) - overloaded a few times

        // Maximum Options Version
        public void Draw(Vector2 position, SpriteEffects spriteEffects, Color tint)
        {
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, width, height);

            spriteBatch.Draw(texture, destRect, sourceRect, tint);
        }

        public void Draw(Vector2 position, SpriteEffects spriteEffects)
        {
            this.Draw(position, spriteEffects, Color.White);
        }

    }
}
