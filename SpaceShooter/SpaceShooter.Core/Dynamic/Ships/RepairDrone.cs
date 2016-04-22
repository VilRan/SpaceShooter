using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.Ships
{
    class RepairDrone : Ship
    {
        enum DroneAiState
        {
            Wander,
            Seek,
            Repair
        }

        const float maxSpeed = 8 * TileSize;
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
            : base(level.Game.Assets.AsteroidTexture, level, position, durability, Faction.Neutral)
        {

        }


        public override void OnUpdate(UpdateEventArgs e)
        {
            Velocity = new Vector2(80, 0);

            DynamicObject target = null;
            float nearest = float.MaxValue;
            foreach (DynamicObject obj in Level.Objects)
            {
                if (obj.Faction == Faction || obj.Category != ObjectCategory.Ship || obj.Faction == Faction.Player)
                    continue;
                float distance = (obj.Position - Position).LengthSquared();
                if (obj.CurrentDurability < obj.MaximumDurability && distance < nearest)
                {
                    target = obj;
                    nearest = distance;
                }
            }

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

        public override void OnCollision(Collision collision)
        {
            base.OnCollision(collision);
            TimedParticle.Emit(Level, collision.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            SoundEffectInstance sound = Level.Game.Assets.ExplosionSound.CreateInstance();
            sound.Volume = (float)(0.5 + 0.5 * Level.Game.Random.NextDouble());
            sound.Play();
        }
    }
}
