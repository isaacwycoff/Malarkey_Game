using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;       // for debugging

// FIXME: do we need all of these XNA headers? Probably not.
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
    /// <summary>
    /// Faders overlay the full-screen for the purposes of fading to black or white.
    /// </summary>
    class Fader: Element
    {

        public const float DEFAULT_FADE_SHIFT = 0.001f;     // This means it takes about 1 second

        public enum FadeStatus
        {
            noFade = 0,
            fadeOut = 1,
            fadeIn = 2,
            black = 3
        };

        const float FADE_OPAQUE = 1.0f;
        const float FADE_TRANSPARENT = 0.0f;



        float fadeShift = DEFAULT_FADE_SHIFT;

        FadeStatus fadeStatus;

        float currentFade = 1.0f;               // how transparent are we? 1.0f = completely opaque. 0.0f = completely transparent.

        public Fader(Texture2D texture, Rectangle fullScreen)
        {
            fadeStatus = FadeStatus.noFade;
            currentFade = 0.0f;

            // the fade texture is a single-pixel bitmap:
            sprite = new Sprite(texture, new Rectangle(0, 0, 1, 1), 1.0);

            sprite.setDimensions(fullScreen.Width, fullScreen.Height);

        }

        public override void Draw(GameTime gameTime)
        {
            if (fadeStatus == FadeStatus.noFade) return;        // not fading, get outta here!
            if (currentFade == 0.0f) return;

            Color fadeColor = new Color(1.0f, 1.0f, 1.0f, currentFade);

            sprite.Draw(new Vector2(0.0f, 0.0f), SpriteEffects.None, fadeColor);

        }

        public override void Update(GameTime gameTime)
        {
            int elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

//            Debug.WriteLine(milliseconds);
            
            switch (fadeStatus)
            {
                case FadeStatus.fadeOut:
                    {

                        currentFade += fadeShift * elapsedTime;
                        if (currentFade >= FADE_OPAQUE) currentFade = FADE_OPAQUE;

                        break;
                    }
                case FadeStatus.fadeIn:
                    {
                        currentFade -= fadeShift * elapsedTime;
                        if (currentFade <= FADE_TRANSPARENT) currentFade = FADE_TRANSPARENT;

                        break;
                    }
                case FadeStatus.black:
                    {
                        if (currentFade != FADE_OPAQUE) currentFade = FADE_OPAQUE;

                        break;
                    }
                case FadeStatus.noFade:
                    {
                        if (currentFade != FADE_TRANSPARENT) currentFade = FADE_TRANSPARENT;

                        break;
                    }
                default:
                    {
                        // ERROR!

                        break;
                    }


            }

            base.Update(gameTime);
        }

        public void fadeIn(float newFadeShift)
        {
            currentFade = FADE_OPAQUE;
            fadeStatus = FadeStatus.fadeIn;
            fadeShift = newFadeShift;
        }

        public void fadeOut(float newFadeShift)
        {
            currentFade = FADE_TRANSPARENT;
            fadeStatus = FadeStatus.fadeOut;
            fadeShift = newFadeShift;
        }

    }
}
