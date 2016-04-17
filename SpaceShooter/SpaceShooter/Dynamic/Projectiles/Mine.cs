using Microsoft.Xna.Framework;
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
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Mine(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.BulletTexture, level, durability)
        {
            Position = position;
            Velocity = velocity;
            Faction = faction;
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            base.OnCollision(e);
            Fragment.Emit(Level, Faction, Position, 40, 80);
        }
    }
}
