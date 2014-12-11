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
    public class Interface : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        SpriteFont spriteFont_Calibri;
        Texture2D nutBackground;

        public Interface(Game game)
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
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteFont_Calibri = Game.Content.Load<SpriteFont>("fonts/spriteFont_Calibri");
            nutBackground = Game.Content.Load<Texture2D>("acorn");
            base.LoadContent();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);   
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game1.gameState == GameStates.Active)
            {
                spriteBatch.Begin();
                {
                    spriteBatch.Draw(nutBackground, new Vector2(608, 648), Color.White);
                    spriteBatch.DrawString(spriteFont_Calibri, ((Player)Game1.spriteManager.Hero).nutsHeld.ToString(), new Vector2(644, 665), Color.Blue);
                }
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
