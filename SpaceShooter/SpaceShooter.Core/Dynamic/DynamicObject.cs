using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
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
        protected virtual Rectangle PlayArea { get { return NormalPlayArea; } }
        protected override Color Color { get { return Color.White; } }
        protected CircleCollider Collider { get { return new CircleCollider(Position, AbsoluteVelocity, HitRadius); } }
        protected Rectangle NormalPlayArea { get { return new Rectangle(Level.PlayArea.Left - 32, Level.PlayArea.Top - 32, Level.PlayArea.Width + 64, Level.PlayArea.Height + 64); } }
        protected Rectangle ExtendedPlayArea {
            get { return new Rectangle(NormalPlayArea.Left - NormalPlayArea.Width / 2, NormalPlayArea.Top - NormalPlayArea.Height / 2, NormalPlayArea.Width * 2, NormalPlayArea.Height * 2); } }

        public DynamicObject(Texture2D texture, Level level, Vector2 position, Vector2 velocity, float durability, Faction faction)
            : base(texture, position, velocity)
        {
            Level = level;
            Faction = faction;
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

        protected virtual bool CanCollideWith(DynamicObject other)
        {
            if (other.IsDying)
                return false;
            if (other.Faction == Faction)
                return false;
            if (other.Category == ObjectCategory.PowerUp)
                return false;
            return true;
        }

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

    public enum Faction
    {
        Player,
        Enemy,
        Neutral
    }
}
