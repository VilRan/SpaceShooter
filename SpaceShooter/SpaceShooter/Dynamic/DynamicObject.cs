using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    public enum Faction
    {
        Player,
        Enemy
    }

    public abstract class DynamicObject : GameObject
    {
        private const float hitRadius = 12;

        public Level Level;
        public Durability Durability = new Durability();
        public Faction Faction = Faction.Enemy;

        public bool IsDying { get { return Durability.Current <= 0; } }
        public virtual float HitRadius { get { return hitRadius; } }
        protected SpaceShooterGame Game { get { return Level.Game; } }
        protected override Color Color { get { return Color.White; } }

        public DynamicObject(Texture2D texture, Level level)
            : base(texture)
        {
            Level = level;
        }

        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Position.X < 0 || Position.X > 1024 || Position.Y < 0 || Position.Y > 768)
                Durability.Current = 0;
        }

        public void CheckCollisions(GameTime gameTime, int startIndex)
        {
            for (int i = startIndex; i < Level.Objects.Count; i++)
            {
                DynamicObject other = Level.Objects[i];
                if (other.Faction == Faction)
                    continue;

                bool isCollision = false;
                float timeOfClosestApproach;
                FindClosestApproach(other, out timeOfClosestApproach, out isCollision);
                if (timeOfClosestApproach > gameTime.ElapsedGameTime.TotalSeconds)
                    isCollision = false;

                if (isCollision)
                {
                    OnCollision(new CollisionEventArgs(other, timeOfClosestApproach));
                    other.OnCollision(new CollisionEventArgs(this, timeOfClosestApproach));
                }
            }
        }

        public void FindClosestApproach(DynamicObject other, out float timeOfClosestApproach, out bool isCollision)
        {
            Vector2 positionDelta = Position - other.Position;
            Vector2 velocityDelta = Velocity - other.Velocity;
            float a = Vector2.Dot(velocityDelta, velocityDelta);
            float b = 2 * Vector2.Dot(positionDelta, velocityDelta);
            float c = Vector2.Dot(positionDelta, positionDelta) - (HitRadius + other.HitRadius) * (HitRadius + other.HitRadius);
            float discriminant = b * b - 4 * a * c;
            
            if (discriminant < 0)
            {
                timeOfClosestApproach = -b / (2 * a);
                isCollision = false;
            }
            else
            {
                float time1 = (-b + (float)Math.Sqrt(discriminant)) / (2 * a);
                float time2 = (-b - (float)Math.Sqrt(discriminant)) / (2 * a);
                timeOfClosestApproach = Math.Min(time1, time2);
                
                if (timeOfClosestApproach < 0)
                    isCollision = false;
                else
                    isCollision = true;
            }
            
            if (timeOfClosestApproach < 0)
                timeOfClosestApproach = 0;
        }

        public virtual void OnCollision(CollisionEventArgs e)
        {

        }
    }

    public class CollisionEventArgs
    {
        public readonly DynamicObject Other;
        public readonly float TimeOfCollision;

        public CollisionEventArgs(DynamicObject other, float timeOfCollision)
        {
            Other = other;
            TimeOfCollision = timeOfCollision;
        }
    }
}
