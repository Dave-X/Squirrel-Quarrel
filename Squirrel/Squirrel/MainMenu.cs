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
    public class MainMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D startTexture, exitTexture, background;          //background texture
        SpriteBatch spriteBatch;    //spritebatch to draw to screen
        Rectangle startClick, exitClick;       //clickable bounding boxes
        Vector2 resumeButtonPos, quitButtonPos; //positions of buttons for clicking menu items
        int buttonWidth, buttonHeight;          //dimensions of buttons
        public bool isPaused { get; set; }
        public bool gameOver = false;
        SpriteFont font;
        Game1 game1;

        public MainMenu(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            game1 = game as Game1;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            isPaused = false;

            base.Initialize();

            buttonWidth = 256;
            buttonHeight = 64;
            resumeButtonPos = new Vector2(GraphicsDevice.Viewport.Width / 2 - buttonWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - buttonHeight / 2 - 50);
            quitButtonPos = new Vector2(GraphicsDevice.Viewport.Width / 2 - buttonWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - buttonHeight / 2 + 50);
            startClick = new Rectangle((int)resumeButtonPos.X, (int)resumeButtonPos.Y, buttonWidth, buttonHeight);
            exitClick = new Rectangle((int)quitButtonPos.X, (int)quitButtonPos.Y, buttonWidth, buttonHeight);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //load in the buttons for the menu
            spriteBatch = new SpriteBatch(GraphicsDevice);
            startTexture = Game.Content.Load<Texture2D>("startButton");
            exitTexture = Game.Content.Load<Texture2D>("exitButton");
            background = Game.Content.Load<Texture2D>("titleScreen");

            font = Game.Content.Load<SpriteFont>("CalibriLarge");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //show the mouse when the game is paused
            if (Game1.gameState == GameStates.Main_Menu || Game1.gameState == GameStates.Game_Over)
            {
                Game.IsMouseVisible = true;
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (startClick.Contains(mouseState.X, mouseState.Y))
                    {
                        if (gameOver)
                            game1.resetGame();
                        Game1.gameState = GameStates.Active;
                    }
                    if (exitClick.Contains(mouseState.X, mouseState.Y))
                        Game.Exit();
                }
            }
            else
            {
                Game.IsMouseVisible = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (Game1.gameState == GameStates.Main_Menu || Game1.gameState == GameStates.Game_Over)
            {
                spriteBatch.Begin();

                GraphicsDevice.Clear(Color.ForestGreen);

                spriteBatch.Draw(background, Vector2.Zero, Color.White);

                spriteBatch.Draw(startTexture, resumeButtonPos, Color.White);
                spriteBatch.Draw(exitTexture, quitButtonPos, Color.White);

                if (gameOver)
                {
                    spriteBatch.DrawString(font, "Game Over", new Vector2(492, 475), Color.Red);
                }

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}