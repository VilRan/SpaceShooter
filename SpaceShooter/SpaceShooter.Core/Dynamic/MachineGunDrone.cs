using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
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
            DynamicObject target = null;
            float nearest = float.MaxValue;
            foreach (DynamicObject obj in Level.Objects)
            {
                if (obj.Faction == Faction || obj.Category != ObjectCategory.Ship)
                    continue;
                float distance = (obj.Position - Position).LengthSquared();
                if (distance < nearest)
                {
                    target = obj;
                    nearest = distance;
                }
            }
            Vector2 direction = new Vector2(0, 0);
            if (target != null)
            {
                direction = target.Position - Position;
                direction.Normalize();
            }
            weapon.Update(e.GameTime);
            weapon.TryFire(new FireEventArgs(Level, Position, direction, this));

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
