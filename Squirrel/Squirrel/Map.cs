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
        Texture2D texture;         //map background texture
        Texture2D rock1Tex, rock2Tex, rock3Tex, nutTex, enemyTex;     //rock textures
        SpriteBatch spriteBatch;   //spritebatch to draw to screen
        Random random;

        bool canMoveUp, canMoveDown, canMoveLeft, canMoveRight;     //allows the player to move the direction if true
        int rockCount, nutCount, enemyCount;                        //number of rock obstacles on the map
        PlayerDirection playerDirection;                //the player's movement direction

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
            random = new Random();

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

            rock1Tex = Game.Content.Load<Texture2D>(@"Textures\Static\Rock_1");
            nutTex = Game.Content.Load<Texture2D>("acorn");
            enemyTex = Game.Content.Load<Texture2D>("sampleSpriteSheet");
            rockCount = 20;
            for (int i = 0; i < rockCount; i++)
                Game1.spriteManager.Obstacles.Add(new Obstacle(rock1Tex, new Vector2(0, 0), new Point(-32, -64), new Point(0, 16)));
            nutCount = 15;
            for (int i = 0; i < nutCount; i++)
                Game1.spriteManager.Nuts.Add(new Nut(nutTex, new Vector2(0, 0), new Point(-32, -64), new Point(0, 16)));
            enemyCount = 5;
            for (int i = 0; i < nutCount; i++)
                Game1.spriteManager.Enemies.Add(new StandardEnemy(Game.Content.Load<Texture2D>(@"sampleSpritesheet"), Vector2.Zero, new Point(128, 128), Point.Zero, new Point(4, 4), 16));
            distributeObjects(Game1.spriteManager.Obstacles);
            distributeObjects(Game1.spriteManager.Nuts);
            distributeObjects(Game1.spriteManager.Enemies);

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
            KeyboardState keystate = Keyboard.GetState();
            int position = 0;
            if ((keystate.IsKeyDown(Keys.Up) || keystate.IsKeyDown(Keys.W)) && canMoveUp)
            {
                this.updateMapPosition(new Vector2(0f, -5f));
                position += 1;
            }
            if ((keystate.IsKeyDown(Keys.Down) || keystate.IsKeyDown(Keys.S)) && canMoveDown)
            {
                this.updateMapPosition(new Vector2(0f, 5f));
                position += 4;
            }
            if ((keystate.IsKeyDown(Keys.Right) || keystate.IsKeyDown(Keys.D)) && canMoveRight)
            {
                this.updateMapPosition(new Vector2(5f, 0f));
                position += 2;
            }
            if ((keystate.IsKeyDown(Keys.Left) || keystate.IsKeyDown(Keys.A)) && canMoveLeft)
            {
                this.updateMapPosition(new Vector2(-5f, 0f));
                position += 8;
            }

            switch (position)
            {
                case 1:
                    playerDirection = PlayerDirection.North;
                    break;
                case 3:
                    playerDirection = PlayerDirection.NorthEast;
                    break;
                case 2:
                    playerDirection = PlayerDirection.East;
                    break;
                case 6:
                    playerDirection = PlayerDirection.SouthEast;
                    break;
                case 4:
                    playerDirection = PlayerDirection.South;
                    break;
                case 12:
                    playerDirection = PlayerDirection.SouthWest;
                    break;
                case 8:
                    playerDirection = PlayerDirection.West;
                    break;
                case 9:
                    playerDirection = PlayerDirection.NorthWest;
                    break;
                default:
                    break;
            }
            
            foreach (Sprite enemy in Game1.spriteManager.Enemies)
            {
                //move the enemy toward the player if the enemy is entirely on screen
                if (enemy.position.X >= 0 && enemy.position.X <= Game1.SCREEN_WIDTH - 64
                    && enemy.position.Y >= 0 && enemy.position.Y <= Game1.SCREEN_HEIGHT - 64)
                {
                    Vector2 newPos = enemy.position;
                    if (enemy.position.X < Game1.spriteManager.Hero.position.X)
                        newPos.X = enemy.position.X + 1;
                    if (enemy.position.X > Game1.spriteManager.Hero.position.X)
                        newPos.X = enemy.position.X - 1;
                    if (enemy.position.Y < Game1.spriteManager.Hero.position.Y)
                        newPos.Y = enemy.position.Y + 1;
                    if (enemy.position.Y > Game1.spriteManager.Hero.position.Y)
                        newPos.Y = enemy.position.Y - 1;
                    enemy.moveTo(newPos);
                }
            }

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
            spriteBatch.Draw(rock1Tex, topBorderPos, topBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(rock1Tex, bttmBorderPos, bottomBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(rock1Tex, leftBorderPos, leftBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(rock1Tex, rightBorderPos, rightBorder, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

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
                obstacle.moveTo(obstacle.position - (oldPosition - this.mapPosition));
            }
            foreach (Sprite enemy in Game1.spriteManager.Enemies)
            {
                //move the enemy based on the map movement
                enemy.moveTo(enemy.position - (oldPosition - this.mapPosition));
            }
            foreach (Sprite nut in Game1.spriteManager.Nuts)
            {
                nut.moveTo(nut.position - (oldPosition - this.mapPosition));
            }
            foreach (Sprite powerup in Game1.spriteManager.PowerUps)
            {
                powerup.moveTo(powerup.position - (oldPosition - this.mapPosition));
            }
            //Game1.spriteManager.HomeTree.position -= (oldPosition - this.mapPosition);
            topBorderPos = new Vector2(mapPosition.X, mapPosition.Y);
            bttmBorderPos = new Vector2(mapPosition.X, mapPosition.Y + mapSize.Y - 328);
            leftBorderPos = new Vector2(mapPosition.X - 32, mapPosition.Y);
            rightBorderPos = new Vector2(mapPosition.X + mapSize.X - 608, mapPosition.Y);
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

        /// <summary>
        /// Properly distributes objects around the map and resolves collisions.
        /// </summary>
        /// <param name="objects">The objects to be disributed</param>
        public void distributeObjects(List<Sprite> objects)
        {
            foreach (Sprite obj in objects)
            {
                bool collision;
                do
                {
                    obj.moveTo(randomMapPosition());
                    collision = false;
                    /*
                    if (obj.collidesWith(Game1.spriteManager.HomeTree))
                        collision = true;
                    if (obj.collidesWith(Game1.spriteManager.Hero))
                        collision = true;*/
                    foreach (Sprite obj2 in Game1.spriteManager.Obstacles)
                    {
                        if (obj == obj2)
                            continue;
                        if (obj.collidesWith(obj2))
                            collision = true;
                    }
                    foreach (Sprite obj2 in Game1.spriteManager.Nuts)
                    {
                        if (obj == obj2)
                            continue;
                        if (obj.collidesWith(obj2))
                            collision = true;
                    }
                    foreach (Sprite obj2 in Game1.spriteManager.PowerUps)
                    {
                        if (obj == obj2)
                            continue;
                        if (obj.collidesWith(obj2))
                            collision = true;
                    }
                }
                while(collision);
            }
        }

        /// <summary>
        /// Generates a random map position for an object to be placed at.
        /// </summary>
        /// <returns>A random map position</returns>
        public Vector2 randomMapPosition()
        {
            Vector2 pos = new Vector2(random.Next((int)lrBorderSize.X, (int)(mapSize.X - lrBorderSize.X)),
                random.Next((int)tbBorderSize.Y, (int)(mapSize.Y - tbBorderSize.Y)));
            return mapPosition + pos;
        }

        /// <summary>
        /// This returns an enumeration that signifies player direction, (North, NorthEast, East, etc.)
        /// </summary>
        /// <returns>player direction as PlayerDirection Enum</returns>
        public PlayerDirection getPlayerDirection()
        {
            return playerDirection;
        }
    }
}
