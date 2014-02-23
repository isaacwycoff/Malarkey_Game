using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Malarkey
{
    class Tile: Element
    {
        int texIndex = 0;

        public Boolean isEmpty { get; private set; }

        public Tile()
        {
            isEmpty = false;
        }

        public void SetTile(int texture, Boolean empty)
        // initialize values for this tile
        // we do this here because we bulk-init all the tiles in a level, without any variables
        {
            this.texIndex = texture;
            this.isEmpty = empty;
        }



    }
}
