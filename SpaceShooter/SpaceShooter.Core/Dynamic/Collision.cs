using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    public class Collision
    {
        public readonly DynamicObject Collider;
        public readonly DynamicObject Other;
        public readonly float TimeOfCollision;

        public Vector2 ColliderCollisionPosition { get { return Collider.Position + Collider.Velocity * TimeOfCollision; } }
        public Vector2 OtherCollisionPosition { get { return Other.Position + Other.Velocity * TimeOfCollision; } }
        public Vector2 CollisionPosition { get { return (ColliderCollisionPosition - OtherCollisionPosition) * (Other.HitRadius / (Collider.HitRadius + Other.HitRadius)) + OtherCollisionPosition; } }

        public Collision(DynamicObject collider, DynamicObject other, float timeOfCollision)
        {
            Collider = collider;
            Other = other;
            TimeOfCollision = timeOfCollision;
        }

        public Collision CreateReverse()
        {
            return new Collision(Other, Collider, TimeOfCollision);
        }

        public void Execute()
        {
            Collider.OnCollision(this);
            Other.OnCollision(CreateReverse());
        }
    }
}
