using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Particles
{
    class BackgroundParticle : Particle
    {
        float distance;

        public override bool IsRemoving { get { return false; } }
        public override ObjectCategory Category { get { return ObjectCategory.Particle; } }

        public BackgroundParticle(Texture2D texture, float distance)
            : base (texture)
        {
            this.distance = distance;
        }

        public override void Draw(DrawEventArgs e)
        {
            Velocity = e.Level.Camera.Velocity * distance;
            base.Draw(e);
        }
    }
}
