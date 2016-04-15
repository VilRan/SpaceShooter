using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic
{
    class Rocket : Projectile
    {
        const float acceleration = 2048f;
        const float maxVelocity = 2048f;
        const float maxVelocitySquared = maxVelocity * maxVelocity;
        const float hitRadius = 3f;
        const float durability = 10;
        const float collisionDamage = 500;

        public override float HitRadius { get { return hitRadius; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        double boostTimer = 0.2;


        public Rocket(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.BulletTexture, level, durability)
        {
            Position = position;
            Velocity = velocity;
            Faction = faction;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (boostTimer > 0)
            {
                boostTimer -= e.ElapsedSeconds;
            }
            else 
            {
                Vector2 direction = Vector2.Normalize(Velocity);
                Velocity += direction * acceleration * (float)e.ElapsedSeconds;
                if (Velocity.LengthSquared() > maxVelocitySquared)
                    Velocity = direction * maxVelocity;
                else
                    TimedParticle.Emit(Level, Position, Color.OrangeRed, 0.25, 0.5, 32);
            }
            

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, collisionDamage));
        }
    }
}

