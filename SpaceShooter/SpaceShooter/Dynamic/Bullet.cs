using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Bullet : DynamicObject
    {
        const float speed = 1024;
        const float hitRadius = 3f;

        public override float HitRadius { get { return hitRadius; } }

        public Bullet(Level level, Vector2 position)
            : base(level.Game.Assets.BulletTexture, level)
        {
            Durability.Both = 10;
            Position = position;
            Velocity = new Vector2(speed, 0);
            Faction = Faction.Player;
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Durability.Current -= 100;
        }
    }
}
