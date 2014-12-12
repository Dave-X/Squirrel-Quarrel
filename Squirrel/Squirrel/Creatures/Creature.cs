using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Creature : AnimatedSprite
    {
        public int Health = 100; // The health of the creature.
        public float speed = 2f;

        public Creature(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {

        }

        public Creature(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, collisionOffset, collisionCenter)
        {

        }

        public void takeDamage(int amount)
        {
            Health -= amount;
        }


    }
}
