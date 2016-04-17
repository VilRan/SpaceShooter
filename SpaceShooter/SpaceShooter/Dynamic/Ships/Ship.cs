using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic.Ships
{
    public abstract class Ship : DynamicObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Ship(Texture2D texture, Level level, float durability)
            : base(texture, level, durability)
        {

        }
    }
}
