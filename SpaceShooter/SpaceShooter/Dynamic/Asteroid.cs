using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class Asteroid : DynamicObject
    {
        const float durability = 1000;
        const float collisionDamage = 100;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public Asteroid(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, durability)
        {

        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, collisionDamage));
            TimedParticle.Emit(Level, e.CollisionPosition, 20, 40);
        }
    }
}
