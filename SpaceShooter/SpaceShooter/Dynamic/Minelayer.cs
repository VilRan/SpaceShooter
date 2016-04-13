using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Minelayer : DynamicObject
    {
        enum MinelayerAiState
        {
            Wander,
            Alert
        }

        const float maxSpeed = 256;
        const float alertDistance = 700f;
        const float hysteresis = 15f;

        Weapon activeWeapon;

        MinelayerAiState aiState = MinelayerAiState.Wander;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Minelayer(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, 500)
        {
            Faction = Faction.Enemy;

            activeWeapon = new MineLauncher();

            activeWeapon.MagazineSize = 1;
            activeWeapon.MagazineCount = 1;
        }

        public override void Update(UpdateEventArgs e)
        {
            Player nearestPlayer = Level.Session.Players.
                Where(player => !player.Ship.IsDying).
                OrderBy(player => (player.Ship.Position - Position).LengthSquared()).
                FirstOrDefault();
            if (nearestPlayer == null)
                return;

            float alertThreshold = alertDistance;

            if (aiState == MinelayerAiState.Wander)
            {
                Velocity = new Vector2(64, 0);

                alertThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == MinelayerAiState.Alert)
            {
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(0, 0), this));

                alertThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }

            float distanceFromPlayer = Vector2.Distance(Position, nearestPlayer.Ship.Position);
            if (distanceFromPlayer > alertThreshold)
            {
                aiState = MinelayerAiState.Wander;
            }
            else
            {
                aiState = MinelayerAiState.Alert;
            }

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            SpaceShooterGame game = Level.Game;

            DynamicObject other = e.Other;
            other.Damage(new DamageEventArgs(e, 100));

            Vector2 thisCollisionPosition = Position + Velocity * e.TimeOfCollision;
            Vector2 otherCollisionPosition = other.Position + other.Velocity * e.TimeOfCollision;
            Vector2 collisionPosition = (thisCollisionPosition - otherCollisionPosition) * (other.HitRadius / (HitRadius + other.HitRadius)) + otherCollisionPosition;

            Random random = game.Random;
            int n = random.Next(20, 40);
            Particle[] particles = new Particle[n];
            for (int i = 0; i < n; i++)
            {
                double particleLifespan = random.NextDouble();
                TimedParticle particle = new TimedParticle(game.Assets.ParticleTexture, particleLifespan);

                particle.Position = collisionPosition;

                double direction = random.NextDouble() * Math.PI * 2;
                double speed = random.NextDouble() * 1000;
                particle.Velocity = new Vector2((float)(Math.Cos(direction) * speed), (float)(Math.Sin(direction) * speed));

                particles[i] = particle;
            }
            Level.Particles.AddRange(particles);
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
