using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Squirrel
{
    public class Player : Creature
    {
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;
        int shotRate = 450; // LOWER IS FASTER;
        int timeSinceLastShot;

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
            // Handle control stuffs
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            timeSinceLastShot += gameTime.ElapsedGameTime.Milliseconds;

            if (previousKeyboardState.IsKeyDown(Keys.Space) == true && currentKeyboardState.IsKeyUp(Keys.Space) == false)
            {
                System.Diagnostics.Debug.WriteLine(timeSinceLastShot);   
                if (timeSinceLastShot >= shotRate)
                {
                    Game1.spriteManager.Shoot(this.position, new Vector2(20, 0));
                    timeSinceLastShot = 0;
                }
            }
            
            // Update draw order.
            //this.drawDepth = Game1.map.
            this.drawDepth = Game1.map.getHeight() / -2160f;
            System.Diagnostics.Debug.WriteLine("Player draw depth: " + this.drawDepth);
            foreach (Sprite s in Game1.spriteManager.Obstacles)
            {
                if (s.collidesWith(this) == true)
                {
                    //System.Diagnostics.Debug.WriteLine("Collision with " + s.ToString());
                    //System.Diagnostics.Debug.WriteLine(this.collisionRectangle.ToString() + " collided with " +  s.collisionRectangle.ToString());
                    //System.Diagnostics.Debug.WriteLine("Draw depth: " + s.drawDepth);

                }
            }
           
            base.Update(gameTime);
        }
    }
}
