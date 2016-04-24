using System;
using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class Shotgun : Weapon
    {
        const float shotSpeed = 28 * TileSize;
        const float shotSpread = (float)(Math.PI / 16);
        const int shotNumber = 20;
        const int magazineSize = 4;

        public override string Name { get { return "Shotgun"; } }
        public override double ReloadDelay { get { return 1.25; } }
        public override double FirerateDelay { get { return 0.5; } }

        public Shotgun()
            :base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            for (int i = 0; i < shotNumber; i++)
            {
                Vector2 velocity = new Vector2(shotSpeed, 0);
                Matrix rotation = Matrix.CreateRotationZ((float)(e.Random.NextDouble() * shotSpread - e.Random.NextDouble() * shotSpread));
                velocity = Vector2.TransformNormal(velocity, rotation);
                Bullet bullet = new Bullet(e.Level, e.Position, velocity,e.Shooter.Faction);
                e.Level.Objects.Add(bullet);
            }
        }
    }
}
