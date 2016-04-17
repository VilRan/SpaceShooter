using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic.Ships
{
    class Asteroid : Ship
    {
        const float durability = 1000;
        const float collisionDamage = 100;

        protected override float CollisionDamage { get { return collisionDamage; } }

        public Asteroid(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, durability)
        {

        }

        public override void OnCollision(CollisionEventArgs e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }
    }
}
