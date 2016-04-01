using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic
{
    class Rocket : DynamicObject
    {
        static Vector2 acceleration = new Vector2(2048, 0);
        const float hitRadius = 3f;

        public override float HitRadius { get { return hitRadius; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        double boostTimer = 0.2;
        static Vector2 maxVelocity = new Vector2(2048, 0);


        public Rocket(AssetManager assets, Vector2 position, Vector2 velocity, Faction faction)
            : base(assets.BulletTexture, 10)
        {
            Position = position;
            Velocity = velocity;
            Faction = faction;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (boostTimer > 0)
            {
                boostTimer -= e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else 
            {
                Vector2 direction = Vector2.Normalize(Velocity);
                Velocity += direction * 2048 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
                if (Velocity.LengthSquared() > maxVelocity.LengthSquared())
                    Velocity = maxVelocity;
                else
                {
                    Random random = e.Level.Game.Random;
                    double trailLifespan = 0.25 + 0.25 * random.NextDouble();
                    TimedParticle trail = new TimedParticle(e.Level.Game.Assets.ParticleTexture, trailLifespan);
                    trail.Position = Position;
                    trail.Velocity = VectorUtility.CreateRandom(random, 32);
                    trail.StartColor = Color.OrangeRed;
                    e.Level.Particles.Add(trail);
                }
            }
            

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e.Level, 500));
        }
    }
}

