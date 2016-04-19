using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class DrawEventArgs
    {
        public readonly Level Level;
        public readonly GameTime GameTime;
        public readonly SpriteBatch SpriteBatch;

        public DrawEventArgs(Level level, GameTime gameTime, SpriteBatch spriteBatch)
        {
            Level = level;
            GameTime = gameTime;
            SpriteBatch = spriteBatch;
        }
    }
    
    public class UpdateEventArgs
    {
        public readonly GameTime GameTime;

        public double ElapsedSeconds { get { return GameTime.ElapsedGameTime.TotalSeconds; } }

        public UpdateEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }

    public class CollisionEventArgs
    {
        public readonly DynamicObject Collider;
        public readonly DynamicObject Other;
        public readonly float TimeOfCollision;

        public Vector2 ColliderCollisionPosition { get { return Collider.Position + Collider.Velocity * TimeOfCollision; } }
        public Vector2 OtherCollisionPosition { get { return Other.Position + Other.Velocity * TimeOfCollision; } }
        public Vector2 CollisionPosition { get { return (ColliderCollisionPosition - OtherCollisionPosition) * (Other.HitRadius / (Collider.HitRadius + Other.HitRadius)) + OtherCollisionPosition; } }

        public CollisionEventArgs(DynamicObject collider, DynamicObject other, float timeOfCollision)
        {
            Collider = collider;
            Other = other;
            TimeOfCollision = timeOfCollision;
        }
    }

    public class DamageEventArgs
    {
        public readonly CollisionEventArgs Collision;
        public readonly float DamageAmount;

        public DamageEventArgs(CollisionEventArgs collision, float damageAmount)
        {
            Collision = collision;
            DamageAmount = damageAmount;
        }
    }

    public class DeathEventArgs
    {
        public DeathEventArgs()
        {

        }
    }
}
