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
    /// <summary>
    /// Basic graphical element that all screen elements are derived from
    /// </summary>
    /// <remarks>
    /// Currently parent of HUDElement and Entity.
    /// </remarks>

    class Element
    {
        protected int screenX, screenY;           // screenpos in pixels
//        protected Sprite sprite;
        protected AnimatedSprite sprite;

        protected bool markedForDeath = false;    

        /// <summary>
        /// Gets the dimensions and location of the sprite
        /// </summary>
        /// <returns>
        /// Returns a Rectangle with dimensions & location of the sprite
        /// </returns>
        virtual public Rectangle ScreenRect()
        {
            Rectangle tmpRect = new Rectangle(screenX, screenY, sprite.GetWidth(), sprite.GetHeight());
            return tmpRect;
        }

        public Boolean IsMarkedForDeath()
        {
            return markedForDeath;
        }

        /// <summary>
        /// Draws the element to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        virtual public void Draw(GameTime gameTime)
        {
            //
        }

        /// <summary>
        /// Updates the element's AI
        /// </summary>
        /// <param name="gameTime"></param>
        virtual public void Update(GameTime gameTime)
        {
            // 
        }

        /// <summary>
        /// Returns current position in the form of a Vector2 object.
        /// </summary>
        /// <returns></returns>
        virtual public Vector2 ScreenPosition()
        {
            Vector2 tmpPos = new Vector2((float)screenX, (float)screenY);
            return tmpPos;
        }

        /// <summary>
        /// Is the object currently visible?
        /// </summary>
        /// <returns></returns>
        virtual public Boolean isVisible()
        {
            if (markedForDeath == true) return false;
            // FIXME: magic numbers attached to the size of the screen.
            if (screenX < -100) return false;
            if (screenY < -400) return false;
            if (screenX > 1000) return false;
            if (screenY > 800) return false;
            return true;
        }

        public void Destroy()
        {
            markedForDeath = true;
        }

    }

}
