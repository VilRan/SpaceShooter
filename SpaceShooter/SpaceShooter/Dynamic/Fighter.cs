using Microsoft.Xna.Framework;
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
            Rockets,
            Machinegun
        }

        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();

        const float RocketUseDistance = 400f;
        const float MachinegunUseDistance = 300f;
        const float Hysteresis = 15f;

        FighterAiState fighterState = FighterAiState.Wander;

        public Fighter(AssetManager assets)
            : base(assets.AsteroidTexture)
        {
            Durability.Both = 500;
            Faction = Faction.Enemy;

            weapons.Add(new Machinegun());
            weapons.Add(new RocketLauncher());
            activeWeapon = weapons[0];

            activeWeapon.MagazineSize = 3;
            activeWeapon.MagazineCount = 3;
        }
        public override void Update(UpdateEventArgs e)
        {
            Vector2 shootingDirection = e.Level.Session.Player.Ship.Position - Position;
            shootingDirection.Normalize();

            float rocketUseThreshold = RocketUseDistance;
            float machinegunUseThreshold = MachinegunUseDistance;

            if (fighterState == FighterAiState.Wander)
            {
                Velocity = new Vector2(-1, 0);

                rocketUseThreshold -= Hysteresis / 2;
            }
            else if (fighterState == FighterAiState.Rockets)
            {
                activeWeapon = weapons[1];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position, shootingDirection, this));

                Velocity = new Vector2(128, 0);

                rocketUseThreshold += Hysteresis / 2;
                machinegunUseThreshold -= Hysteresis / 2;
            }
            else if (fighterState == FighterAiState.Machinegun)
            {
                activeWeapon = weapons[0];
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position, shootingDirection, this));

                Velocity = new Vector2(128, 0);

                machinegunUseThreshold += Hysteresis / 2;
            }

            float distanceFromPlayer = Vector2.Distance(Position, e.Level.Session.Player.Ship.Position);
            if (distanceFromPlayer > rocketUseThreshold)
            {
                fighterState = FighterAiState.Wander;
            }
            else if (distanceFromPlayer > machinegunUseThreshold)
            {
                fighterState = FighterAiState.Rockets;
            }
            else
            {
                fighterState = FighterAiState.Machinegun;
            }

            base.Update(e);
        }   

        public override void OnCollision(CollisionEventArgs e)
        {
            SpaceShooterGame game = e.Level.Game;

            DynamicObject other = e.Other;
            other.Durability.Current -= 100;

            Vector2 thisCollisionPosition = Position + Velocity * e.TimeOfCollision;
            Vector2 otherCollisionPosition = other.Position + other.Velocity * e.TimeOfCollision;
            Vector2 collisionPosition = (thisCollisionPosition - otherCollisionPosition) * (other.HitRadius / (HitRadius + other.HitRadius)) + otherCollisionPosition;

            Random random = game.Random;
            int n = random.Next(20, 40);
            Particle[] particles = new Particle[n];
            for (int i = 0; i < n; i++)
            {
                TimedParticle particle = new TimedParticle(game.Assets.ParticleTexture);

                particle.Position = collisionPosition;

                double direction = random.NextDouble() * Math.PI * 2;
                double speed = random.NextDouble() * 1000;
                particle.Velocity = new Vector2((float)(Math.Cos(direction) * speed), (float)(Math.Sin(direction) * speed));

                particle.Lifespan = random.NextDouble();

                particles[i] = particle;
            }
            e.Level.Particles.AddRange(particles);
        }
    }
}
