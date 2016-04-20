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
    class Minelayer : EnemyShip
    {
        enum MinelayerAiState
        {
            Wander,
            Alert
        }

        const float maxSpeed = 8 * TileSize;
        const float alertDistance = 700f;
        const float hysteresis = 15f;
        const float durability = 500;
        const float collisionDamage = 100;
        const int score = 50;

        Weapon activeWeapon;

        MinelayerAiState aiState = MinelayerAiState.Wander;

        public override int Score { get { return score; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Minelayer(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
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
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
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
