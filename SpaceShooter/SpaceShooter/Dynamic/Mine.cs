﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Mine : Projectile
    {
        const float hitRadius = 35f;
        const float durability = 10f;
        const float collisionDamage = 10;

        public override float HitRadius { get { return hitRadius; } }

        public Mine(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.BulletTexture, level, durability)
        {
            Position = position;
            Velocity = velocity;
            Faction = faction;
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, collisionDamage));

            SpaceShooterGame game = Level.Game;
            Random random = game.Random;
            int n = random.Next(40, 80);
            for (int i = 0; i < n; i++)
            {
                Vector2 velocity = new Vector2(512, 0);
                Matrix rotation = Matrix.CreateRotationZ((float)(random.NextDouble() * Math.PI * 2));

                velocity = Vector2.TransformNormal(velocity, rotation);

                Fragment fragment = new Fragment(Level, Position, velocity, Faction.Enemy);
                fragment.Lifespan = random.NextDouble();
                Level.Objects.Add(fragment);
            }
        }
    }
}
