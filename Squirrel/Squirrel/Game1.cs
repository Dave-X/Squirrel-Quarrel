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
        public static Map map;
        Menu menu;
        MainMenu mainMenu;
        Texture2D test1;
        Texture2D test2;
        Texture2D scoreBoard;
        SpriteFont font;
        public static KeyboardState keyboardState, oldKeyboardState;
        public static SpriteManager spriteManager;
        public static Interface iface;


        // Changed these to constants and declared here so the size can be set in the constructor.
        // Graphics information.
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        private const Boolean FULL_SCREEN = false;

        // All the sprite lists in the game.


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
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            menu = new Menu(this);
            Components.Add(menu);
            mainMenu = new MainMenu(this);
            Components.Add(mainMenu);
            iface = new Interface(this);
            Components.Add(iface);

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

            scoreBoard = Content.Load<Texture2D>("scoreLabel");
            font = Content.Load<SpriteFont>("Calibri");

            // Testing stuff...

            //Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero));
            //Obstacles.Add(new AnimatedSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            //Enemies.Sprites.Add(new AnimatedSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            //Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Rock_1"), new Vector2(128, 256), Point.Zero));
            //Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Rock_2"), Vector2.Zero, Point.Zero));
            //Obstacles.Sprites.Add(new AnimatedSprite(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            //Hometree.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Home_Tree"), Vector2.Zero));
            
            
            //Hero.Sprites.Add(new Player(Content.Load<Texture2D>(@"sampleSpriteSheet"), Vector2.Zero, new Point(128, 128), new Point(0, 0), new Point(4, 4), 16));
            
            /*
            // holy comments

            Obstacles.Sprites.Add(new StaticSprite(Content.Load<Texture2D>(@"Textures\Static\Rock_3"), new Vector2(0, 0)));
            Obstacles.Sprites[0].moveTo(Obstacles.Sprites[0].center());
            Obstacles.Sprites[0].collisionOffset.Y = -32; // Bring in the top and bottom by 16 pixels each.
            Obstacles.Sprites[0].collisionOffset.X = -16; 
            Obstacles.Sprites[0].collisionCenter.Y = 48;
            
            Hero.Sprites[0].collisionOffset.X = -16;
            Hero.Sprites[0].collisionOffset.Y = -16;
            //Hero.Sprites[0].collisionCenter.Y = -128;
            Hero.Sprites[0].moveTo(Hero.Sprites[0].center());

            */


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
            //check if player is dead
            if ((Game1.spriteManager.Hero as Player).Health <= 0)
            {
                gameState = GameStates.Game_Over;
            }

            keyboardState = Keyboard.GetState();
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
            else if (gameState == GameStates.Game_Over)
            {
                System.Diagnostics.Debug.WriteLine("game over");
                mainMenu.gameOver = true;
                mainMenu.Update(gameTime);  //needed so the mainmenu functionality still updates
                return;
            }
            else
            {
                //opens the game menu
                if (oldKeyboardState.IsKeyUp(Keys.Escape) && keyboardState.IsKeyDown(Keys.Escape))
                    gameState = GameStates.Paused;
            }
            oldKeyboardState = Keyboard.GetState();

            //check for returning nuts to home tree
            if (spriteManager.Hero.collidesWith(spriteManager.HomeTree))
            {
                (spriteManager.Hero as Player).dropOff();
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

            base.Draw(gameTime);

            spriteBatch.Begin();

            //draw the score board
            if (gameState == GameStates.Active || gameState == GameStates.Paused)
            {
                spriteBatch.Draw(scoreBoard, new Rectangle(1004, 20, 256, 64), Color.White);
                spriteBatch.DrawString(font, (Game1.spriteManager.Hero as Player).totalCollected.ToString(), new Vector2(1180, 23), Color.DarkGreen);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public void resetGame()
        {
            //delete old
            Components.Remove(map);
            Components.Remove(spriteManager);
            Components.Remove(menu);
            Components.Remove(mainMenu);
            Components.Remove(iface);

            map = new Map(this);
            Components.Add(map);
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            menu = new Menu(this);
            Components.Add(menu);
            mainMenu = new MainMenu(this);
            Components.Add(mainMenu);
            iface = new Interface(this);
            Components.Add(iface);
        }
    }
}
