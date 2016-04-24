using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    public abstract class Projectile : DynamicObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }

        public Projectile(Texture2D texture, Level level, Vector2 position, Vector2 velocity, float durability, Faction faction)
            : base(texture, level, position, velocity + level.Camera.Velocity, durability, faction)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            foreach (Wall wall in Level.Walls)
            {
                float timeOfCollision;
                if (Collider.FindCollision(wall.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                {
                    //Position += Velocity * timeOfCollision;
                    Die();
                    break;
                }
            }

            base.OnUpdate(e);
        }

        protected override bool CanCollideWith(DynamicObject other)
        {
            if (other.Category == ObjectCategory.Projectile)
                return false;
            if (other.Category == ObjectCategory.PowerUp)
                return false;
            return base.CanCollideWith(other);
        }
    }
}
