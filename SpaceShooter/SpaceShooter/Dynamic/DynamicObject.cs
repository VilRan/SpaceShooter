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

        Durability durability;
        public Faction Faction = Faction.Enemy;

        public double CurrentDurability { get { return durability.Current; } }
        public double MaximumDurability { get { return durability.Maximum; } }
        public bool IsDying { get { return durability.Current <= 0; } }
        public virtual float HitRadius { get { return hitRadius; } }
        protected override Color Color { get { return Color.White; } }

        public DynamicObject(Texture2D texture, float durability)
            : base(texture)
        {
            this.durability = new Durability(durability);
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

        public virtual void Update(UpdateEventArgs e)
        {
            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;
            
            if (!e.Level.PlayArea.Contains(Position))
                Die();
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
            Vector2 relativeVelocity = Velocity - other.Velocity;
            float a = Vector2.Dot(relativeVelocity, relativeVelocity);
            if (a == 0)
            {
                timeOfClosestApproach = float.NaN;
                isCollision = false;
                return;
            }

            Vector2 relativePosition = Position - other.Position;
            float b = 2 * Vector2.Dot(relativePosition, relativeVelocity);
            float c = Vector2.Dot(relativePosition, relativePosition) - (HitRadius + other.HitRadius) * (HitRadius + other.HitRadius);
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

        public virtual void Repair(float amount)
        {
            durability.Current += amount;
        }

        public virtual void Damage(DamageEventArgs e)
        {
            durability.Current -= e.DamageAmount;
        }

        public void Die()
        {
            durability.Current = 0;
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

    public class DamageEventArgs
    {
        public readonly Level Level;
        public readonly float DamageAmount;

        public DamageEventArgs(CollisionEventArgs collision, float damageAmount)
        {
            Level = collision.Level;
            DamageAmount = damageAmount;
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
