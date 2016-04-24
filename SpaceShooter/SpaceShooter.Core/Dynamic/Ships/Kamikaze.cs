using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System.Linq;

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

        const float maxSpeed = 16 * TileSize;
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

        public override void OnUpdate(UpdateEventArgs e)
        {
            DynamicObject target = GetNearestPlayer();
            if (target == null)
                return;

            Vector2 shootingDirection = target.Position - Position;
            shootingDirection.Normalize();

            Vector2 chasingDirection = target.Position - Position;
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

            float distanceFromTarget = Vector2.Distance(Position, target.Position);
            if (distanceFromTarget > alertThreshold)
            {
                aiState = KamikazeAiState.Wander;
            }
            else if (distanceFromTarget > chaseThreshold)
            {
                aiState = KamikazeAiState.Alert;
            }
            else if (distanceFromTarget > catchThreshold)
            {
                aiState = KamikazeAiState.Chase;
            }
            else
            {
                aiState = KamikazeAiState.Catch;
            }

            base.OnUpdate(e);
        }

        public override void OnCollision(Collision e)
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
