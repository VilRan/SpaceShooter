using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.Ships
{
    class Asteroid : EnemyShip
    {
        const float durability = 3000;
        const float collisionDamage = 1000;
        const float hitRadius = 40;
        const int score = 50;

        float rotationPhase = 0;
        float rotationSpeed = 0;

        public override int Score { get { return score; } }
        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Asteroid(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
            rotationSpeed = (float)(Random.NextDouble() - Random.NextDouble());
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            base.OnUpdate(e);
            rotationPhase += rotationSpeed * (float)e.ElapsedSeconds;
        }

        public override void Draw(DrawEventArgs e)
        {
            Vector2 screenPosition = Position - e.Level.Camera.Position;
            e.SpriteBatch.Draw(Texture, screenPosition, Texture.Bounds, Color, rotationPhase, Origin, 2f, SpriteEffects.None, 0);
        }
    }
}
