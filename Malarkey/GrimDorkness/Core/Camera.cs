using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malarkey
{
    class Camera
    {
        // map coords of upper left corner, in tiles
        public double mapX { get; private set; }
        public double mapY { get; private set; }
        // size in tiles, or fractions of tiles:
        public double sizeX { get; private set; }
        public double sizeY { get; private set; }

        private Entity focusEntity;

        public Camera()
        {
            this.mapX = 0;
            this.mapY = 0;
            this.sizeX = 32.0f;
            this.sizeY = 16.0f;
            this.focusEntity = null;
        }

        public Camera(Entity focus)
        {
            this.focusEntity = focus;

            // FIXME: this should center around the focus
            this.mapX = 0.0;
            this.mapY = 0.0;
            this.sizeX = 32.0;
            this.sizeY = 16.0;            
        }

        public Camera(int mapX = 0, int mapY = 0, float tileX = 0.0f, float tileY = 0.0f, float sizeX = 32.0f, float sizeY = 16.0f)
        {

        }

        public void Update()
        {
            // updates the camera based on where the focusEntity is
            if (focusEntity == null) return;        // error silently

            const int HALFSCREEN_X = 8;
            const int HALFSCREEN_Y = 6;

            this.mapX = focusEntity.mapX - HALFSCREEN_X;
            this.mapY = focusEntity.mapY - HALFSCREEN_Y;

        }


    }
}
