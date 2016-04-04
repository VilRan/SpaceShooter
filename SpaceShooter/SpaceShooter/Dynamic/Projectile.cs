using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    public class Projectile : DynamicObject
    {
        public override ObjectCategory Category { get { return ObjectCategory.Projectile; } }

        public Projectile(Texture2D texture, float durability)
            : base(texture, durability)
        {

        }

        public override void Update(UpdateEventArgs e)
        {
            Position += (Velocity + e.Level.Camera.Velocity) * (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (!e.Level.PlayArea.Contains(Position))
                Die();
        }
    }
}
