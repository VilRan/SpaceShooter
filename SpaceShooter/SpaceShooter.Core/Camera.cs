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

        public void Update(GameTime gameTime, Rectangle limits)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.X < limits.Left)
            {
                Position.X = limits.Left;
                if (Velocity.X < 0)
                    Velocity.X = 0;
            }
            if (Position.Y < limits.Top)
            {
                Position.Y = limits.Top;
                if (Velocity.Y < 0)
                    Velocity.Y = 0;
            }
            if (Position.X > limits.Right - Bounds.Width)
            {
                Position.X = limits.Right - Bounds.Width;
                if (Velocity.X > 0)
                    Velocity.X = 0;
            }
            if (Position.Y > limits.Bottom - Bounds.Height)
            {
                Position.Y = limits.Bottom - Bounds.Height;
                if (Velocity.Y > 0)
                    Velocity.Y = 0;
            }
        }
    }
}
