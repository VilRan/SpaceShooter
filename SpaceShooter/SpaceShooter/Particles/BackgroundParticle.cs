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
        Color color;
        float distance;

        public override bool IsRemoving { get { return false; } }
        public override ObjectCategory Category { get { return ObjectCategory.Particle; } }
        protected override Color Color { get { return color; } }

        public BackgroundParticle(Texture2D texture, Random random)
            : base (texture)
        {
            distance = (float)random.NextDouble();
            float brightness = (float)random.NextDouble();
            color = new Color(brightness, brightness, brightness);
        }

        public override void Draw(DrawEventArgs e)
        {
            Velocity = e.Level.Camera.Velocity * distance;
            if (Position.X < e.Level.PlayArea.Left)
                Position.X = e.Level.PlayArea.Right;
            base.Draw(e);
        }
    }
}
