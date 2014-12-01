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
        Vector2 mapSize, position; //size of the entire map, position map will start at for a new game, current position
        Texture2D texture;         //map background texture
        SpriteBatch spriteBatch;   //spritebatch to draw to screen

        public Map(Game game)
            : base(game)
        {
            mapSize = new Vector2(2400f, 1200f);
            resetMap();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //load in the texture for the ground (it will be drawn independent of the SpriteBatch stuff)
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("ground");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //boundaries for map movement to stop player from walking off the map.
            if (position.X <= -(mapSize.X - GraphicsDevice.Viewport.Width))     //Left boundary
                position.X = -(mapSize.X - GraphicsDevice.Viewport.Width);
            if (position.Y <= -(mapSize.Y - GraphicsDevice.Viewport.Height))    //Top boundary
                position.Y = -(mapSize.Y - GraphicsDevice.Viewport.Height);
            if (position.X >= 0)                                                //Right boundary
                position.X = 0;
            if (position.Y >= 0)                                                //Bottom boundary
                position.Y = 0;

            //test controls for map movement
            
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                this.updateMapPosition(new Vector2(0f, -5f));
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                this.updateMapPosition(new Vector2(0f, 5f));
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                this.updateMapPosition(new Vector2(5f, 0f));
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                this.updateMapPosition(new Vector2(-5f, 0f));
            
            
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
            Rectangle source = new Rectangle((int)position.X, (int)position.Y, (int)mapSize.X, (int)mapSize.Y);
            spriteBatch.Draw(texture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

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
            Vector2 oldPosition = this.position;
            this.position += playerMovement;
            //boundaries for map movement to stop player from walking off the map.
            if (position.X <= -(mapSize.X - GraphicsDevice.Viewport.Width))     //Left boundary
                position.X = -(mapSize.X - GraphicsDevice.Viewport.Width);
            if (position.Y <= -(mapSize.Y - GraphicsDevice.Viewport.Height))    //Top boundary
                position.Y = -(mapSize.Y - GraphicsDevice.Viewport.Height);
            if (position.X >= 0)                                                //Right boundary
                position.X = 0;
            if (position.Y >= 0)                                                //Bottom boundary
                position.Y = 0;
            //loop through and update the position of each obstacle relative to any map movement done this frame
            foreach (Sprite obstacle in Game1.Obstacles.Sprites)
            {
                obstacle.position += (oldPosition - this.position);
            }
            foreach (Sprite enemy in Game1.Enemies.Sprites)
            {
                enemy.position += (oldPosition - this.position);
            }
            foreach (Sprite nut in Game1.Nuts.Sprites)
            {
                nut.position += (oldPosition - this.position);
            }
            foreach (Sprite powerup in Game1.PowerUps.Sprites)
            {
                powerup.position += (oldPosition - this.position);
            }
            Game1.Hometree.Sprites[0].position += (oldPosition - this.position);
        }

        /// <summary>
        /// Resets the map to its initial position for restarting or starting new levels
        /// </summary>
        public void resetMap()
        {
            position = new Vector2(-mapSize.X / 2 + Game1.SCREEN_WIDTH / 2, -mapSize.Y / 2 + Game1.SCREEN_HEIGHT / 2);
        }
    }
}
