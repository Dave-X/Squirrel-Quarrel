using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class AnimatedSprite : Sprite
    {
        Point frameSize; // This is the size of each frame within the sprite. A sprite sheet is made up of a number of frames.
        Point currentFrame; // The starting frame number on the sheet as row, column.
        Point sheetSize; // This is the number of sprites as number of columns, number of rows.
        public int timeSinceLastFrame = 0;
        public int millisecondsPerFrame = 0; // The amount of time to spend per a frame.
        public override Rectangle collisionRectangle // Used for detecting collisions.
        {
            get
            {
                int left = (int)position.X - collisionOffset.X - collisionCenter.X;
                int top = (int)position.Y - collisionOffset.Y - collisionCenter.Y;
                int right = frameSize.X + (collisionOffset.X * 2);
                int bottom = frameSize.Y + (collisionOffset.Y * 2);

                return new Rectangle(left, top, right, bottom);
                //return new Rectangle((int)position.X + collisionOffset.X, (int)position.Y + collisionOffset.Y, frameSize.X - (collisionOffset.X * 2), frameSize.Y - (collisionOffset.Y * 2));
                //return new Rectangle((int)position.X + collisionOffset.X + collisionCenter.X, (int)position.Y + collisionOffset.Y + collisionCenter.Y, frameSize.X - (collisionOffset.X * 2), frameSize.Y - (collisionOffset.Y * 2));
            }
        }


        #region Constructors

            public AnimatedSprite(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
                : base(image, position)
            {
                this.frameSize = frameSize;
                this.currentFrame = currentFrame;
                this.sheetSize = sheetSize;
                this.millisecondsPerFrame = millisecondsPerFrame;
            }

            public AnimatedSprite(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
                : base(image, position, collisionOffset, collisionCenter)
            {
                this.frameSize = frameSize;
                this.currentFrame = currentFrame;
                this.sheetSize = sheetSize;
                this.millisecondsPerFrame = millisecondsPerFrame;
            }

        #endregion


        public override void Update(GameTime gameTime)
        {

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            //System.Diagnostics.Debug.Print(currentFrame.X.ToString());
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                currentFrame.X++;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    currentFrame.Y++;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y), new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.drawDepth);
        }

        // Returns the "center" as a vector based on the size of the sprite and the resolution.
        public override Vector2 center()
        {
            return new Vector2((Game1.SCREEN_WIDTH / 2) - (frameSize.X / 2), (Game1.SCREEN_HEIGHT / 2) - (frameSize.Y / 2));
        }

    }
}
