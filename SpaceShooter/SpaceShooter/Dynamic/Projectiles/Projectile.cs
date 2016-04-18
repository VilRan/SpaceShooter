using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceShooter.Dynamic
{
    public abstract class Projectile : DynamicObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }
        public override Vector2 RelativeVelocity { get { return Velocity; } set { Velocity = value - Level.Camera.Velocity; } }
        public override Vector2 AbsoluteVelocity { get { return Velocity + Level.Camera.Velocity; } }

        public Projectile(Texture2D texture, Level level, float durability)
            : base(texture, level, durability)
        {

        }

        public override void Update(UpdateEventArgs e)
        {
            foreach (Wall wall in Level.Walls)
            {
                float timeOfCollision;
                if (Collider.FindCollision(wall.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                {
                    //Position += Velocity * timeOfCollision;
                    Die();
                    break;
                }
            }

            base.Update(e);
        }
    }
}
