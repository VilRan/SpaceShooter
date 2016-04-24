﻿using Microsoft.Xna.Framework;
using SpaceShooter.Weapons;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic
{
    class MachinegunDrone : DynamicObject
    {
        const float durability = 200;
        const float collisionDamage = 100;
        const float speed = 32 * TileSize;

        Weapon weapon;
        protected override float CollisionDamage { get { return collisionDamage; } }
        public override ObjectCategory Category { get { return ObjectCategory.PowerUp; } }

        public MachinegunDrone(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.AsteroidTexture, level, position, velocity, durability, Faction.Player)
        {
            weapon = new Machinegun();
            weapon.MagazineSize = 3;
            weapon.MagazineCount = 3;
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            weapon.Update(e.GameTime);

            DynamicObject target = GetNearest(obj => obj.Faction != Faction && obj.Category == ObjectCategory.Ship);
            if (target != null)
            {
                Vector2? direction = VectorUtility.FindInterceptDirection(Position, Camera.Velocity, target.Position, target.AbsoluteVelocity, weapon.ProjectileSpeed);
                if (direction != null)
                    weapon.TryFire(new FireEventArgs(Level, Position, direction.Value, this));
            }

            base.OnUpdate(e);
        }

        public override void OnCollision(Collision collision)
        {
            base.OnCollision(collision);
            TimedParticle.Emit(Level,collision.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            SoundEffectInstance sound = Level.Game.Assets.ExplosionSound.CreateInstance();
            sound.Volume = (float)(0.5 + 0.5 * Level.Game.Random.NextDouble());
            sound.Play();
        }
    }
}
