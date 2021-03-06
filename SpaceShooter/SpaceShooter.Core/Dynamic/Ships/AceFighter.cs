﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter.Dynamic.Ships
{
    class AceFighter : EnemyShip
    {
        enum FighterAiState
        {
            Wander,
            Alert,
            Chase,
            Catch,
            Evade
        }

        const float maxSpeed = 16 * TileSize;
        const float alertDistance = 900f;
        const float chaseDistance = 700f;
        const float catchDistance = 5f;
        const float hysteresis = 15f;
        const float durability = 5000;
        const float collisionDamage = 100;
        const int score = 1500;
        float attackTimer = 15;

        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();
        FighterAiState aiState = FighterAiState.Wander;

        public override int Score { get { return score; } }
        protected override Rectangle PlayArea { get { return ExtendedPlayArea; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        

        public AceFighter(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
            weapons.Add(new Machinegun());
            weapons.Add(new RocketLauncher());
            activeWeapon = weapons[0];

            activeWeapon.MagazineSize = 3;
            activeWeapon.MagazineCount = 3;
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            attackTimer -= (float)e.ElapsedSeconds;

            PlayerShip target = GetNearestPlayer();
            if (target == null)
                return;

            Vector2 closeFightPosition;
            Vector2 chasingDirection;
            Vector2 shootingDirection = target.Position - Position;
            shootingDirection.Normalize();
            
            float alertThreshold = alertDistance;
            float chaseThreshold = chaseDistance;
            float catchThreshold = catchDistance;

            if (attackTimer >= 5 && attackTimer <= 15)
            {
                closeFightPosition = target.Position + new Vector2(200, 0);
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

                Velocity = Camera.Velocity;

                catchThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (attackTimer <= 5 && aiState == FighterAiState.Catch)
            {
                activeWeapon = weapons[1];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));

                Velocity = Camera.Velocity;

                catchThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
            }
            else if (aiState == FighterAiState.Evade)
            {
                Vector2 evadeDirection = new Vector2(0, 256);
                //Velocity = new Vector2(0, 256);
                evadeDirection.Normalize();

                Position += evadeDirection * (float)e.ElapsedSeconds * maxSpeed * 2;                                
            }
            Controller controller = target.Player.Controller;

            float distanceFromNearestPlayer = Vector2.Distance(Position, target.Position);
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

            base.OnUpdate(e);
        }
    }
}
