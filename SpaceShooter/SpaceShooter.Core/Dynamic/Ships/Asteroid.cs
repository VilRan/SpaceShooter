using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Particles;
using System;

namespace SpaceShooter.Dynamic.Ships
{
    class Asteroid : Ship
    {
        const float baseDurability = 3000;
        const float baseCollisionDamage = 3000;
        const float baseHitRadius = 23;
        const int score = 50;

        Color color;
        float rotationPhase = 0;
        float rotationSpeed = 0;
        float size = 1;

        public override int Score { get { return score; } }
        public override float HitRadius { get { return baseHitRadius * size; } }
        protected override float CollisionDamage { get { return baseCollisionDamage * size * size; } }
        protected override float Rotation { get { return rotationPhase; } }
        protected override float Scale { get { return size; } }
        protected override Color Color { get { return color; } }
        
        public Asteroid(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, baseDurability, Faction.Neutral)
        {
            rotationSpeed = (float)(Random.NextDouble() - Random.NextDouble());
            size += (float)Math.Abs(2 * (Random.NextDouble() - Random.NextDouble()));
            Durability = baseDurability * size * size;

            int brightness = Random.Next(100, 200);
            color = new Color(brightness, brightness, brightness);
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            base.OnUpdate(e);
            rotationPhase += rotationSpeed * (float)e.ElapsedSeconds;
        }
    }
}
