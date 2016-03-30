using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Particles
{
    public abstract class Particle : GameObject
    {
        protected override Color Color { get { return Color.LightGray; } }
        public abstract bool IsRemoving { get; }

        public Particle(Texture2D texture)
            : base(texture)
        {

        }

        public override void Draw(DrawEventArgs e)
        {
            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            base.Draw(e);
        }
    }
}
