using Microsoft.Xna.Framework;


namespace SpaceShooter.Dynamic
{
    class GuidedMissile : Projectile
    {
        const float hitRadius = 3f;
        const float durability = 10;
        const float collisionDamage = 500;
        const float speed = 32 * TileSize;

        public override Vector2 AbsoluteVelocity { get { return Velocity; } }
        public override float HitRadius { get { return hitRadius; } }
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        protected override Rectangle PlayArea { get { return ExtendedPlayArea; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public GuidedMissile(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, position, velocity, durability, Faction.Player)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            DynamicObject target = null;
            float nearest = float.MaxValue;
            foreach(DynamicObject obj in Level.Objects)
            {
                if (obj.Faction == Faction || obj.Category != ObjectCategory.Ship)
                    continue;
                float distance = (obj.Position - Position).LengthSquared();
                if(distance < nearest)
                {
                    target = obj;
                    nearest = distance;
                }
            }

            if (target != null)
            {                
                Vector2? direction = VectorUtility.FindInterceptDirection(Position, Vector2.Zero, target.Position, target.AbsoluteVelocity, speed);
                if(direction != null)
                {
                    Vector2 targetVelocity = direction.Value * speed;
                    Velocity = Vector2.Lerp(Velocity, targetVelocity, 0.03f);
                }                
            }            

            base.OnUpdate(e);
        }
    }
}