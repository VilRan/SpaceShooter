using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter.Dynamic
{
    class Fragment : Projectile
    {
        const float hitRadius = 3f;
        const float durability = 10;
        const float collisionDamage = 100;

        public double Lifespan;

        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Fragment(Level level, Vector2 position, Vector2 velocity, Faction faction, double lifespan)
            : base(level.Game.Assets.MachineGunBulletTexture, level, position, velocity, durability, faction)
        {
            Lifespan = lifespan;
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            if (Lifespan <= 0)
                Die();
            Lifespan -= e.ElapsedSeconds;

            base.OnUpdate(e);
        }

        public static void Emit(Level level, Faction faction, Vector2 position, int minCount, int maxCount)
        {
            SpaceShooterGame game = level.Game;
            Random random = game.Random;
            int n = random.Next(minCount, maxCount + 1);
            Fragment[] fragments = new Fragment[n];
            for (int i = 0; i < n; i++)
            {
                double lifespan = 0.25 + 0.25 * random.NextDouble();
                Vector2 velocity = VectorUtility.CreateRandom(random, 1024, 1024);
                Fragment fragment = new Fragment(level, position, velocity, faction, lifespan);
                fragments[i] = fragment;
            }
            level.Objects.AddRange(fragments);
        }
    }
}
