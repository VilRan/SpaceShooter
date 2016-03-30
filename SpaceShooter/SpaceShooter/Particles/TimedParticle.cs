using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Particles
{
    class TimedParticle : Particle
    {
        public double Lifespan = 1;

        public override bool IsRemoving { get { return Lifespan <= 0; } }

        public TimedParticle(Texture2D texture)
            : base(texture)
        {

        }

        public override void Draw(DrawEventArgs e)
        {
            if (!e.Level.PlayArea.Contains(Position))
                Lifespan = 0;
            else
                Lifespan -= e.GameTime.ElapsedGameTime.TotalSeconds;
            base.Draw(e);
        }
    }
}
