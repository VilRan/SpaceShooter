using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Fragment : Projectile
    {
        const float hitRadius = 3f;
        const float durability = 10;
        const float collisionDamage = 100;

        public double Lifespan;
        
        public override float HitRadius { get { return hitRadius; } }

        public Fragment(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, durability)
        {
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (Lifespan <= 0)
                Die();
            Lifespan -= e.GameTime.ElapsedGameTime.TotalSeconds;

            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, collisionDamage));
        }
    }
}
