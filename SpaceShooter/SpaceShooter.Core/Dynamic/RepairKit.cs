using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic.Ships;

namespace SpaceShooter.Dynamic
{
    class RepairKit : DynamicObject
    {
        const float hitRadius = 10f;
        const float durability = 10;
        const float collisionDamage = 0f;

        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        public override ObjectCategory Category { get { return ObjectCategory.PowerUp; } }
        public RepairKit(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, position, velocity, durability, Faction.Neutral)
        {

        }

        public override void OnCollision(Collision e)
        {
            e.Other.Repair(200);
        }

        protected override bool CanCollideWith(DynamicObject other)
        {
            if (other is PlayerShip)
                return true;

            return false;
        }
    }
}
