using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class FlakCannonShell : DynamicObject
    {
        double flyingTime = 0.6;

        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        protected override Color Color { get { return Color.LightGray; } }

        public FlakCannonShell(AssetManager assets, Vector2 position, Vector2 velocity)
            : base(assets.BulletTexture)
        {
            Durability.Both = 10;
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (flyingTime > 0)
            {
                flyingTime -= e.GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Durability.Current = 0;
            }
            base.Update(e);
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            DynamicObject other = e.Other;
            other.Durability.Current -= 1;
        }

        public override void OnDeath(DeathEventArgs e)
        {
            SpaceShooterGame game = e.Level.Game;
            Random random = game.Random;
            int n = random.Next(20, 40);
            for (int i = 0; i < n; i++)
            {
                Vector2 velocity = new Vector2(512, 0);
                Matrix rotation = Matrix.CreateRotationZ((float)(random.NextDouble() * Math.PI * 2));

                velocity = Vector2.TransformNormal(velocity, rotation);

                Fragment fragment = new Fragment(game.Assets, Position, velocity);
                fragment.Lifespan = random.NextDouble();
                e.Level.Objects.Add(fragment);
            }
        }
    }
}
