using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class RailgunProjectile : DynamicObject
    {
        const float hitRadius = 64f;

        public override float HitRadius { get { return hitRadius; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }

        public RailgunProjectile(AssetManager assets, Vector2 position, Vector2 velocity)
            : base(assets.BulletTexture, 2000)
        {
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, 2000));
        }
    }
}
