using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Dynamic.Ships
{
    public abstract class EnemyShip : Ship
    {
        public EnemyShip(Texture2D texture, Level level, Vector2 position, float durability)
            : base(texture, level, position, durability, Faction.Enemy)
        {

        }

        protected override bool CanCollideWith(DynamicObject other)
        {
            if (other.Category == ObjectCategory.PowerUp)
                return false;
            return base.CanCollideWith(other);
        }
    }
}
