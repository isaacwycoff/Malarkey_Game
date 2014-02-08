using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malarkey
{
    class Camera
    {
        // map coords of upper left corner
        public int mapX { get; private set; }
        public int mapY { get; private set; }
        // tile coords of upper left corner (what part of the tile do we start on?
        public float tileX { get; private set; }
        public float tileY { get; private set; }
        // size in tiles, or fractions of tiles:
        public float sizeX { get; private set; }
        public float sizeY { get; private set; }

        private Entity focusEntity;

        public Camera()
        {
            this.mapX = 0;
            this.mapY = 0;
            this.tileX = 0.0f;
            this.tileY = 0.0f;
            this.sizeX = 32.0f;
            this.sizeY = 16.0f;
            this.focusEntity = null;
        }

        public Camera(Entity focus)
        {
            this.focusEntity = focus;

            // FIXME: this should center around the focus
            this.mapX = 0;
            this.mapY = 0;
            this.tileX = 0.0f;
            this.tileY = 0.0f;
            this.sizeX = 32.0f;
            this.sizeY = 16.0f;            
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

            this.tileX = focusEntity.tileX;
            this.tileY = focusEntity.tileY;

        }


    }
}
