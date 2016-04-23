using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    public class Wall : GameObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Tile; } }
        public override Vector2 Origin { get { return Vector2.Zero; } }
        public RectangleCollider Collider { get { return new RectangleCollider((int)Position.X, (int)Position.Y, TileSize, TileSize); } }

        protected override Color Color { get { return Color.Gray; } }
        
        public Wall(AssetManager assets, Vector2 position)
            : base(assets.TileTexture, position, Vector2.Zero)
        {

        }
    }
}
