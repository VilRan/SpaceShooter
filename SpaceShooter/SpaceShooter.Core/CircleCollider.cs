using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public struct CircleCollider
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Radius;

        public CircleCollider(Vector2 position, Vector2 velocity, float radius)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Radius = radius;
        }

        public bool FindCollision(CircleCollider other, float timeLimit, out float timeOfCollision)
        {
            bool isCollision = false;
            FindClosestApproach(other, out timeOfCollision, out isCollision);
            if (timeOfCollision > timeLimit)
                isCollision = false;
            return isCollision;
        }

        public void FindClosestApproach(CircleCollider other, out float timeOfClosestApproach, out bool isCollision)
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
            float c = Vector2.Dot(relativePosition, relativePosition) - (Radius + other.Radius) * (Radius + other.Radius);
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

        public bool FindCollision(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            return FindCollisionHorizontally(other, timeLimit, out timeOfCollision)
                || FindCollisionVertically(other, timeLimit, out timeOfCollision);
        }

        public bool FindCollisionHorizontally(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            return FindCollisionFromLeft(other, timeLimit, out timeOfCollision)
                || FindCollisionFromRight(other, timeLimit, out timeOfCollision);
        }

        public bool FindCollisionFromLeft(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            float deltaX = other.Bounds.Left - Position.X - Radius;
            if (deltaX >= 0 && Velocity.X > 0)
                return findLinearCollision(other, deltaX, Velocity.X, NormalDirection.Vertical, timeLimit, out timeOfCollision);
            timeOfCollision = float.NaN;
            return false;
        }

        public bool FindCollisionFromRight(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            float deltaX = other.Bounds.Right - Position.X + Radius;
            if (deltaX <= 0 && Velocity.X < 0)
                return findLinearCollision(other, deltaX, Velocity.X, NormalDirection.Vertical, timeLimit, out timeOfCollision);
            timeOfCollision = float.NaN;
            return false;
        }

        public bool FindCollisionVertically(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            return FindCollisionFromTop(other, timeLimit, out timeOfCollision)
                || FindCollisionFromBottom(other, timeLimit, out timeOfCollision);
        }

        public bool FindCollisionFromTop(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            float deltaY = other.Bounds.Top - Position.Y - Radius;
            if (deltaY >= 0 && Velocity.Y > 0)
                return findLinearCollision(other, deltaY, Velocity.Y, NormalDirection.Horizontal, timeLimit, out timeOfCollision);
            timeOfCollision = float.NaN;
            return false;
        }

        public bool FindCollisionFromBottom(RectangleCollider other, float timeLimit, out float timeOfCollision)
        {
            float deltaY = other.Bounds.Bottom - Position.Y + Radius;
            if (deltaY <= 0 && Velocity.Y < 0)
                return findLinearCollision(other, deltaY, Velocity.Y, NormalDirection.Horizontal, timeLimit, out timeOfCollision);
            timeOfCollision = float.NaN;
            return false;
        }

        bool findLinearCollision(RectangleCollider other, float distance, float speed, NormalDirection normalDirection, float timeLimit, out float timeOfCollision)
        {
            timeOfCollision = float.NaN;
            if (speed != 0)
            {
                float timeToCollision = distance / speed;
                if (timeToCollision < timeLimit)
                {
                    Vector2 positionAtCollision = Position + Velocity * timeToCollision;
                    switch (normalDirection)
                    {
                        case NormalDirection.Vertical:
                            if (positionAtCollision.Y >= other.Bounds.Top && positionAtCollision.Y <= other.Bounds.Bottom)
                            {
                                timeOfCollision = timeToCollision;
                                return true;
                            }
                            break;
                        case NormalDirection.Horizontal:
                            if (positionAtCollision.X >= other.Bounds.Left && positionAtCollision.X <= other.Bounds.Right)
                            {
                                timeOfCollision = timeToCollision;
                                return true;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        enum NormalDirection
        {
            Horizontal,
            Vertical
        }
    }
}
