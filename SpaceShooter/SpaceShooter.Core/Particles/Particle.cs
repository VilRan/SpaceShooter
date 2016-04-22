using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Particles
{
    public abstract class Particle : GameObject
    {
        public override ObjectCategory Category
        {
            get
            {
                return ObjectCategory.Particle;
            }
        }
        protected override Color Color { get { return Color.LightGray; } }
        public abstract bool IsRemoving { get; }

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position, velocity)
        {

        }

        public override void Draw(DrawEventArgs e)
        {
            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            base.Draw(e);
        }
    }
}
