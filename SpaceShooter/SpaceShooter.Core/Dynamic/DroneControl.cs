using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic
{
    class DroneControl : DynamicObject
    {
        const float hitRadius = 10f;
        const float durability = 10;
        const float collisionDamage = 0f;

        public override float HitRadius { get { return hitRadius; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        public override ObjectCategory Category { get { return ObjectCategory.PowerUp; } }

        public DroneControl(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, position, velocity, durability, Faction.Neutral)
        {

        }

        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);            
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            MachinegunDrone machinegunDrone = new MachinegunDrone(Level, Position, Level.Camera.Velocity);
            Level.Objects.Add(machinegunDrone);
        }
        protected override bool CanCollideWith(DynamicObject other)
        {
            if (other is PlayerShip)
                return true;

            return false;
        }
    }
}

