using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class StaticSprite : Sprite
    {
        public StaticSprite(Texture2D image, Vector2 position) : base(image, position)
        {

        }        
    }
}
