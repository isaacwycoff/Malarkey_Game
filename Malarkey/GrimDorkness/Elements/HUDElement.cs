using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Malarkey
{
    class HUDElement: Element
    {

        protected Vector2 position;

        protected Rectangle sourceRect;
        protected Rectangle destRect;

        public override void Draw(GameTime gameTime)
        {
            sprite.Draw(position, SpriteEffects.None);
        }

    }
}
