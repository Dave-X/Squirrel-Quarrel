using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Squirrel
{
    public class Projectile : Sprite
    {
        public Vector2 direction; // The direction and speed this projectile is traveling.
        public int timeAlive = 0; // How long this projectile has been going.
        public int lifeSpan = 1000;
        public bool hit = false;
        public bool dead
        {
            get
            {
                return timeAlive >= lifeSpan;
            }
        }



        public Projectile(Texture2D image, Vector2 position, Vector2 direction)
            : base(image, position)
        {
            this.direction = direction;
            // Override draw depth
            this.drawDepth = 1.0f;
        }

        public override void Update(GameTime gameTime)
        {
            timeAlive += gameTime.ElapsedGameTime.Milliseconds;
            this.position += direction;

            // Check if it hit an enemy.
            foreach (Creature c in Game1.spriteManager.Enemies)
            {
                if (this.collidesWith(c))
                {
                    this.hit = true;
                    c.takeDamage(25);
                    ((Enemy)c).red = true;
                    if (c.Health <= 0)
                    {
                        c.dead = true;
                    }
                }
            }            
        }


    }
}
