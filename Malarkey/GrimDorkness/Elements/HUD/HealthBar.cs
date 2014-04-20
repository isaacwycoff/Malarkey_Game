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
    class HealthBar:HUDElement
    {
        Entity entity;          // what entity are we tracking?

        public HealthBar(Texture2D texture, Entity entity)
        {
            this.entity = entity;

            position = new Vector2(10.0f, 10.0f);           // FIXME - this should be resolution agnostic

            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);

            sprite = new AnimatedSprite(1, texture, sourceRect, 2.0);
        }

        public override void Draw(GameTime gameTime)
        {
            int numberOfTicks = (10 * entity.health) / entity.maxHealth;

            for (int currentTick = 0; currentTick < numberOfTicks; currentTick++)
            {
                Vector2 currentTickPosition = new Vector2(10.0f + (10.0f * (float)currentTick), 10);
                sprite.Draw(currentTickPosition, SpriteEffects.None);
            }            
        }

    }
}
