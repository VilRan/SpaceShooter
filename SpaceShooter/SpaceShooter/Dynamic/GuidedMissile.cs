using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace SpaceShooter.Dynamic
{
    class GuidedMissile : DynamicObject
    {
        const float hitRadius = 3f;

        public override float HitRadius { get { return hitRadius; } }
        const float speed = 1024;

        public GuidedMissile(AssetManager assets, Vector2 position, Vector2 velocity)
            : base(assets.BulletTexture)
        {
            Durability.Both = 10;
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {            
            DynamicObject target = e.Level.Objects.
                Where(obj => obj.Faction!=Faction).
                OrderBy(obj => (obj.Position - Position).LengthSquared()).
                First();
            if (target != null)
            {                
                Vector2? intercept = FindInterceptPoint(Position, Vector2.Zero, target.Position, target.Velocity, speed);
                if(intercept != null)
                {
                    Vector2 direction = intercept.Value - Position;                    
                    direction.Normalize();

                    Vector2 velocity = direction * speed;

                    Velocity = Vector2.Lerp(Velocity, velocity, 0.03f);
                }                
            }            

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Durability.Current -= 500;
        }

        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 shooterVelocity, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2 relativePosition = targetPosition - shooterPosition;
            Vector2 relativeVelocity = targetVelocity - shooterVelocity;
            float a = projectileSpeed * projectileSpeed - relativeVelocity.LengthSquared();
            float b = -2 * Vector2.Dot(relativeVelocity, relativePosition);
            float c = -relativePosition.LengthSquared();
            float d = b * b - 4 * a * c;
            if (d > 0)
            {
                float result = (b + (float)Math.Sqrt(d)) / (2 * a);
                return targetPosition + result * relativeVelocity;
            }
            return null;
        }
    }
}