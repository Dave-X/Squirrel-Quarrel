using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    class SpeedPowerUp : StaticSprite
    {
        public SpeedPowerUp(Texture2D image, Vector2 position)
            : base(image, position)
        {

        }

        public SpeedPowerUp(Texture2D image, Vector2 position, Point collisionOffset, Point collisionCenter)
            : base(image, position, collisionOffset, collisionCenter)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (this.collidesWith(Game1.spriteManager.Hero))
            {
                (Game1.spriteManager.Hero as Player).speed = 7;
                this.dead = true;
            }
        }
    }
}
