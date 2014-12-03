using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Player : Creature
    {
        public Player(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {

        }

        public Player(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, collisionOffset, collisionCenter)
        {

        }

        public override void Update(GameTime gameTime)
        {
            // Testing collision
            foreach (Sprite s in Game1.Obstacles.Sprites)
            {
                if (s.collidesWith(this) == true)
                {
                    System.Diagnostics.Debug.WriteLine("Collision with " + s.ToString());
                    System.Diagnostics.Debug.WriteLine(this.collisionRectangle.ToString() + " collided with " +  s.collisionRectangle.ToString());
                }
            }
            base.Update(gameTime);
        }
    }
}
