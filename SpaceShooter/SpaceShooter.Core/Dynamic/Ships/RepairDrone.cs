using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.Ships
{
    class RepairDrone : EnemyShip
    {
        enum DroneAiState
        {
            Wander,
            Seek,
            Repair
        }

        const float maxSpeed = 24 * TileSize;
        const float durability = 500;
        const int score = 50;
        const float collisionDamage = 50;
        const float seekingDistance = 5000f;
        const float repairDistance = 2000f;
        const float hysteresis = 15f;

        DroneAiState aiState = DroneAiState.Wander;
        public override int Score { get { return score; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public RepairDrone(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {

        }
        
        public override void OnUpdate(UpdateEventArgs e)
        {
            Velocity = new Vector2(80, 0);

            DynamicObject target = GetNearest(obj =>
                obj.Faction == Faction
                && obj.Category == ObjectCategory.Ship
                && obj.CurrentDurability < obj.MaximumDurability);

            float seekingThreshold = seekingDistance;
            float repairThreshold = repairDistance;
            Vector2 direction = Vector2.Zero;
            if (target != null)
            {
                Vector2 repairPosition = target.Position;
                direction = repairPosition - Position;
                direction.Normalize();

                if (aiState == DroneAiState.Wander)
                {
                    Velocity = new Vector2(80, 0);
                    seekingThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
                }
                else if (aiState == DroneAiState.Seek)
                {
                    Position += direction * (float)e.ElapsedSeconds * maxSpeed;

                    seekingThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                    repairThreshold -= hysteresis / 2 * (float)e.ElapsedSeconds;
                }
                else if (aiState == DroneAiState.Repair)
                {
                    Velocity = target.Velocity;
                    target.Repair(50 * (float)e.ElapsedSeconds);
                    repairThreshold += hysteresis / 2 * (float)e.ElapsedSeconds;
                }
                float distanceToTarget = (target.Position - Position).LengthSquared();
                if (target != null && distanceToTarget > seekingThreshold)
                {
                    aiState = DroneAiState.Wander;
                }
                if (target != null && distanceToTarget > repairDistance)
                {
                    aiState = DroneAiState.Seek;
                }
                else aiState = DroneAiState.Repair;
                if (target.CurrentDurability >= target.MaximumDurability)
                {
                    aiState = DroneAiState.Wander;
                }
            }

            base.OnUpdate(e);
        }
    }
}
