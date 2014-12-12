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
    public class StatsScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D startTexture, exitTexture, background;          //background texture
        SpriteBatch spriteBatch;    //spritebatch to draw to screen
        Rectangle startClick, exitClick;       //clickable bounding boxes
        Vector2 resumeButtonPos, quitButtonPos; //positions of buttons for clicking menu items
        int buttonWidth, buttonHeight;          //dimensions of buttons
        public bool isPaused { get; set; }
        public bool gameOver = false;
        SpriteFont font, font2;
        Game1 game1;
        string endingStatus = "";

        public StatsScreen(Game game)
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
            resumeButtonPos = new Vector2(363, 534);
            quitButtonPos = new Vector2(649, 534);
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
            background = Game.Content.Load<Texture2D>("statsScreen");

            font = Game.Content.Load<SpriteFont>("CalibriLarge");
            font2 = Game.Content.Load<SpriteFont>("Calibri");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (gameOver)
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
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (gameOver)
            {
                if (Game1.gameState == GameStates.Main_Menu || Game1.gameState == GameStates.Game_Over)
                {
                    spriteBatch.Begin();

                    GraphicsDevice.Clear(Color.ForestGreen);

                    spriteBatch.Draw(background, Vector2.Zero, Color.White);

                    spriteBatch.Draw(startTexture, resumeButtonPos, Color.White);
                    spriteBatch.Draw(exitTexture, quitButtonPos, Color.White);

                    if ((Game1.spriteManager.Hero as Player).totalCollected == Game1.maxNuts)
                        endingStatus = "You Won!";
                    else
                        endingStatus = "Game Over";
                    spriteBatch.DrawString(font, endingStatus, new Vector2(492, 249), Color.Red);
                    spriteBatch.DrawString(font2, (Game1.spriteManager.Hero as Player).enemiesKilled.ToString(), new Vector2(490, 345), Color.DarkGreen);
                    spriteBatch.DrawString(font2, (Game1.spriteManager.Hero as Player).totalCollected.ToString(), new Vector2(510, 390), Color.DarkGreen);
                    spriteBatch.DrawString(font2, (Game1.spriteManager.Hero as Player).powerupsCollected.ToString(), new Vector2(620, 435), Color.DarkGreen);

                    spriteBatch.End();
                }
            }

            base.Draw(gameTime);
        }
    }
}