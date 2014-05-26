using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Malarkey
{
    class AnimatedSprite: Sprite
    {
        List<Animation> animations;

        int spriteID;

        // take care of instance stuff:
        Animation currentAnimation;
        // AnimationID currentAnim = AnimationID.WALK_SOUTH;
        int currentFrame = 0;
        int msElapsed = 0;

               
        // Sprite sprite;
        public AnimatedSprite(int spriteID, Texture2D newTexture, Rectangle newRect, double newScale)
        {
            this.spriteID = spriteID;

            animations = new List<Animation>();

            scale = newScale;
            // set up width & height based on scale:
            width = (int)(newRect.Width * scale);
            height = (int)(newRect.Height * scale);
            texture = newTexture;
            sourceRect = newRect;


            if (this.spriteID == 1)
            {

                List<AnimationFrame> animIdleSouth = new List<AnimationFrame>
                {
                    new AnimationFrame(234, 738, 66, 53, 1000)
                };

                List<AnimationFrame> animIdleNorth = new List<AnimationFrame>
                {
                    new AnimationFrame(25, 395, 62, 62, 1000)
                };

                List<AnimationFrame> animMoveSouth = new List<AnimationFrame>
                {
                    new AnimationFrame(30, 669, 66, 53, 200),
                    new AnimationFrame(96, 669, 66, 53, 200),
                    new AnimationFrame(160, 669, 66, 53, 200)
                };

                Animation walkSouth = new Animation(animMoveSouth, AnimationID.WALK_SOUTH);
                Animation idleSouth = new Animation(animIdleSouth, AnimationID.IDLE_SOUTH);
                Animation idleNorth = new Animation(animIdleNorth, AnimationID.IDLE_NORTH);

                animations.Add(walkSouth);
                animations.Add(idleSouth);
                animations.Add(idleNorth);
            }
        }

        public int GetSpriteID()
        {
            return spriteID;
        }

        public void Update(GameTime gameTime, AnimationID id)
        {
            // only look for it when we have to (if we do this properly, we should be able to get rid of the null checks)
            if((currentAnimation == null) || (currentAnimation.id != id)) {

                foreach (Animation animation in animations)
                {           // got to be a better way to do this
                    if (animation.id == id)
                    {
                        currentFrame = 0;
                        msElapsed = 0;
                        currentAnimation = animation;
                        break;
                    }
                }
            }
            
            if(currentAnimation != null)
            {
                
                
                msElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (msElapsed > currentAnimation.frames[currentFrame].milliseconds)
                {
                    this.msElapsed -= currentAnimation.frames[currentFrame].milliseconds;
                    this.currentFrame++;
                    if (this.currentFrame >= currentAnimation.frames.Count)
                    {
                        this.currentFrame = 0;
                    }
                }

                this.UpdateRect(currentAnimation.frames[currentFrame].rect);
            }


        }

    }
}
