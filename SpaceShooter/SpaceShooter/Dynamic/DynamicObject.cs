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
        const float hitRadius = 12;

        public Level Level;
        public Faction Faction = Faction.Enemy;
        Durability durability;
        bool isRemoving = false;

        public virtual Vector2 RelativeVelocity { get { return Velocity - Level.Camera.Velocity; } set { Velocity = value + Level.Camera.Velocity; } }
        public virtual Vector2 AbsoluteVelocity { get { return Velocity; } }
        public virtual float HitRadius { get { return hitRadius; } }
        public double CurrentDurability { get { return durability.Current; } }
        public double MaximumDurability { get { return durability.Maximum; } }
        public bool IsRemoving { get { return isRemoving || IsDying; } }
        public bool IsDying { get { return durability.Current <= 0; } }
        protected abstract float CollisionDamage { get; }
        protected virtual Rectangle PlayArea { get { return new Rectangle(Level.PlayArea.Left - 32, Level.PlayArea.Top - 32, Level.PlayArea.Width + 64, Level.PlayArea.Height + 64); } }
        protected override Color Color { get { return Color.White; } }
        protected CircleCollider Collider { get { return new CircleCollider(Position, AbsoluteVelocity, HitRadius); } }

        public DynamicObject(Texture2D texture, Level level, float durability)
            : base(texture)
        {
            Level = level;
            this.durability = new Durability(durability);
        }

        public virtual void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, CollisionDamage));
        }

        public virtual void OnDeath(DeathEventArgs e)
        {

        }

        public virtual void Update(UpdateEventArgs e)
        {
            Position += AbsoluteVelocity * (float)e.ElapsedSeconds;

            if (!PlayArea.Contains(Position))
                Remove();
        }
        
        public void CheckCollisions(GameTime gameTime, int startIndex)
        {
            for (int i = startIndex; i < Level.Objects.Count; i++)
            {
                if (IsDying)
                    return;

                DynamicObject other = Level.Objects[i];
                if (!CanCollideWith(other))
                    continue;

                float timeOfCollision;
                if (Collider.FindCollision(other.Collider, (float)gameTime.ElapsedGameTime.TotalSeconds, out timeOfCollision))
                {
                    OnCollision(new CollisionEventArgs(this, other, timeOfCollision));
                    other.OnCollision(new CollisionEventArgs(other, this, timeOfCollision));
                }
            }
        }

        bool CanCollideWith(DynamicObject other)
        {
            if (other.IsDying)
                return false;
            if (other.Faction == Faction)
                return false;
            if (Category == ObjectCategory.Projectile && other.Category == ObjectCategory.Projectile)
                return false;
            return true;
        }
        /*
        public void FindClosestApproach(DynamicObject other, out float timeOfClosestApproach, out bool isCollision)
        {
            Vector2 relativeVelocity = AbsoluteVelocity - other.AbsoluteVelocity;
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
        */
        public bool TryActivate()
        {
            if (Level.PlayArea.Contains(Position))
            {
                Level.Objects.Add(this);
                Level.Inactive.Remove(this);
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

        public void Remove()
        {
            isRemoving = true;
        }
    }
}
