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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Menu menu;
        MainMenu mainMenu;


        // Changed these to constants and declared here so the size can be set in the constructor.
        // Graphics information.
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        private const Boolean FULL_SCREEN = false;

        // All the sprite lists in the game.
        public static SpriteManager PowerUps;
        public static SpriteManager Nuts;
        public static SpriteManager Enemies;
        public static SpriteManager Obstacles;
        public static SpriteManager Hometree;
        public static SpriteManager Hero;

        public static GameStates gameState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            map = new Map(this);
            Components.Add(map);

            // Setup sprite managers.
            Obstacles = new SpriteManager(this);
            PowerUps = new SpriteManager(this);
            Nuts = new SpriteManager(this);
            Enemies = new SpriteManager(this);
            Hometree = new SpriteManager(this);
            Hero = new SpriteManager(this);

            
            // The order here maters.
            Components.Add(PowerUps);
            Components.Add(Nuts);
            Components.Add(Enemies);
            Components.Add(Obstacles);
            Components.Add(Hometree);
            Components.Add(Hero);

            //Hometree = new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Home_Tree"), Vector2.Zero);

            menu = new Menu(this);
            Components.Add(menu);
            mainMenu = new MainMenu(this);
            Components.Add(mainMenu);

            gameState = GameStates.Main_Menu;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // Testing stuff...

            //Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero));
            //Obstacles.Add(new AnimatedSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            //Enemies.Sprites.Add(new AnimatedSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            //Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Rock_1"), new Vector2(128, 256), Point.Zero));
            //Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Rock_2"), Vector2.Zero, Point.Zero));
            //Obstacles.Sprites.Add(new AnimatedSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            //Hometree.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Home_Tree"), Vector2.Zero));
            
            
            //Hero.Sprites.Add(new Player(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Rock_3"), new Vector2(0, 0)));
            Obstacles.Sprites[0].moveTo(Obstacles.Sprites[0].center());
            Obstacles.Sprites[0].collisionOffset.Y = -32; // Bring in the top and bottom by 16 pixels each.
            Obstacles.Sprites[0].collisionOffset.X = -16; 
            Obstacles.Sprites[0].collisionCenter.Y = 48;
            Hero.Sprites.Add(new Player(Content.Load<Texture2D>(@"sampleSpritesheet"), Vector2.Zero, new Point(128, 128), Point.Zero, new Point(4, 4), 16));
            Hero.Sprites[0].collisionOffset.X = -16;
            Hero.Sprites[0].collisionOffset.Y = -16;
            //Hero.Sprites[0].collisionCenter.Y = -128;
            Hero.Sprites[0].moveTo(Hero.Sprites[0].center());

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //skips over all game logic while game is paused
            if (gameState == GameStates.Paused)
            {
                menu.Update(gameTime);  //needed so the update functionality in the menu class still works while paused
                return;
            }
            else if (gameState == GameStates.Main_Menu)
            {
                mainMenu.Update(gameTime);  //needed so the mainmenu functionality still updates
                return;
            }
            else
            {
                //opens the game menu
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gameState = GameStates.Paused;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // Section for drawing sprites.




            base.Draw(gameTime);
        }
    }
}
