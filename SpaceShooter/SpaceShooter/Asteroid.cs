using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Asteroid : GameObject
    {
        const float speed = 128;

        public Asteroid(Texture2D texture, Random random)
            : base(texture)
        {
            HP = 300;
            Position = new Vector2(1024, (float)random.NextDouble() * 768);
            Velocity = new Vector2(-speed, -speed / 2 + speed * (float)random.NextDouble());
        }
    }
}
