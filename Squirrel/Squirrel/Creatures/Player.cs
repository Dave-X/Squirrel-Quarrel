using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Squirrel
{
    public class Player : Creature
    {
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;
        SpriteEffects SE = SpriteEffects.None;
        float XShot = 0f;
        float YShot = 0f;
        int shotRate = 450; // LOWER IS FASTER;
        int timeSinceLastShot;
        public int nutsHeld = 0;
        public int totalCollected = 0; // Total amount of nuts collected.

        //copy of enemy's red stuff
        private int timeRed = 0;
        public bool red = false;
        private Color color = Color.White;
        //

        public Player(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {
            speed = 5f;
        }

        public Player(Texture2D image, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame, Point collisionOffset, Point collisionCenter)
            : base(image, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame, collisionOffset, collisionCenter)
        {
            speed = 5f;
        }

        public override void Update(GameTime gameTime)
        {
            //red stuff copied from enemy
            if (red)
            {
                color = Color.Red;
                timeRed += gameTime.ElapsedGameTime.Milliseconds;
                if (timeRed > 100)
                {
                    timeRed = 0;
                    red = false;
                    color = Color.White;
                }
            }
            //

            // Handle control stuffs
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            timeSinceLastShot += gameTime.ElapsedGameTime.Milliseconds;

            if (Game1.map.isMoving)
            {
                currentAnimation = move;
            }
            else
            {
                currentAnimation = idle;
            }

            if (previousKeyboardState.IsKeyDown(Keys.Space) == true && currentKeyboardState.IsKeyUp(Keys.Space) == false)
            {
                //System.Diagnostics.Debug.WriteLine(timeSinceLastShot);

                // shooting business.
                if (timeSinceLastShot >= shotRate)
                {
                    // All directions
                    /*
                    switch (Game1.map.getPlayerDirection())
                    {
                        case PlayerDirection.East:
                            X = 20f;
                            break;
                        case PlayerDirection.NorthEast:
                            X = 20f;
                            Y = -20f;
                            break;
                        case PlayerDirection.SouthEast:
                            X = 20f;
                            Y = 20f;
                            break;
                        case PlayerDirection.West:
                            X = -20f;
                            break;
                        case PlayerDirection.SouthWest:
                            X = -20f;
                            Y = 20f;
                            break;
                        case PlayerDirection.NorthWest:
                            X = -20f;
                            Y = -20f;
                            break;
                        case PlayerDirection.South:
                            Y = 20f;
                            break;
                        case PlayerDirection.North:
                            Y = -20f;
                            break;
                        default:
                            break;
                    }
                    */

                    // Some directions
                    switch (Game1.map.getPlayerDirection())
                    {
                        case PlayerDirection.East:
                        case PlayerDirection.NorthEast:
                        case PlayerDirection.SouthEast:
                            XShot = 20;
                            YShot = 0;
                            break;
                        case PlayerDirection.West:
                        case PlayerDirection.SouthWest:
                        case PlayerDirection.NorthWest:
                            XShot = -20;
                            YShot = 0;
                            break;
                        default:
                            break;
                    }
                    Game1.spriteManager.Shoot(this.position, new Vector2(XShot, YShot));
                    timeSinceLastShot = 0;
                }
            }

            // Nut stuff
            foreach (Nut s in Game1.spriteManager.Nuts)
            {
                if (this.collidesWith(s) && this.nutsHeld < 2)
                {
                    nutsHeld++; // Increase nutes held.
                    s.dead = true;
                    System.Diagnostics.Debug.WriteLine(this.nutsHeld);
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


        /// <summary>
        /// Removes nuts and adds them to total.
        /// </summary>
        public void dropOff()
        {
            totalCollected += nutsHeld;
            nutsHeld = 0;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (Game1.map.getPlayerDirection())
            {
                case PlayerDirection.East:
                case PlayerDirection.NorthEast:
                case PlayerDirection.SouthEast:
                    SE = SpriteEffects.None;
                    break;
                case PlayerDirection.West:
                case PlayerDirection.SouthWest:
                case PlayerDirection.NorthWest:
                    SE = SpriteEffects.FlipHorizontally;
                    break;
                default:
                    break;
            }

            //spriteBatch.Draw(currentAnimation.image, new Rectangle((int)position.X, (int)position.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), new Rectangle(currentAnimation.currentFrame.X * currentAnimation.frameSize.X, currentAnimation.currentFrame.Y * currentAnimation.frameSize.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), Color.White, 0f, Vector2.Zero, SE, this.drawDepth);
            //new Rectangle((int)position.X, (int)position.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y)

            spriteBatch.Draw(currentAnimation.image, position, new Rectangle(currentAnimation.currentFrame.X * currentAnimation.frameSize.X, currentAnimation.currentFrame.Y * currentAnimation.frameSize.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y), color, 0f, Vector2.Zero, 1.0f, SE, this.drawDepth);
            Debug.WriteLine(Game1.map.isMoving);
            //base.Draw(gameTime, spriteBatch);
        }
    }
}
