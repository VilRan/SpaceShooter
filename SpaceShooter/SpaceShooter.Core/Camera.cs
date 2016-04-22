using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    public class Camera
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Velocity;

        public Rectangle Bounds { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); } }

        public Camera(Vector2 position, Vector2 size, Vector2 velocity)
        {
            Position = position;
            Size = size;
            Velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
