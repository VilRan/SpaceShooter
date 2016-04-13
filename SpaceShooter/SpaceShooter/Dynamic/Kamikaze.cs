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
    class Kamikaze : DynamicObject
    {
        enum KamikazeAiState
        {
            Wander,
            Alert,
            Chase,
            Catch
        }

        const float maxSpeed = 256;
        const float alertDistance = 900f;
        const float chaseDistance = 700f;
        const float catchDistance = 25f;
        const float hysteresis = 15f;

        Weapon activeWeapon;
        KamikazeAiState aiState = KamikazeAiState.Wander;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Kamikaze(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, 500)
        {
            Faction = Faction.Enemy;
            activeWeapon = new Machinegun();

            activeWeapon.MagazineSize = 3;
            activeWeapon.MagazineCount = 3;
        }

        public override void Update(UpdateEventArgs e)
        {
            Player nearestPlayer = Level.Session.Players.
                Where(player => !player.Ship.IsDying).
                OrderBy(player => (player.Ship.Position - Position).LengthSquared()).
                FirstOrDefault();
            if (nearestPlayer == null)
                return;

            Vector2 shootingDirection = nearestPlayer.Ship.Position - Position;
            shootingDirection.Normalize();

            Vector2 chasingDirection = nearestPlayer.Ship.Position - Position;
            chasingDirection.Normalize();

            float alertThreshold = alertDistance;
            float chaseThreshold = chaseDistance;
            float catchThreshold = catchDistance;

            if (aiState == KamikazeAiState.Wander)
            {
                Velocity = new Vector2(0, 0);

                alertThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == KamikazeAiState.Alert)
            {
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));

                alertThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                chaseThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == KamikazeAiState.Chase)
            {
                Position += chasingDirection * (float)e.ElapsedSeconds * maxSpeed;

                chaseThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == KamikazeAiState.Catch)
            {
                SpaceShooterGame game = Level.Game;
                Random random = game.Random;

                Die();

                int n = random.Next(40, 80);
                for (int i = 0; i < n; i++)
                {
                    Vector2 velocity = new Vector2(512, 0);
                    Matrix rotation = Matrix.CreateRotationZ((float)(random.NextDouble() * Math.PI * 2));

                    velocity = Vector2.TransformNormal(velocity, rotation);

                    Fragment fragment = new Fragment(Level, Position, velocity, Faction.Enemy);
                    fragment.Lifespan = random.NextDouble();
                    Level.Objects.Add(fragment);
                }


                catchThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }

            float distanceFromPlayer = Vector2.Distance(Position, nearestPlayer.Ship.Position);
            if (distanceFromPlayer > alertThreshold)
            {
                aiState = KamikazeAiState.Wander;
            }
            else if (distanceFromPlayer > chaseThreshold)
            {
                aiState = KamikazeAiState.Alert;
            }
            else if (distanceFromPlayer > catchThreshold)
            {
                aiState = KamikazeAiState.Chase;
            }
            else
            {
                aiState = KamikazeAiState.Catch;
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
