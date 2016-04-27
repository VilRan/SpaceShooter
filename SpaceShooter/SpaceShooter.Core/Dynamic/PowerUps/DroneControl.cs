using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.PowerUps
{
    class DroneControl : PowerUp
    {
        protected override float Scale { get { return 0.75f; } }

        public DroneControl(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.PowerUpTexture, level, position, velocity)
        {

        }

        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            MachinegunDrone machinegunDrone = new MachinegunDrone(Level, Position, Camera.Velocity, e.Other);
            Level.Objects.Add(machinegunDrone);
        }
    }
}

