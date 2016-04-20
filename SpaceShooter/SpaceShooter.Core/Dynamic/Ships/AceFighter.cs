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
    class AceFighter : Ship
    {
        enum FighterAiState
        {
            Wander,
            Alert,
            Chase,
            Catch,
            Evade
        }

        const float maxSpeed = 256;
        const float alertDistance = 900f;
        const float chaseDistance = 700f;
        const float catchDistance = 5f;
        const float hysteresis = 15f;
        const float durability = 5000;
        const float collisionDamage = 100;
        const int score = 50;
        float attackTimer = 15;

        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();
        FighterAiState aiState = FighterAiState.Wander;

        public override int Score { get { return score; } }
        protected override Rectangle PlayArea { get { return ExtendedPlayArea; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        

        public AceFighter(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability, Faction.Enemy)
        {
            weapons.Add(new Machinegun());
            weapons.Add(new RocketLauncher());
            activeWeapon = weapons[0];

            activeWeapon.MagazineSize = 3;
            activeWeapon.MagazineCount = 3;
        }

        public override void Update(UpdateEventArgs e)
        {
            attackTimer -= (float)e.ElapsedSeconds;

            Player nearestPlayer = Level.Session.Players.
                Where(player => !player.Ship.IsDying).
                OrderBy(player => (player.Ship.Position - Position).LengthSquared()).
                FirstOrDefault();
            if (nearestPlayer == null)
                return;

            Vector2 closeFightPosition;
            Vector2 chasingDirection;
            Vector2 shootingDirection = nearestPlayer.Ship.Position - Position;
            shootingDirection.Normalize();
            
            float alertThreshold = alertDistance;
            float chaseThreshold = chaseDistance;
            float catchThreshold = catchDistance;

            if (attackTimer >= 5 && attackTimer <= 15)
            {
                closeFightPosition = nearestPlayer.Ship.Position + new Vector2(200, 0);
                chasingDirection = closeFightPosition - Position;
                chasingDirection.Normalize();
            }
            else
            {
                closeFightPosition = PlayArea.Center.ToVector2();                
                chasingDirection = closeFightPosition - Position;
                chasingDirection.Normalize();

                chaseThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;                
            }

            if (aiState == FighterAiState.Wander)
            {
                Velocity = new Vector2(0, 0);

                alertThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Alert)
            {
                activeWeapon = weapons[1];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));

                alertThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                chaseThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Chase)
            {
                Position += chasingDirection * (float)e.ElapsedSeconds * maxSpeed;

                chaseThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (attackTimer > 5 && aiState == FighterAiState.Catch)
            {
                activeWeapon = weapons[0];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(-1, 0), this));

                Velocity = Level.Camera.Velocity;

                catchThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (attackTimer <= 5 && aiState == FighterAiState.Catch)
            {
                activeWeapon = weapons[1];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));

                Velocity = Level.Camera.Velocity;

                catchThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Evade)
            {
                Vector2 evadeDirection = new Vector2(0, 256);
                //Velocity = new Vector2(0, 256);
                evadeDirection.Normalize();

                Position += evadeDirection * (float)e.ElapsedSeconds * maxSpeed * 2;                                
            }
            Controller controller = nearestPlayer.Controller;

            float distanceFromNearestPlayer = Vector2.Distance(Position, nearestPlayer.Ship.Position);
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
            if (distanceFromNearestPlayer < 300 && controller.IsControlDown(Action.Fire))            
            {
                aiState = FighterAiState.Evade;
            }

            if (attackTimer <= 0)
            {
                attackTimer = 15;
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
