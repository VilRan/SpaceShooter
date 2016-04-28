using Microsoft.Xna.Framework;
using System;

namespace SpaceShooter
{
    public static class VectorUtility
    {
        public static Vector2 CreateRandom(Random random, float maxLength, float minLength = 0, float maxAngle = MathHelper.TwoPi, float minAngle = 0)
        {
            double angle = minAngle + (maxAngle - minAngle) * random.NextDouble();
            double length = minLength + (maxLength - minLength) * random.NextDouble();
            return new Vector2((float)(Math.Cos(angle) * length), (float)(Math.Sin(angle) * length));
        }

        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 shooterVelocity, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2 relativeVelocity = targetVelocity - shooterVelocity;
            float a = projectileSpeed * projectileSpeed - relativeVelocity.LengthSquared();
            if (a == 0)
                return null;

            Vector2 relativePosition = targetPosition - shooterPosition;
            float b = -2 * Vector2.Dot(relativeVelocity, relativePosition);
            float c = -relativePosition.LengthSquared();
            float d = b * b - 4 * a * c;
            if (d > 0)
            {
                float result = (b + (float)Math.Sqrt(d)) / (2 * a);
                return targetPosition + result * relativeVelocity;
            }
            return null;
        }

        public static Vector2? FindInterceptDirection(Vector2 shooterPosition, Vector2 shooterVelocity, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2? point = FindInterceptPoint(shooterPosition, shooterVelocity, targetPosition, targetVelocity, projectileSpeed);
            if (point != null)
                return Vector2.Normalize(point.Value - shooterPosition);
            return null;
        }
    }
}
