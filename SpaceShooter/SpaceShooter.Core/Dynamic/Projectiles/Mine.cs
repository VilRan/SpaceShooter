﻿using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    class Mine : Projectile
    {
        const float hitRadius = 35f;
        const float durability = 10f;
        const float collisionDamage = 10;

        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Mine(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.BulletTexture, level, position, velocity, durability, faction)
        {

        }
        
        public override void OnDeath(DeathEventArgs e)
        {
            Fragment.Emit(Level, Faction, Position, 40, 80);
        }
    }
}
