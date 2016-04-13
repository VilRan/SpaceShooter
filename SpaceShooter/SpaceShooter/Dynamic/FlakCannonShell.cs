using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class FlakCannonShell : Projectile
    {
        const float durability = 10;
        const float collisionDamage = 1;

        double flyingTime = 0.6;

        protected override Color Color { get { return Color.LightGray; } }

        public FlakCannonShell(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, durability)
        {
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (flyingTime > 0)
            {
                flyingTime -= e.ElapsedSeconds;
            }
            else
            {
                Die();
            }
            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            DynamicObject other = e.Other;
            other.Damage(new DamageEventArgs(e, collisionDamage));
        }

        public override void OnDeath(DeathEventArgs e)
        {
            SpaceShooterGame game = Level.Game;
            Random random = game.Random;
            int n = random.Next(40, 80);
            for (int i = 0; i < n; i++)
            {
                Vector2 velocity = new Vector2(512, 0);
                Matrix rotation = Matrix.CreateRotationZ((float)(random.NextDouble() * Math.PI * 2));

                velocity = Vector2.TransformNormal(velocity, rotation);

                Fragment fragment = new Fragment(Level, Position, velocity, Faction.Player);
                fragment.Lifespan = random.NextDouble();
                Level.Objects.Add(fragment);
            }
        }
    }
}
