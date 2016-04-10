﻿using Microsoft.Xna.Framework;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Fighter : DynamicObject
    {
        enum FighterAiState
        {
            Wander,
            Alert,
            Chase,
            Catch
        }

        const float maxSpeed = 256;
        const float alertDistance = 900f;
        const float chaseDistance = 700f;
        const float catchDistance = 5f;
        const float hysteresis = 15f;

        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();
        FighterAiState fighterState = FighterAiState.Wander;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Fighter(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, 500)
        {
            Faction = Faction.Enemy;

            weapons.Add(new Machinegun());
            weapons.Add(new RocketLauncher());
            activeWeapon = weapons[0];

            activeWeapon.MagazineSize = 3;
            activeWeapon.MagazineCount = 3;
        }

        public override void Update(UpdateEventArgs e)
        {
            Vector2 closeFightPosition = Level.Session.Player.Ship.Position + new Vector2(200, 0);

            Vector2 shootingDirection = Level.Session.Player.Ship.Position - Position;
            shootingDirection.Normalize();

            Vector2 chasingDirection = closeFightPosition - Position;
            chasingDirection.Normalize();

            float alertThreshold = alertDistance;
            float chaseThreshold = chaseDistance;
            float catchThreshold = catchDistance;

            if (fighterState == FighterAiState.Wander)
            {
                Velocity = new Vector2(-1, 0);

                alertThreshold -= hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (fighterState == FighterAiState.Alert)
            {
                activeWeapon = weapons[1];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));
                
                alertThreshold += hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
                chaseThreshold -= hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (fighterState == FighterAiState.Chase)
            {
                /*
                activeWeapon = weapons[0];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position, shootingDirection, this));
                                
                */
                Position += chasingDirection * (float)e.GameTime.ElapsedGameTime.TotalSeconds * maxSpeed;

                chaseThreshold += hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
                catchThreshold -= hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (fighterState == FighterAiState.Catch)
            {
                activeWeapon = weapons[0];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(-1, 0), this));
                                
                Velocity = new Vector2(128, 0);

                catchThreshold += hysteresis / 2 * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            }

            float distanceFromPlayer = Vector2.Distance(Position, closeFightPosition);
            if (distanceFromPlayer > alertThreshold)
            {
                fighterState = FighterAiState.Wander;
            }
            else if (distanceFromPlayer > chaseThreshold)
            {
                fighterState = FighterAiState.Alert;
            }
            else if (distanceFromPlayer > catchThreshold)
            {
                fighterState = FighterAiState.Chase;
            }
            else
            {
                fighterState = FighterAiState.Catch;
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
    }
}
