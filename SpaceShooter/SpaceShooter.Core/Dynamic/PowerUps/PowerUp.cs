using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Dynamic.PowerUps
{
    abstract class PowerUp : DynamicObject
    {
        const float hitRadius = 10f;
        const float durability = 10;
        const float collisionDamage = 0f;

        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        public override ObjectCategory Category { get { return ObjectCategory.PowerUp; } }

        public PowerUp(Texture2D texture, Level level, Vector2 position, Vector2 velocity)
            : base(texture, level, position, velocity, durability, Faction.Neutral)
        {

        }

        protected override bool CanCollideWith(DynamicObject other)
        {
            if (other is PlayerShip)
                return true;

            return false;
        }
    }
}
