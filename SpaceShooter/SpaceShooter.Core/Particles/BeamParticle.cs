using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Particles
{
    public class BeamParticle : Particle
    {
        Color color;

        public override bool IsRemoving { get { return true; } }

        public BeamParticle(Texture2D texture, Vector2 position, Vector2 velocity, Color color)
            : base(texture, position, velocity)
        {
            this.color = color;
        }

        public override void Draw(DrawEventArgs e)
        {
            int length = (int)Velocity.Length();
            Point start = (Position - Origin - e.Level.Camera.Position).ToPoint();
            Point size = Texture.Bounds.Size + new Point(length, 0);
            Rectangle destination = new Rectangle(start, size);
            Rectangle source = new Rectangle(Point.Zero, size);
            e.SpriteBatch.Draw(Texture, destination, source, color);
        }

        public override GameObject Clone()
        {
            throw new NotImplementedException();
        }
    }
}
