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

        protected override Color Color { get { return Color.White; } }

        public Wall(AssetManager assets)
            : base(assets.TileTexture)
        {

        }
    }
}
