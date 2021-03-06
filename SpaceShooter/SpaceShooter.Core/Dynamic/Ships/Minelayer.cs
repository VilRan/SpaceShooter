﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System.Linq;

namespace SpaceShooter.Dynamic.Ships
{
    class Minelayer : EnemyShip
    {
        enum MinelayerAiState
        {
            Wander,
            Alert
        }

        const float maxSpeed = 16 * TileSize;
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

        public override void OnUpdate(UpdateEventArgs e)
        {
            DynamicObject target = GetNearestPlayer();
            if (target == null)
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

            float distanceFromPlayer = Vector2.Distance(Position, target.Position);
            if (distanceFromPlayer > alertThreshold)
            {
                aiState = MinelayerAiState.Wander;
            }
            else
            {
                aiState = MinelayerAiState.Alert;
            }

            base.OnUpdate(e);
        }
    }
}
