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
    class AdvancedFighter : EnemyShip
    {
        enum FighterAiState
        {
            Wander,
            Alert,
            Chase,
            Catch
        }

        const float maxSpeed = 8 * TileSize;
        const float alertDistance = 900f;
        const float chaseDistance = 700f;
        const float catchDistance = 5f;
        const float hysteresis = 15f;
        const float durability = 500;
        const float collisionDamage = 100;
        const int score = 50;
        const double repairKitDropChance = 0.3;
        
        Weapon weapon;
        //List<Weapon> weapons = new List<Weapon>();
        FighterAiState aiState = FighterAiState.Wander;

        public override int Score { get { return score; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        
        public AdvancedFighter(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
            /*
            weapons.Add(new Machinegun());
            weapons.Add(new RocketLauncher());
            activeWeapon = weapons[0];
            */
            weapon = new Machinegun();

            weapon.MagazineSize = 3;
            weapon.MagazineCount = 3;
        }

        public override void Update(UpdateEventArgs e)
        {
            Player nearestPlayer = Level.Session.Players.
                Where(player => !player.Ship.IsDying).
                OrderBy(player => (player.Ship.Position - Position).LengthSquared()).
                FirstOrDefault();
            if (nearestPlayer == null)
                return;

            Vector2 closeFightPosition = nearestPlayer.Ship.Position + new Vector2(200, 0);

            Vector2 shootingDirection = nearestPlayer.Ship.Position - Position;
            shootingDirection.Normalize();

            Vector2 chasingDirection = closeFightPosition - Position;
            chasingDirection.Normalize();

            float alertThreshold = alertDistance;
            float chaseThreshold = chaseDistance;
            float catchThreshold = catchDistance;

            if (aiState == FighterAiState.Wander)
            {
                Velocity = new Vector2(0, 0);

                alertThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Alert)
            {
                //activeWeapon = weapons[0];
                weapon.Update(e.GameTime);
                weapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));

                alertThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                chaseThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Chase)
            {
                /*
                activeWeapon = weapons[0];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position, shootingDirection, this));
                */
                Position += chasingDirection * (float)e.ElapsedSeconds * maxSpeed;

                chaseThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Catch)
            {
                //activeWeapon = weapons[0];
                weapon.Update(e.GameTime);
                weapon.TryFire(new FireEventArgs(Level, Position, new Vector2(-1, 0), this));

                Velocity = Level.Camera.Velocity;

                catchThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }

            float distanceFromPlayer = Vector2.Distance(Position, closeFightPosition);
            if (distanceFromPlayer > alertThreshold)
            {
                aiState = FighterAiState.Wander;
            }
            else if (distanceFromPlayer > chaseThreshold)
            {
                aiState = FighterAiState.Alert;
            }
            else if (distanceFromPlayer > catchThreshold)
            {
                aiState = FighterAiState.Chase;
            }
            else
            {
                aiState = FighterAiState.Catch;
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
            if (random.NextDouble() < repairKitDropChance)
            {
                RepairKit repairKit = new RepairKit(Level, Position, Vector2.Zero);
                Level.Objects.Add(repairKit);
            }
        }
    }
}
