﻿using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    class Bullet : Projectile
    {
        const float hitRadius = 3f;
        const float durability = 10f;
        const float collisionDamage = 100;

        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Bullet(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.MachineGunBulletTexture, level, position, velocity, durability, faction)
        {

        }
    }
}
