using Microsoft.Xna.Framework;
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
        public Color StartColor = Color.White;
        public Color EndColor = Color.TransparentBlack;
        Color color;
        double lifespan;
        double life;

        public override bool IsRemoving { get { return life <= 0; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        protected override Color Color { get { return color; } }

        public TimedParticle(Texture2D texture, double lifespan)
            : base(texture)
        {
            this.lifespan = lifespan;
            life = lifespan;
        }

        public override void Draw(DrawEventArgs e)
        {
            if (!e.Level.PlayArea.Contains(Position))
                life = 0;
            else
                life -= e.GameTime.ElapsedGameTime.TotalSeconds;

            color = Color.Lerp(EndColor, StartColor, (float)(life / lifespan));

            base.Draw(e);
        }
    }
}
