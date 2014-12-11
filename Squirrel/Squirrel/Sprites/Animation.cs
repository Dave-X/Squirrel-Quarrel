using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Animation
    {
        public Texture2D image;
        public Point frameSize; // This is the size of each frame within the sprite. A sprite sheet is made up of a number of frames.
        public Point currentFrame; // The starting frame number on the sheet as row, column.
        public Point sheetSize; // This is the number of sprites as number of columns, number of rows.
        public int millisecondsPerFrame = 0; // The amount of time to spend per a frame.

        public Animation(Texture2D image, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
        {
            this.image = image;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

    }
}
