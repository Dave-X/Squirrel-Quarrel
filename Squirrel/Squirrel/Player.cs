using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Player : AnimatedSprite
    {
        private int Health { get; set; } // The health of the creature.

        public Player(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {

        }

        public override void Update(GameTime gameTime)
        {
            // Testing collision
            foreach (Sprite s in Game1.Obstacles.Sprites)
            {
                if (s.collidesWith(this) == true)
                {
                    System.Diagnostics.Debug.WriteLine("Collided...");
                }
            }
            base.Update(gameTime);
        }
    }
}
