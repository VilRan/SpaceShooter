using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Particle : GameObject
    {
        public double Lifespan;

        protected override Color Color { get { return Color.LightGray; } }

        public Particle(Texture2D texture)
            : base(texture)
        {

        }

        public override void Draw(DrawEventArgs e)
        {
            Lifespan -= e.GameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            base.Draw(e);
        }
    }
}
