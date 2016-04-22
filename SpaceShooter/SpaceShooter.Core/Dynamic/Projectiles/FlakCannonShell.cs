using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    class FlakCannonShell : Projectile
    {
        const float durability = 10;
        const float collisionDamage = 1;

        double flyingTime = 0.6;

        protected override Color Color { get { return Color.LightGray; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public FlakCannonShell(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, position, velocity, durability, Faction.Player)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            if (flyingTime > 0)
            {
                flyingTime -= e.ElapsedSeconds;
            }
            else
            {
                Die();
            }
            base.OnUpdate(e);
        }
        
        public override void OnDeath(DeathEventArgs e)
        {
            Fragment.Emit(Level, Faction, Position, 40, 80);
        }
    }
}
