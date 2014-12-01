using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    // Base class sprites.
    abstract public class Sprite
    {
        public Texture2D image {get; set;}
        public Vector2 position; // Stores the position of this sprite.

        /// <summary>
        /// Reposition the sprite using a vector.
        /// </summary>
        /// <param name="position"></param>
        public void moveTo(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Reposition the sprite using x and y coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void moveTo(float x, float y)
        {
            this.position = new Vector2(x, y);
        }

        public Sprite(Texture2D image, Vector2 position)
        {
            this.image = image;
            this.position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, Color.White);
        }
    } // GameObject
}
