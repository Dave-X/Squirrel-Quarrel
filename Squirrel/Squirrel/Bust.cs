using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Bust : AnimatedSprite
    {
        public int timeAlive = 0; // How long this projectile has been going.
        public int lifeSpan = 576;
        public bool hit = false;
        public bool dead
        {
            get
            {
                return timeAlive >= lifeSpan;
            }
        }



        public Bust(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {
            this.currentAnimation = new Animation(image, frameSize, currentFrame, sheetSize, millisecondsPerFrame);
            this.collisionOffset.X += 256;
            this.collisionOffset.Y += 256;
            this.collisionCenter.X = -256;
        }

        public Bust(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, collisionOffset, collisionCenter)
        {
            this.currentAnimation = new Animation(image, frameSize, currentFrame, sheetSize, millisecondsPerFrame);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.image, position, new Rectangle(currentAnimation.currentFrame.X * currentAnimation.frameSize.X, currentAnimation.currentFrame.Y * currentAnimation.frameSize.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), Color.White, 0f, Vector2.Zero, 3.0f, SpriteEffects.None, this.drawDepth);
            //base.Draw(gameTime, spriteBatch);
        }

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


            timeAlive += gameTime.ElapsedGameTime.Milliseconds;

            // Check if it hit an enemy.
            foreach (Creature c in Game1.spriteManager.Enemies)
            {
                if (this.collidesWith(c))
                {
                    this.hit = true;
                    c.takeDamage(4);
                    ((Enemy)c).red = true;
                    if (c.Health <= 0)
                    {
                        c.dead = true;
                    }
                }
            }
        }
    }
}
