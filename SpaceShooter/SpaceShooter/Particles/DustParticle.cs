using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Particles
{
    class DustParticle : BackgroundParticle
    {
        protected Color color;

        protected override Color Color { get { return color; } }

        public DustParticle(Texture2D texture, Random random)
            : base(texture, (float)random.NextDouble())
        {
            float brightness = (float)random.NextDouble();
            color = new Color(brightness, brightness, brightness);
        }

        public override void Draw(DrawEventArgs e)
        {
            base.Draw(e);
            if (Position.X < e.Level.PlayArea.Left)
                Position.X = e.Level.PlayArea.Right;
        }
    }
}
