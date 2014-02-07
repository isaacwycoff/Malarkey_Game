using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;          // for content loading
using Microsoft.Xna.Framework.Graphics;         // for Texture2D

namespace Malarkey
{
    class TextureManager
    {
        private static TextureManager manager = new TextureManager();
        private List<TextureReference> textureReferences;
        private ContentManager _content = null;

        private TextureManager()
        {
            textureReferences = new List<TextureReference>;
        }

        public static TextureManager GetInstance()
        {
            return manager;
        }

        public Boolean SetContentManager(ContentManager content)
        {
            content = _content;
            return true;
        }

        public ContentManager GetContentManager()
        {
            return _content;
        }

        public void AddTexture(String name, String path)
        {
            // adds a new texture reference to the list
            // note - this DOES NOT load the actual texture.
            TextureReference newReference = new TextureReference(name, path);
            textureReferences.Add(newReference);
        }

        public Texture2D GetTexture(String name)
        {
            // check if the texture's name is in our list
            foreach (TextureReference tmpReference in textureReferences)
            {
                if (name.Equals(tmpReference.name))
                {
                    return tmpReference.GetTexture();
                }
            }
            throw new Exception("Couldn't find a texture with name " + name);
        }

        private class TextureReference
        {
            public String name;           // short-hand name for the texture
            private String path;
            private Texture2D texture = null;

            public TextureReference(String name, String path)
            {
                this.name = name;
                this.path = path;
            }

            public Texture2D GetTexture()
            {
                // if the texture hasn't been loaded, load it.
                // either way, return the texture

                if (this.texture == null)
                {
                    TextureManager manager = TextureManager.GetInstance();
                    ContentManager content = TextureManager.GetInstance().GetContentManager();

                    this.texture = content.Load<Texture2D>("Graphics/" + path);
                    
                }
                return this.texture;
            }
        }
    }
}
