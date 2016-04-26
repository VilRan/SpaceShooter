using Microsoft.Xna.Framework;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic
{
    class Rocket : Projectile
    {
        const float acceleration = 64 * TileSize;
        const float maxVelocity = 64 * TileSize;
        const float maxVelocitySquared = maxVelocity * maxVelocity;
        const float hitRadius = 3f;
        const float durability = 10;
        const float collisionDamage = 500;

        double boostTimer = 0.2;

        public override float HitRadius { get { return hitRadius; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        
        public Rocket(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.RocketBulletTexture, level, position, velocity, durability, faction)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            if (boostTimer > 0)
            {
                boostTimer -= e.ElapsedSeconds;
            }
            else 
            {
                Vector2 direction = Vector2.Normalize(Velocity);
                Velocity += direction * acceleration * (float)e.ElapsedSeconds;
                if (Velocity.LengthSquared() > maxVelocitySquared)
                    Velocity = direction * maxVelocity;
                else
                    TimedParticle.Emit(Level, Position, Color.OrangeRed, 0.25, 0.5, 32);
            }
            

            base.OnUpdate(e);
        }
    }
}

