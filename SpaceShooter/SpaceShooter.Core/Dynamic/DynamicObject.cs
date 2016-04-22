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
        protected abstract float CollisionDamage { get; }
        protected virtual Rectangle PlayArea { get { return NormalPlayArea; } }

        public double CurrentDurability { get { return durability.Current; } }
        public double MaximumDurability { get { return durability.Maximum; } }
        public bool IsRemoving { get { return isRemoving || IsDying; } }
        public bool IsDying { get { return durability.Current <= 0; } }
        protected override Color Color { get { return Color.White; } }
        protected SpaceShooterGame Game { get { return Level.Game; } }
        protected Random Random { get { return Game.Random; } }
        protected CircleCollider Collider { get { return new CircleCollider(Position, AbsoluteVelocity, HitRadius); } }
        protected Rectangle NormalPlayArea {
            get { return new Rectangle(Level.PlayArea.Left - TileSize, Level.PlayArea.Top - TileSize, Level.PlayArea.Width + TileSize * 2, Level.PlayArea.Height + TileSize * 2); } }
        protected Rectangle ExtendedPlayArea {
            get { return new Rectangle(NormalPlayArea.Left - NormalPlayArea.Width / 2, NormalPlayArea.Top - NormalPlayArea.Height / 2, NormalPlayArea.Width * 2, NormalPlayArea.Height * 2); } }
        protected Rectangle ExtendedVerticalPlayArea {
            get { return new Rectangle(NormalPlayArea.Left, NormalPlayArea.Top - NormalPlayArea.Height / 2, NormalPlayArea.Width, NormalPlayArea.Height * 2); } }

        public DynamicObject(Texture2D texture, Level level, Vector2 position, Vector2 velocity, float durability, Faction faction)
            : base(texture, position, velocity)
        {
            Level = level;
            Faction = faction;
            this.durability = new Durability(durability);
        }

        public virtual void OnCollision(Collision e)
        {
            e.Other.Damage(new DamageEventArgs(e, CollisionDamage));
        }

        public virtual void OnDeath(DeathEventArgs e)
        {

        }

        public virtual void OnUpdate(UpdateEventArgs e)
        {
            Position += AbsoluteVelocity * (float)e.ElapsedSeconds;

            if (!PlayArea.Contains(Position))
                Remove();
        }
        
        public virtual void Update(UpdateEventArgs e, int collisionStartIndex)
        {
            CheckCollisions(e, collisionStartIndex);
            OnUpdate(e);
        }

        public void CheckCollisions(UpdateEventArgs e, int startIndex)
        {
            if (IsDying)
                return;

            List<Collision> collisions = new List<Collision>();
            for (int index = startIndex; index < Level.Objects.Count; index++)
            {
                DynamicObject other = Level.Objects[index];
                if (!CanCollideWith(other))
                    continue;

                float timeOfCollision;
                if (Collider.FindCollision(other.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                    collisions.Add(new Collision(this, other, timeOfCollision));
            }
            foreach (Collision collision in collisions.OrderBy(c => c.TimeOfCollision))
            {
                collision.Execute();
                if (IsDying)
                    break;
            }
        }

        protected virtual bool CanCollideWith(DynamicObject other)
        {
            if (other.IsDying)
                return false;
            if (other.Faction == Faction)
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
