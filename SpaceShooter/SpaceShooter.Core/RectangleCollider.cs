using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    public struct RectangleCollider
    {
        public Rectangle Bounds;

        public RectangleCollider(int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);
        }

        public RectangleCollider(Rectangle bounds)
        {
            Bounds = bounds;
        }
    }
}
