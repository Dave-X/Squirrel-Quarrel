using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class StandardEnemy : Enemy
    {
        public StandardEnemy(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, 5)
        {
            
        }

        public StandardEnemy(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, 5, collisionOffset, collisionCenter)
        {

        }

        public override void Update(GameTime gameTime)
        {
            //move towards the player

            base.Update(gameTime);
        }
    }
}
