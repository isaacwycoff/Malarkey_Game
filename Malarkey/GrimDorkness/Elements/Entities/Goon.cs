using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace Malarkey
{
    class Goon: Entity
    {

        List<Rectangle> animIdleSouth;

        public Goon(Texture2D texture, double x, double y, Camera camera)
        {
            this.setMapCoords(x, y);            // FIXME: this should NOT be set here

            this.UpdateScreenCoords();

            this.camera = camera;

            // these should be defined in an external file:
            this.health = 100;
            this.maxHealth = health;
            this.shield = 100;
            this.maxShield = shield;
            this.damage = 10;

            animIdleSouth = new List<Rectangle>
            {
                new Rectangle(234, 738, 66, 53)
            };


            sprite = new Sprite(texture, new Rectangle(234, 738, 66, 53), 1.0);
        }


        public override void Draw(GameTime gameTime)
        {

            // this.UpdateScreenCoords();


            sprite.UpdateRect(animIdleSouth[0]);
            // sprite.UpdateRect(animIdleNorth[0]);

            base.Draw(gameTime);

        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

    }
}
