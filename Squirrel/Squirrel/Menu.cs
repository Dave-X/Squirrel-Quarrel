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
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D resumeTexture, quitTexture;          //map background texture
        SpriteBatch spriteBatch;    //spritebatch to draw to screen
        Rectangle resumeClick, quitClick;       //clickable bounding boxes
        Vector2 resumeButtonPos, quitButtonPos; //positions of buttons for clicking menu items
        int buttonWidth, buttonHeight;          //dimensions of buttons
        MouseState mouseState, oldMouseState;
        KeyboardState keyboardState, oldKeyboardState;
        public bool isPaused { get; set; }

        public Menu(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
            resumeClick = new Rectangle((int)resumeButtonPos.X, (int)resumeButtonPos.Y, buttonWidth, buttonHeight);
            quitClick = new Rectangle((int)quitButtonPos.X, (int)quitButtonPos.Y, buttonWidth, buttonHeight);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //load in the buttons for the menu
            spriteBatch = new SpriteBatch(GraphicsDevice);
            resumeTexture = Game.Content.Load<Texture2D>("resumeButton");
            quitTexture = Game.Content.Load<Texture2D>("quitButton");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            Game1.keyboardState = Keyboard.GetState();
            //show the mouse when the game is paused
            if (Game1.gameState == GameStates.Paused)
            {
                Game.IsMouseVisible = true;
                if (mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (resumeClick.Contains(mouseState.X, mouseState.Y))
                        Game1.gameState = GameStates.Active;
                    if (quitClick.Contains(mouseState.X, mouseState.Y))
                        Game1.gameState = GameStates.Main_Menu;
                }
                if (Game1.oldKeyboardState.IsKeyUp(Keys.Escape) && Game1.keyboardState.IsKeyDown(Keys.Escape))
                    Game1.gameState = GameStates.Active;
            }
            else
            {
                Game.IsMouseVisible = false;
            }

            oldMouseState = Mouse.GetState();  //get mouse state to prevent multiple clicks
            Game1.oldKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (Game1.gameState == GameStates.Paused)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(resumeTexture, resumeButtonPos, Color.White);
                spriteBatch.Draw(quitTexture, quitButtonPos, Color.White);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}