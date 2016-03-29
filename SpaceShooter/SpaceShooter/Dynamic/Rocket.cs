﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace SpaceShooter.Dynamic
{
    class Rocket : DynamicObject
    {
        static Vector2 acceleration = new Vector2(2048, 0);
        const float hitRadius = 3f;

        public override float HitRadius { get { return hitRadius; } }
        double boostTimer = 0.2;
        static Vector2 maxVelocity = new Vector2(2048, 0);


        public Rocket(AssetManager assets, Vector2 position, Vector2 velocity)
            : base(assets.BulletTexture)
        {
            Durability.Both = 10;
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (boostTimer > 0)
            {
                boostTimer -= e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else 
            {
                Velocity += acceleration * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
                if (Velocity.LengthSquared() > maxVelocity.LengthSquared())
                    Velocity = maxVelocity;
            }
            

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Durability.Current -= 500;
        }
    }
}
