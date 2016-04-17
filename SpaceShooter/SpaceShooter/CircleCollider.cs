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
        Vector2 position;
        Vector2 velocity;
        float radius;

        public CircleCollider(Vector2 position, Vector2 velocity, float radius)
        {
            this.position = position;
            this.velocity = velocity;
            this.radius = radius;
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
            Vector2 relativeVelocity = velocity - other.velocity;
            float a = Vector2.Dot(relativeVelocity, relativeVelocity);
            if (a == 0)
            {
                timeOfClosestApproach = float.NaN;
                isCollision = false;
                return;
            }

            Vector2 relativePosition = position - other.position;
            float b = 2 * Vector2.Dot(relativePosition, relativeVelocity);
            float c = Vector2.Dot(relativePosition, relativePosition) - (radius + other.radius) * (radius + other.radius);
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
    }
}
