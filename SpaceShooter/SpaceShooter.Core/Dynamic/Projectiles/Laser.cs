using Microsoft.Xna.Framework;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.Projectiles
{
    class Laser : Projectile
    {
        const float collisionDamage = 50;
        const float durability = 10f;

        Vector2 beamLength = Vector2.Zero;

        protected override float CollisionDamage { get { return collisionDamage; } }

        public Laser(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.BulletTexture, level, position, velocity, durability, faction)
        {

        }
        
        public override void OnUpdate(UpdateEventArgs e)
        {
            if (beamLength == Vector2.Zero)
                beamLength = Velocity * (float)e.ElapsedSeconds;
            BeamParticle beam = new BeamParticle(Game.Assets.LaserTexture, Position, beamLength, Color.Red);
            Level.Particles.Add(beam);
            beamLength = Vector2.Zero;

            base.OnUpdate(e);
        }

        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            beamLength = (e.ColliderCollisionPosition - Position);
        }
    }
}
