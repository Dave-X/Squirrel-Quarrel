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
        public Point collisionOffset; // Set this point (x, y) as an adjustment for the collision box.
        public Point collisionCenter; // Sets the center (x, y) local to the sprite for collision detection.  The center is 0,0.
        public float drawDepth = 0.5f; //
        public virtual Rectangle collisionRectangle // Used for detecting collisions.
        {
            get
            {
                int left = (int)position.X - collisionOffset.X - collisionCenter.X;
                int top = (int)position.Y - collisionOffset.Y - collisionCenter.Y;
                int right = image.Width + (collisionOffset.X * 2);
                int bottom = image.Height + (collisionOffset.Y * 2);
                return new Rectangle(left, top, right, bottom);
            }
        }
            

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

        /// <summary>
        /// Sprites with a higher number are drawn on top of sprites with a lower value.  
        /// </summary>
        /// <param name="z">Between 0 and 1.</param>
        public void setDepth(float z)
        {
            this.drawDepth = z;
        }

        // Called once.  This is required because map moves its position.
        private void calcDrawDepth()
        {
            this.drawDepth = (this.position.Y + (this.image.Height / 2) / -2160);
            System.Diagnostics.Debug.WriteLine(this.drawDepth);
        }

        public Sprite(Texture2D image, Vector2 position)
        {
            this.image = image;
            this.position = position;
            this.collisionOffset = Point.Zero;
            this.collisionCenter = Point.Zero;
            calcDrawDepth();
        }
        // Additional constructor.
        public Sprite(Texture2D image, Vector2 position, Point collisionOffset, Point collisionCenter)
        {
            this.image = image;
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.collisionCenter = collisionCenter;
            calcDrawDepth();

        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(image, position, Color.White);
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, drawDepth);
        }

        // Returns true if this sprite collides with the given sprite.
        public Boolean collidesWith(Sprite other)
        {
            return collisionRectangle.Intersects(other.collisionRectangle);
        }


        // Returns the "center" as a vector based on the size of the sprite and the resolution.
        public virtual Vector2 center()
        {
            return new Vector2((Game1.SCREEN_WIDTH / 2) - (image.Width / 2), (Game1.SCREEN_HEIGHT / 2) - (image.Height / 2));
        }


    } // GameObject
}
