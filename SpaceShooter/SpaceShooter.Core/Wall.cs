using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    public class Wall : GameObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Tile; } }
        public RectangleCollider Collider { get { return new RectangleCollider((int)Position.X, (int)Position.Y, 32, 32); } }

        protected override Color Color { get { return Color.Gray; } }

        public Wall(AssetManager assets, Vector2 position)
            : base(assets.TileTexture, position, Vector2.Zero)
        {

        }
    }
}
