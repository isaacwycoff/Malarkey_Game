using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Malarkey
{
    class DialogPortrait:HUDElement
    {

        public DialogPortrait(Texture2D texture)
        {
            position = new Vector2(100.0f, 100.0f);         // FIXME

            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);

            sprite = new AnimatedSprite(texture, sourceRect, 2.0);
        }

    }
}
