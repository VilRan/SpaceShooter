using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Particle : GameObject
    {
        public double Lifespan;

        public Particle(Texture2D texture)
            : base(texture)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Lifespan -= gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Draw(gameTime, spriteBatch);
        }
    }
}
