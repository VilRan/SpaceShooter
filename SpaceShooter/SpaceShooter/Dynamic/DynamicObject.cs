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

        public Durability Durability = new Durability();
        public Faction Faction = Faction.Enemy;

        public bool IsDying { get { return Durability.Current <= 0; } }
        public virtual float HitRadius { get { return hitRadius; } }
        protected override Color Color { get { return Color.White; } }

        public DynamicObject(Texture2D texture)
            : base(texture)
        {

        }

        public bool TryActivate(Level level)
        {
            if (level.PlayArea.Contains(Position))
            {
                level.Objects.Add(this);
                level.Inactive.Remove(this);
                return true;
            }
            return false;
        }

        public virtual void Update(UpdateEventArgs e)
        {
            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            
            if (!e.Level.PlayArea.Contains(Position))
                Durability.Current = 0;
        }

        public void CheckCollisions(GameTime gameTime, Level level, int startIndex)
        {
            for (int i = startIndex; i < level.Objects.Count; i++)
            {
                if (IsDying)
                    return;

                DynamicObject other = level.Objects[i];
                if (other.Faction == Faction || other.IsDying)
                    continue;

                bool isCollision = false;
                float timeOfClosestApproach;
                FindClosestApproach(other, out timeOfClosestApproach, out isCollision);
                if (timeOfClosestApproach > gameTime.ElapsedGameTime.TotalSeconds)
                    isCollision = false;

                if (isCollision)
                {
                    OnCollision(new CollisionEventArgs(level, other, timeOfClosestApproach));
                    other.OnCollision(new CollisionEventArgs(level, this, timeOfClosestApproach));
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

        public virtual void OnDeath(DeathEventArgs e)
        {

        }

        public virtual DynamicObject Clone()
        {
            return (DynamicObject)MemberwiseClone();
        }
    }

    public class UpdateEventArgs
    {
        public readonly Level Level;
        public readonly GameTime GameTime;

        public UpdateEventArgs(Level level, GameTime gameTime)
        {
            Level = level;
            GameTime = gameTime;
        }
    }

    public class CollisionEventArgs
    {
        public readonly Level Level;
        public readonly DynamicObject Other;
        public readonly float TimeOfCollision;

        public CollisionEventArgs(Level level, DynamicObject other, float timeOfCollision)
        {
            Level = level;
            Other = other;
            TimeOfCollision = timeOfCollision;
        }
    }

    public class DeathEventArgs
    {
        public readonly Level Level;

        public DeathEventArgs(Level level)
        {
            Level = level;
        }
    }
}
