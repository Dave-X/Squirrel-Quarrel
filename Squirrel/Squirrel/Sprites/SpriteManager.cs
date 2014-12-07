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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;

        public List<Sprite> Obstacles = new List<Sprite>(); // Stores all the sprites.
        public List<Sprite> PowerUps = new List<Sprite>();
        public List<Sprite> Nuts = new List<Sprite>();
        public List<Sprite> Enemies = new List<Sprite>();
        public List<Sprite> Projectiles = new List<Sprite>(); // Bullets and such.
        public Sprite Hero; // The player.
        public Sprite HomeTree; // The center hometree.
        
        public SpriteManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Hero = new Player(Game.Content.Load<Texture2D>(@"sampleSpritesheet"), Vector2.Zero, new Point(128, 128), Point.Zero, new Point(4, 4), 16);
            //Hero = new StaticSprite(Game.Content.Load<Texture2D>(@"sampleSpritesheet"), Vector2.Zero);
            Obstacles.Add(new StaticSprite(Game.Content.Load<Texture2D>(@"Textures\Static\Rock_3"), new Vector2(0, 0)));
            Hero.moveTo(Hero.center());

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update each sprite in the order it was drawn.
            foreach (Sprite s in Obstacles)
            {
                s.Update(gameTime);
            }
            Hero.Update(gameTime);
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            {
                foreach (Sprite s in Obstacles)
                {
                    s.Draw(gameTime, spriteBatch);
                }
                Hero.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}


/*
 * Draw Order
 * Map
 * Power ups
 * Nuts
 * Player
 * Enemies
 * Obstacles
 * Hometree
 * Menu
 * 
 * Componets Order
 * Map
 * SpriteManager
 * Menu
*/ 
