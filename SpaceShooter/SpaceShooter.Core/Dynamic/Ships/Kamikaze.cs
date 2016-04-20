using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic.Ships
{
    class Kamikaze : EnemyShip
    {
        enum KamikazeAiState
        {
            Wander,
            Alert,
            Chase,
            Catch
        }

        const float maxSpeed = 8 * TileSize;
        const float alertDistance = 900f;
        const float chaseDistance = 700f;
        const float catchDistance = 25f;
        const float hysteresis = 15f;
        const float durability = 500;
        const float collisionDamage = 100;
        const int score = 200;

        Weapon activeWeapon;
        KamikazeAiState aiState = KamikazeAiState.Wander;

        public override int Score { get { return score; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Kamikaze(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
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
                RelativeVelocity = chasingDirection * maxSpeed;

                chaseThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == KamikazeAiState.Catch)
            {
                Die();

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
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            Fragment.Emit(Level, Faction, Position, 40, 80);
            SoundEffectInstance sound = Level.Game.Assets.ExplosionSound.CreateInstance();
            sound.Volume = (float)(0.5 + 0.5 * Level.Game.Random.NextDouble());
            sound.Play();
        }
    }
}
