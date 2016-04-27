using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.PowerUps
{
    class RepairKit : PowerUp
    {
        float rotationPhase = 0;
        
        protected override float Rotation { get { return rotationPhase; } }
        protected override float Scale { get { return 0.5f; } }

        public RepairKit(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.RepairKitTexture, level, position, velocity)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            base.OnUpdate(e);
            rotationPhase += MathHelper.PiOver2 * (float)e.ElapsedSeconds;
        }

        public override void OnCollision(Collision e)
        {
            e.Other.Repair(1000);
            TimedParticle.Emit(Level, Game.Assets.HealCrossTexture, Position, Color.LightGreen, 0.7, 1 , 200, 10, 20);
        }
    }
}
