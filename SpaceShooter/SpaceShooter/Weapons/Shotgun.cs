using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class Shotgun : Weapon
    {
        public override int MagazineSize { get { return 5; } }
        public override double ReloadDelay { get { return 1.25; } }
        public override double FirerateDelay { get { return 0.15; } }

        public override void OnFire(FireEventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 velocity = new Vector2(1024, 0);
                Matrix rotation = Matrix.CreateRotationZ((float)(e.Random.NextDouble() * Math.PI / 16 - e.Random.NextDouble() * Math.PI / 16));
                velocity = Vector2.TransformNormal(velocity, rotation);
                Bullet bullet = new Bullet(e.Level.Game.Assets, e.Position, velocity);
                e.Level.Objects.Add(bullet);
            }
        }
    }
}
