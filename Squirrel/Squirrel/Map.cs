using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Squirrel
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Vector2 mapSize, mapPosition; //size of the entire map, position map will start at for a new game, current position
        Vector2 topBorderPos, bttmBorderPos, leftBorderPos, rightBorderPos;  //positions of border pieces
        Vector2 lrBorderSize, tbBorderSize;  //size of border pieces
        Texture2D texture, textureBorder;         //map background texture
        SpriteBatch spriteBatch;   //spritebatch to draw to screen

        bool canMoveUp, canMoveDown, canMoveLeft, canMoveRight;

        public Map(Game game)
            : base(game)
        {
            mapSize = new Vector2(5120f, 2880f);
            
            resetMap();

            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            topBorderPos = new Vector2(mapPosition.X, mapPosition.Y - 56);
            bttmBorderPos = new Vector2(mapPosition.X, mapPosition.Y + mapSize.Y - 328);
            leftBorderPos = new Vector2(mapPosition.X - 32, mapPosition.Y);
            rightBorderPos = new Vector2(mapPosition.X + mapSize.X - 608, mapPosition.Y);
            lrBorderSize = new Vector2(640f, mapSize.Y);
            tbBorderSize = new Vector2(mapSize.X, 384f);

            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //load in the texture for the ground (it will be drawn independent of the SpriteBatch stuff)
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("ground");
            textureBorder = Game.Content.Load<Texture2D>(".\\Textures\\Static\\Rock_1");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //boundaries for map movement to stop player from walking off the map.
            if (mapPosition.X <= -(mapSize.X - GraphicsDevice.Viewport.Width))     //Left boundary
                mapPosition.X = -(mapSize.X - GraphicsDevice.Viewport.Width);
            if (mapPosition.Y <= -(mapSize.Y - GraphicsDevice.Viewport.Height))    //Top boundary
                mapPosition.Y = -(mapSize.Y - GraphicsDevice.Viewport.Height);
            if (mapPosition.X >= 0)                                                //Right boundary
                mapPosition.X = 0;
            if (mapPosition.Y >= 0)                                                //Bottom boundary
                mapPosition.Y = 0;

            //prevent movement in case of collision
            collisionMovement(Game1.spriteManager.Obstacles);
            collisionMovement(Game1.spriteManager.Enemies);
            

            //controls for map movement
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && canMoveUp)
                this.updateMapPosition(new Vector2(0f, -5f));
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && canMoveDown)
                this.updateMapPosition(new Vector2(0f, 5f));
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && canMoveRight)
                this.updateMapPosition(new Vector2(5f, 0f));
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && canMoveLeft)
                this.updateMapPosition(new Vector2(-5f, 0f));

            //reset collision bools
            canMoveUp = true;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the components to the screen
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            
            //create and draw a rectangle to put the background texture on
            Rectangle background = new Rectangle(0, 0, (int)mapSize.X, (int)mapSize.Y);
            //set up the rectangles to tile the border inside of
            Rectangle topBorder = new Rectangle(0, 0, (int)tbBorderSize.X, (int)tbBorderSize.Y);
            Rectangle bottomBorder = new Rectangle(0, 0, (int)tbBorderSize.X, (int)tbBorderSize.Y);
            Rectangle leftBorder = new Rectangle(0, 0, (int)lrBorderSize.X, (int)lrBorderSize.Y);
            Rectangle rightBorder = new Rectangle(0, 0, (int)lrBorderSize.X, (int)lrBorderSize.Y);

            spriteBatch.Draw(texture, mapPosition, background, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            //draw the borders
            spriteBatch.Draw(textureBorder, topBorderPos, topBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(textureBorder, bttmBorderPos, bottomBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(textureBorder, leftBorderPos, leftBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(textureBorder, rightBorderPos, rightBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Based on the player's movement, this function will update the position of the map and everything
        /// located on it, (i.e. enemies, obstacles, nuts, etc.)
        /// </summary>
        /// <param name="playerPosition">References the player's current position.</param>
        public void updateMapPosition(Vector2 playerMovement)
        {
            //move map based on playerMovement
            Vector2 oldPosition = this.mapPosition;
            this.mapPosition -= playerMovement;
            //boundaries for map movement to stop player from walking off the map.
            if (mapPosition.X <= -(mapSize.X - GraphicsDevice.Viewport.Width))     //Left boundary
                mapPosition.X = -(mapSize.X - GraphicsDevice.Viewport.Width);
            if (mapPosition.Y <= -(mapSize.Y - GraphicsDevice.Viewport.Height))    //Top boundary
                mapPosition.Y = -(mapSize.Y - GraphicsDevice.Viewport.Height);
            if (mapPosition.X >= 0)                                                //Right boundary
                mapPosition.X = 0;
            if (mapPosition.Y >= 0)                                                //Bottom boundary
                mapPosition.Y = 0;
            //loop through and update the position of each obstacle relative to any map movement done this frame
            foreach (Sprite obstacle in Game1.spriteManager.Obstacles)
            {
                obstacle.position -= (oldPosition - this.mapPosition);
            }
            foreach (Sprite enemy in Game1.spriteManager.Enemies)
            {
                enemy.position -= (oldPosition - this.mapPosition);
            }
            foreach (Sprite nut in Game1.spriteManager.Nuts)
            {
                nut.position -= (oldPosition - this.mapPosition);
            }
            foreach (Sprite powerup in Game1.spriteManager.PowerUps)
            {
                powerup.position -= (oldPosition - this.mapPosition);
            }
            topBorderPos = new Vector2(mapPosition.X, mapPosition.Y);
            bttmBorderPos = new Vector2(mapPosition.X, mapPosition.Y + mapSize.Y - 328);
            leftBorderPos = new Vector2(mapPosition.X - 32, mapPosition.Y);
            rightBorderPos = new Vector2(mapPosition.X + mapSize.X - 608, mapPosition.Y);
            System.Diagnostics.Debug.WriteLine("topBorderPos X: " + topBorderPos.X + ", Y: " + topBorderPos.Y);
            //Game1.Hometree.Sprites[0].position += (oldPosition - this.position);
        }

        /// <summary>
        /// Resets the map to its initial position for restarting or starting new levels
        /// </summary>
        public void resetMap()
        {
            mapPosition = new Vector2(-mapSize.X / 2 + Game1.SCREEN_WIDTH / 2, -mapSize.Y / 2 + Game1.SCREEN_HEIGHT / 2);
        }

        /// <summary>
        /// Returns the value of the "camera or player" Y position.
        /// </summary>
        /// <returns>float stuff</returns>
        public float getHeight()
        {
            return this.topBorderPos.Y;
        }

        /// <summary>
        /// Returns the position of the map relative to the screen.
        /// </summary>
        /// <returns>Map position</returns>
        public Vector2 getMapPosition()
        {
            return this.mapPosition;
        }

        /// <summary>
        /// Checks for collision and prevents movement based on that collision
        /// </summary>
        /// <param name="sprites">The list of sprites player position is being compared to.</param>
        public void collisionMovement(List<Sprite> sprites)
        {
            Sprite player = Game1.spriteManager.Hero;
            foreach (Sprite s in sprites)
            {
                if (s.collidesWith(player))
                {
                    if (Math.Abs(s.position.X - player.position.X) > Math.Abs(s.position.Y - player.position.Y))
                    {
                        if (s.position.X - player.position.X < 0)
                            canMoveLeft = false;
                        else
                            canMoveRight = false;
                    }
                    else
                    {
                        if (s.position.Y - player.position.Y < 0)
                            canMoveUp = false;
                        else
                            canMoveDown = false;
                    }
                }
            }
        }
    }
}
