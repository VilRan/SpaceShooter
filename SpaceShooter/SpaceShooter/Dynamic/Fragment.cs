﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Fragment : DynamicObject
    {
        public double Lifespan;

        const float hitRadius = 3f;

        public override float HitRadius { get { return hitRadius; } }

        public Fragment(AssetManager assets, Vector2 position, Vector2 velocity)
            : base(assets.BulletTexture)
        {
            Durability.Both = 10;
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (Lifespan <= 0)
                Durability.Current = 0;
            Lifespan -= e.GameTime.ElapsedGameTime.TotalSeconds;

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Durability.Current -= 100;
        }
    }
}