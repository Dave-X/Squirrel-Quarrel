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
        public Animation idle;
        public Animation move;
        public Animation attack;
        public Animation death;
        public Animation currentAnimation;
        public int timeSinceLastFrame = 0;
        public override Rectangle collisionRectangle // Used for detecting collisions.
        {
            get
            {
                int left = (int)position.X - collisionOffset.X - collisionCenter.X;
                int top = (int)position.Y - collisionOffset.Y - collisionCenter.Y;
                int right = currentAnimation.frameSize.X + (collisionOffset.X * 2);
                int bottom = currentAnimation.frameSize.Y + (collisionOffset.Y * 2);

                return new Rectangle(left, top, right, bottom);
                //return new Rectangle((int)position.X + collisionOffset.X, (int)position.Y + collisionOffset.Y, frameSize.X - (collisionOffset.X * 2), frameSize.Y - (collisionOffset.Y * 2));
                //return new Rectangle((int)position.X + collisionOffset.X + collisionCenter.X, (int)position.Y + collisionOffset.Y + collisionCenter.Y, frameSize.X - (collisionOffset.X * 2), frameSize.Y - (collisionOffset.Y * 2));
            }
        }


        #region Constructors
        
            public AnimatedSprite(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
                : base(image, position)
            {
                idle = new Animation(image, frameSize, currentFrame, sheetSize, millisecondsPerFrame);
                currentAnimation = idle;
            }

            public AnimatedSprite(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
                : base(image, position, collisionOffset, collisionCenter)
            {
                idle = new Animation(image, frameSize, currentFrame, sheetSize, millisecondsPerFrame);
                currentAnimation = idle;
            }

        #endregion


        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            //System.Diagnostics.Debug.Print(currentFrame.X.ToString());
            if (timeSinceLastFrame > currentAnimation.millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                currentAnimation.currentFrame.X++;
                if (currentAnimation.currentFrame.X >= currentAnimation.sheetSize.X)
                {
                    currentAnimation.currentFrame.X = 0;
                    currentAnimation.currentFrame.Y++;
                    if (currentAnimation.currentFrame.Y >= currentAnimation.sheetSize.Y)
                    {
                        currentAnimation.currentFrame.Y = 0;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.image, new Rectangle((int)position.X, (int)position.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), new Rectangle(currentAnimation.currentFrame.X * currentAnimation.frameSize.X, currentAnimation.currentFrame.Y * currentAnimation.frameSize.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.drawDepth);
            base.Draw(gameTime, spriteBatch);
        }

        // Returns the "center" as a vector based on the size of the sprite and the resolution.
        public override Vector2 center()
        {
            return new Vector2((Game1.SCREEN_WIDTH / 2) - (currentAnimation.frameSize.X / 2), (Game1.SCREEN_HEIGHT / 2) - (currentAnimation.frameSize.Y / 2));
        }

    }
}
