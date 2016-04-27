using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    class RailgunProjectile : Projectile
    {
        const float hitRadius = 64f;
        const float durability = 2000;
        const float collisionDamage = 2000;

        public override float HitRadius { get { return hitRadius; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public RailgunProjectile(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.MachineGunBulletTexture, level, position, velocity, durability, Faction.Player)
        {

        }
    }
}
