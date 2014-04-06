using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Malarkey
{
    public class Animation
    {
        List<AnimationFrame> frames;

        public Animation() {
            frames = new List<AnimationFrame>();
        }

        public Animation(List<AnimationFrame> frames) {
            this.frames = frames;
        }

        public void AddFrame(AnimationFrame frame) {
            frames.Add(frame);
        }
    }

    public class AnimationFrame
    {
        Rectangle rect;
        double milliseconds;

        public AnimationFrame(int x, int y, int width, int height, double milliseconds) {

            Rectangle rect = new Rectangle(x, y, width, height);
            this.milliseconds = milliseconds;
        }

        public AnimationFrame(Rectangle rect, double milliseconds) {

            this.rect = rect;
            this.milliseconds = milliseconds;
        }

    }
}
