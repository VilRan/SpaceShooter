﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic.Ships;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter.Dynamic
{
    public abstract class DynamicObject : GameObject
    {
        const float hitRadius = 16;

        public Level Level;
        public Faction Faction = Faction.Enemy;
        Durability durability;
        bool isRemoving = false;

        public virtual Vector2 RelativeVelocity { get { return Velocity - Camera.Velocity; } set { Velocity = value + Camera.Velocity; } }
        public virtual Vector2 AbsoluteVelocity { get { return Velocity; } }
        public virtual float HitRadius { get { return hitRadius; } }
        protected abstract float CollisionDamage { get; }
        protected virtual Rectangle PlayArea { get { return NormalPlayArea; } }

        public float Durability { set { durability.Both = value; } }
        public float CurrentDurability { get { return durability.Current; } }
        public float MaximumDurability { get { return durability.Maximum; } set { durability.Maximum = value; } }
        public bool IsRemoving { get { return isRemoving || IsDying; } }
        public bool IsDying { get { return durability.Current <= 0; } }
        public bool IsAlive { get { return !IsDying; } }
        protected override Color Color { get { return Color.White; } }
        protected SpaceShooterGame Game { get { return Level.Game; } }
        protected ISpaceShooterUI UI { get { return SpaceShooterGame.UI; } }
        protected AssetManager Assets { get { return Game.Assets; } }
        protected Session Session { get { return Level.Session; } }
        protected Random Random { get { return Game.Random; } }
        protected Camera Camera { get { return Level.Camera; } }
        protected CircleCollider Collider { get { return new CircleCollider(Position, AbsoluteVelocity, HitRadius); } }
        protected Rectangle NormalPlayArea {
            get { return new Rectangle(Level.PlayArea.Left - (int)HitRadius, Level.PlayArea.Top - (int)HitRadius, Level.PlayArea.Width + (int)HitRadius * 2, Level.PlayArea.Height + (int)HitRadius * 2); } }
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
            OnDeathEffects(e);
        }

        public virtual void OnUpdate(UpdateEventArgs e)
        {
            Position += AbsoluteVelocity * (float)e.ElapsedSeconds;

            if (!PlayArea.Contains(Position))
                Remove();
        }
        
        public virtual void Update(UpdateEventArgs e, int collisionStartIndex)
        {
            DetectCollisions(e, collisionStartIndex);
            OnUpdate(e);
        }

        public virtual void Repair(float amount)
        {
            durability.Current += amount;
        }

        public virtual void Damage(DamageEventArgs e)
        {
            durability.Current -= e.DamageAmount;
            OnDamageEffects(e);
        }

        public override GameObject Clone()
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<Collision> FindCollisions(UpdateEventArgs e, int startIndex)
        {
            for (int index = startIndex; index < Level.Objects.Count; index++)
            {
                DynamicObject other = Level.Objects[index];
                if (!CanCollideWith(other))
                    continue;

                float timeOfCollision;
                if (Collider.FindCollision(other.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                    yield return new Collision(this, other, timeOfCollision);
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

        protected virtual void OnDeathEffects(DeathEventArgs e)
        {

        }

        protected virtual void OnDamageEffects(DamageEventArgs e)
        {

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

        public void Die()
        {
            durability.Current = 0;
        }

        public void Remove()
        {
            isRemoving = true;
        }

        protected DynamicObject GetNearest(Func<DynamicObject, bool> meetsCondition)
        {
            DynamicObject nearestObject = null;
            float nearestDistance = float.MaxValue;
            foreach (DynamicObject obj in Level.Objects)
            {
                if (!meetsCondition(obj))
                    continue;
                float distance = (obj.Position - Position).LengthSquared();
                if (distance < nearestDistance)
                {
                    nearestObject = obj;
                    nearestDistance = distance;
                }
            }
            return nearestObject;
        }

        protected PlayerShip GetNearestPlayer()
        {
            Player nearestPlayer = Level.Session.PlayersAlive.
                OrderBy(player => (player.Ship.Position - Position).LengthSquared()).
                FirstOrDefault();
            if (nearestPlayer != null)
                return nearestPlayer.Ship;
            return null;
        }

        void DetectCollisions(UpdateEventArgs e, int startIndex)
        {
            if (IsDying)
                return;
            IEnumerable<Collision> collisions = FindCollisions(e, startIndex);
            foreach (Collision collision in collisions.OrderBy(c => c.TimeOfCollision))
            {
                collision.Execute();
                if (IsDying)
                    break;
            }
        }
    }

    public enum Faction
    {
        Player,
        Enemy,
        Neutral
    }
}
