using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    class PowerUp : StaticSprite
    {
        bool used = false;  //true when the powerup is used

        public PowerUp(Texture2D image, Vector2 position)
            : base(image, position)
        {

        }

        public PowerUp(Texture2D image, Vector2 position, Point collisionOffset, Point collisionCenter)
            : base(image, position, collisionOffset, collisionCenter)
        {

        }
    }
}
