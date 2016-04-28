using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter.Dynamic
{
    public abstract class Projectile : DynamicObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        public override Vector2 AbsoluteVelocity { get { return Velocity + Level.Camera.Velocity; } }
        public override Vector2 RelativeVelocity { get { return Velocity; } set  { Velocity = value; } }
        protected override float Rotation { get { return (float)Math.Atan2(Velocity.Y, Velocity.X); } }

        public Projectile(Texture2D texture, Level level, Vector2 position, Vector2 velocity, float durability, Faction faction)
            : base(texture, level, position, velocity, durability, faction)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            // TODO: Wall collisions should be detected together with object collisions, so that they can be properly ordered.
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
