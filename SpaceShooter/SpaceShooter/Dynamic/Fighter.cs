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
        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();
        double shootingTimer = 1;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Fighter(AssetManager assets)
            : base(assets.AsteroidTexture)
        {
            Durability.Both = 500;
            Faction = Faction.Enemy;

            weapons.Add(new Machinegun());
            weapons.Add(new RocketLauncher());
            activeWeapon = weapons[0];

            activeWeapon.MagazineSize = 5;
            activeWeapon.MagazineCount = 5;
        }

        public override void Update(UpdateEventArgs e)
        {
            Vector2 shootingDirection = e.Level.Session.Player.Ship.Position - Position;
            shootingDirection.Normalize();

            if(shootingTimer > 0)
            {
                shootingTimer -= e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                activeWeapon.Update(e.GameTime);
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position, shootingDirection, this));
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
