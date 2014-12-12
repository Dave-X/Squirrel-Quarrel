using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Enemy : Creature
    {
        private int Strength { get; set; }      //how much damage the enemy does when it attacks
        SpriteEffects SE;
        private int timeRed = 0;
        public bool red = false;
        private Color color = Color.White;
        public PlayerDirection facing;

        public Enemy(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, int strength)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {
            Strength = strength;
        }

        public Enemy(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, int strength, Point collisionOffset, Point collisionCenter)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, collisionOffset, collisionCenter)
        {
            Strength = strength;
        }

        public override void Update(GameTime gameTime)
        {
            // Update draw order.
            //this.drawDepth = Game1.map.

            this.drawDepth = Game1.map.getHeight() / -2160f;
            foreach (Sprite s in Game1.spriteManager.Obstacles)
            {
                if (s.collidesWith(this) == true)
                {
                    //System.Diagnostics.Debug.WriteLine("Collision with " + s.ToString());
                    //System.Diagnostics.Debug.WriteLine(this.collisionRectangle.ToString() + " collided with " +  s.collisionRectangle.ToString());
                    //System.Diagnostics.Debug.WriteLine("Draw depth: " + s.drawDepth);

                }
            }

            Player player = Game1.spriteManager.Hero as Player;
            if (this.collidesWith(Game1.spriteManager.Hero) && !player.red)
            {
                player.takeDamage(this.attack());
                player.red = true;
            }

            if (red)
            {
                color = Color.Red;
                timeRed += gameTime.ElapsedGameTime.Milliseconds;
                if (timeRed > 100)
                {
                    timeRed = 0;
                    red = false;
                    color = Color.White;
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// Gets the amount of damage that will result from this enemy's attack.
        /// </summary>
        /// <returns>Damage amount</returns>
        public int attack()
        {
            return Strength;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.facing == PlayerDirection.East)
            {
                SE = SpriteEffects.None;
            }
            else
            {
                SE = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(currentAnimation.image, new Rectangle((int)position.X, (int)position.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), new Rectangle(currentAnimation.currentFrame.X * currentAnimation.frameSize.X, currentAnimation.currentFrame.Y * currentAnimation.frameSize.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), color, 0f, Vector2.Zero, SE, this.drawDepth);

            //base.Draw(gameTime, spriteBatch);
        }
    }
}
