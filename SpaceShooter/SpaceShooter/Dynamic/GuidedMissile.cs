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
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        const float speed = 1024;

        public GuidedMissile(AssetManager assets, Vector2 position, Vector2 velocity)
            : base(assets.BulletTexture, 10)
        {
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            DynamicObject target = null;
            float nearest = float.MaxValue;
            foreach(DynamicObject obj in e.Level.Objects)
            {
                if (obj.Faction == Faction || obj.Category != ObjectCategory.Ship)
                    continue;
                float distance = (obj.Position - Position).LengthSquared();
                if(distance < nearest)
                {
                    target = obj;
                    nearest = distance;
                }
            }

            if (target != null)
            {                
                Vector2? intercept = VectorUtility.FindInterceptPoint(Position, Vector2.Zero, target.Position, target.Velocity, speed);
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
            e.Other.Damage(new DamageEventArgs(e.Level, 500));
        }
    }
}