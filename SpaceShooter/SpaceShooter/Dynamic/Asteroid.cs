using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Asteroid : DynamicObject
    {
        const float durability = 1000;
        const float collisionDamage = 100;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Asteroid(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, durability)
        {

        }

        public override void OnCollision(CollisionEventArgs e)
        {
            SpaceShooterGame game = Level.Game;

            DynamicObject other = e.Other;
            other.Damage(new DamageEventArgs(e, collisionDamage));

            Vector2 thisCollisionPosition = Position + Velocity * e.TimeOfCollision;
            Vector2 otherCollisionPosition = other.Position + other.Velocity * e.TimeOfCollision;
            Vector2 collisionPosition = (thisCollisionPosition - otherCollisionPosition) * (other.HitRadius / (HitRadius + other.HitRadius)) + otherCollisionPosition;

            Random random = game.Random;
            int n = random.Next(20, 40);
            Particle[] particles = new Particle[n];
            for (int i = 0; i < n; i++)
            {
                double lifespan = random.NextDouble();
                TimedParticle particle = new TimedParticle(game.Assets.ParticleTexture, lifespan);

                particle.Position = collisionPosition;
                particle.Velocity = VectorUtility.CreateRandom(random, 1000);

                particles[i] = particle;
            }
            Level.Particles.AddRange(particles);
        }
    }
}
