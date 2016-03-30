using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public static class VectorUtility
    {
        public static Vector2? FindInterceptPoint(Vector2 shooterPosition, Vector2 shooterVelocity, Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2 relativePosition = targetPosition - shooterPosition;
            Vector2 relativeVelocity = targetVelocity - shooterVelocity;
            float a = projectileSpeed * projectileSpeed - relativeVelocity.LengthSquared();
            float b = -2 * Vector2.Dot(relativeVelocity, relativePosition);
            float c = -relativePosition.LengthSquared();
            float d = b * b - 4 * a * c;
            if (a != 0 && d > 0)
            {
                float result = (b + (float)Math.Sqrt(d)) / (2 * a);
                return targetPosition + result * relativeVelocity;
            }
            return null;
        }
    }
}
