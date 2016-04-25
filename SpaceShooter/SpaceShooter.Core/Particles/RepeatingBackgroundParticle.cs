using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Particles
{
    class DustParticle : BackgroundParticle
    {
        protected Color color;

        protected override Color Color { get { return color; } }

        public DustParticle(Texture2D texture, Vector2 position, float distance, float brightness)
            : base(texture, position, distance)
        {
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
