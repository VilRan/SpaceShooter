using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.PowerUps
{
    class DroneControl : PowerUp
    {
        public DroneControl(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, position, velocity)
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
            MachinegunDrone machinegunDrone = new MachinegunDrone(Level, Position, Camera.Velocity);
            Level.Objects.Add(machinegunDrone);
        }
    }
}

