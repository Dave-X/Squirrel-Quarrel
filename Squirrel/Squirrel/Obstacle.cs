﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    class Obstacle : StaticSprite
    {
        public Obstacle(Texture2D image, Vector2 position)
            : base(image, position)
        {

        }

        public Obstacle(Texture2D image, Vector2 position, Point collisionOffset, Point collisionCenter)
            : base(image, position, collisionOffset, collisionCenter)
        {

        }
    } // Obstacle
}
