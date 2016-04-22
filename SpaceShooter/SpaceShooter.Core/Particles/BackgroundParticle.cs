using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Particles
{
    class BackgroundParticle : Particle
    {
        float distance;

        public override bool IsRemoving { get { return false; } }
        public override ObjectCategory Category { get { return ObjectCategory.Particle; } }

        public BackgroundParticle(Texture2D texture, Vector2 position, float distance)
            : base(texture, position, Vector2.Zero)
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
