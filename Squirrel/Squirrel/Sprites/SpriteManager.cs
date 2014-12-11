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
        public List<Projectile> Projectiles = new List<Projectile>(); // Bullets and such.
        public Sprite Hero; // The player.
        public Sprite HomeTree; // The center hometree.
        private Texture2D bullet;
        
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
            bullet = Game.Content.Load<Texture2D>(@"Textures\bulletS");


            // Hero animations.
            Texture2D playerMove = Game.Content.Load<Texture2D>(@"Textures\Goat\playerMoveGun");
            Texture2D playerIdle = Game.Content.Load<Texture2D>(@"Textures\Goat\playerIdleGun");
            

            Hero = new Player(playerIdle, Vector2.Zero, new Point(92, 92), Point.Zero, new Point(8, 1), 256);
            ((Creature)Hero).move = new Animation(playerMove, new Point(92, 92), Point.Zero, new Point(8, 1), 16);
            Hero.moveTo(Hero.center());
           

            //Hero = new Player(Game.Content.Load<Texture2D>(@"sampleSpritesheet"), Vector2.Zero, new Point(128, 128), Point.Zero, new Point(4, 4), 16);
            //Hero = new StaticSprite(Game.Content.Load<Texture2D>(@"sampleSpritesheet"), Vector2.Zero);
            //Obstacles.Add(new StaticSprite(Game.Content.Load<Texture2D>(@"Textures\Static\Rock_3"), new Vector2(0, 0)));
            //Obstacles.Add(new StaticSprite(Game.Content.Load<Texture2D>(@"Textures\Static\Rock_3"), new Vector2(0, 0), new Point(-32, -64), new Point(0, 16)));
            
            //Hero.collisionOffset.Y = -16;
            //Hero.collisionOffset.X = -8;
            //Hero.moveTo(Hero.center());
            Enemy x = new StandardEnemy(Game.Content.Load<Texture2D>(@"Textures\Goat\darkGoatIdle"), Vector2.Zero, new Point(92, 92), Point.Zero, new Point(8, 1), 256);
            Enemies.Add(x);
            Texture2D xy = Game.Content.Load<Texture2D>(@"Textures\Goat\darkGoatMove");
            x.move = new Animation(xy, new Point(92, 92), Point.Zero, new Point(8, 1), 64);


            HomeTree = new StaticSprite(Game.Content.Load<Texture2D>(@"Textures\Static\Home_Tree"), new Vector2(Hero.position.X - 290, Hero.position.Y - 650));
            HomeTree.collisionOffset.X = -132;
            HomeTree.collisionOffset.Y = -309;
            HomeTree.collisionCenter.Y = -240;
            Obstacles.Add(HomeTree);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            CleanUp();
            // Update each sprite in the order it was drawn.
            foreach (Sprite s in Obstacles)
            {
                s.Update(gameTime);
            }
            foreach (Sprite s in Nuts)
            {
                s.Update(gameTime);
            }
            foreach (Sprite s in Enemies)
            {
                s.Update(gameTime);
            }
            foreach (Sprite s in Projectiles)
            {
                s.Update(gameTime);
            }
            Hero.Update(gameTime);
            base.Update(gameTime);
        }

        // Removes stuffs.
        private void CleanUp()
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i].dead || Projectiles[i].hit)
                {
                    Projectiles.RemoveAt(i);
                }
            }
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].dead)
                {
                    Enemies.RemoveAt(i);
                }
            }
            for (int i = 0; i < Nuts.Count; i++)
            {
                if (Nuts[i].dead)
                {
                    Nuts.RemoveAt(i);
                }
            }
        }


        public void Shoot(Vector2 position, Vector2 direction)
        {
            Vector2 reposition;
            if (direction.X > 0)
            {
                reposition = new Vector2(position.X + 84, position.Y + 62);
            }
            else
            {
                reposition = new Vector2(position.X + 4, position.Y + 62);
            }
            
            
            this.Projectiles.Add(new Projectile(bullet, reposition, direction));
        }






        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            {
                foreach (Sprite s in Obstacles)
                {
                    s.Draw(gameTime, spriteBatch);
                }
                foreach (Sprite s in Nuts)
                {
                    s.Draw(gameTime, spriteBatch);
                }
                foreach (Sprite s in Enemies)
                {
                    s.Draw(gameTime, spriteBatch);
                }
                foreach (Sprite s in Projectiles)
                {
                    s.Draw(gameTime, spriteBatch);
                }

                 


                Hero.Draw(gameTime, spriteBatch);
                //HomeTree.Draw(gameTime, spriteBatch);
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
