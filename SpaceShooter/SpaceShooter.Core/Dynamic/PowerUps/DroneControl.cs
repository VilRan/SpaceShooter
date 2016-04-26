using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.PowerUps
{
    class DroneControl : PowerUp
    {
        public DroneControl(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.PowerUpTexture, level, position, velocity)
        {

        }

        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, Assets.ParticleTexture, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
            MachinegunDrone machinegunDrone = new MachinegunDrone(Level, Position, Camera.Velocity, e.Other);
            Level.Objects.Add(machinegunDrone);
        }
    }
}

