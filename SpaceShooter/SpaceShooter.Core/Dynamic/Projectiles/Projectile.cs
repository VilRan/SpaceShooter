using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter.Dynamic
{
    public abstract class Projectile : DynamicObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        public float Rotation { get { return (float)Math.Atan2(Velocity.Y, Velocity.X); } }
        public override Vector2 AbsoluteVelocity { get { return Velocity + Level.Camera.Velocity; } }
        public override Vector2 RelativeVelocity { get { return Velocity; } set  { Velocity = value; } }

        public Projectile(Texture2D texture, Level level, Vector2 position, Vector2 velocity, float durability, Faction faction)
            : base(texture, level, position, velocity, durability, faction)
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

        public override void Draw(DrawEventArgs e)
        {
            Vector2 screenPosition = Position - e.Level.Camera.Position;
            e.SpriteBatch.Draw(Texture, screenPosition, Texture.Bounds, Color, Rotation, Origin, 1f, SpriteEffects.None, 0);
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
