using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    /// <summary>
    /// Contains information about collision events.
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// The object initiating the collision.
        /// </summary>
        public readonly DynamicObject Collider;
        /// <summary>
        /// The object on the receiving end of the collision.
        /// </summary>
        public readonly DynamicObject Other;
        /// <summary>
        /// How many seconds into the future the collision will happen.
        /// </summary>
        public readonly float TimeOfCollision;

        /// <summary>
        /// Calculates the position of the initiating object when the collision happens.
        /// </summary>
        public Vector2 ColliderCollisionPosition { get { return Collider.Position + Collider.Velocity * TimeOfCollision; } }
        /// <summary>
        /// Calculates the position of the target object when the collision happens.
        /// </summary>
        public Vector2 OtherCollisionPosition { get { return Other.Position + Other.Velocity * TimeOfCollision; } }
        /// <summary>
        /// The point between the objects where they come into contact.
        /// </summary>
        public Vector2 ContactPosition { get { return (ColliderCollisionPosition - OtherCollisionPosition) * (Other.HitRadius / (Collider.HitRadius + Other.HitRadius)) + OtherCollisionPosition; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider">The initiator of the collision.</param>
        /// <param name="other">The target of the collision.</param>
        /// <param name="timeOfCollision">Seconds into the future.</param>
        public Collision(DynamicObject collider, DynamicObject other, float timeOfCollision)
        {
            Collider = collider;
            Other = other;
            TimeOfCollision = timeOfCollision;
        }

        /// <summary>
        /// Creates a copy of this collision where the roles of the objects as initiator and target are reversed.
        /// </summary>
        /// <returns></returns>
        public Collision CreateReverse()
        {
            return new Collision(Other, Collider, TimeOfCollision);
        }

        /// <summary>
        /// Calls OnCollision for both objects involved in the collision.
        /// </summary>
        public void Execute()
        {
            Collider.OnCollision(this);
            Other.OnCollision(CreateReverse());
        }
    }
}
