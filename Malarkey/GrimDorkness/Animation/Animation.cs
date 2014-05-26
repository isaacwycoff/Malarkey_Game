using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Malarkey
{
    public enum AnimationID
    {
        NO_ANIM = 0,
        IDLE_SOUTH = 1,
        IDLE_WEST = 2,
        IDLE_NORTH = 3,
        IDLE_EAST = 4,
        WALK_SOUTH = 5,
        WALK_WEST = 6,
        WALK_NORTH = 7,
        WALK_EAST = 8
    }

    public class Animation
    {
        public List<AnimationFrame> frames { get; private set; }
        public AnimationID id { get; private set; } 


        public Animation() {
            frames = new List<AnimationFrame>();
            this.id = AnimationID.NO_ANIM;
        }

        public Animation(List<AnimationFrame> frames, AnimationID id) {
            this.frames = frames;
            this.id = id;
        }

        public void AddFrame(AnimationFrame frame) {
            frames.Add(frame);
        }

    }

    public class AnimationFrame
    {
        public Rectangle rect { get; private set; }
        public int milliseconds { get; private set; }

        public AnimationFrame(int x, int y, int width, int height, int milliseconds) {

            this.rect = new Rectangle(x, y, width, height);
            this.milliseconds = milliseconds;
        }

        public AnimationFrame(Rectangle rect, int milliseconds) {

            this.rect = rect;
            this.milliseconds = milliseconds;
        }

    }
}
