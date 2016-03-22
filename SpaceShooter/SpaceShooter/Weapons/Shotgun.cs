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
        const double firerateDelay = 0.15;
        const double reloadDelay = 1.25;
        const int magazineSize = 5;

        double reloadTimer = 0.0;
        double firerateTimer = 0.0;
        int magazineCount = magazineSize;

        public override bool CanFire { get { return firerateTimer <= 0 && magazineCount > 0; } }

        public override void Update(GameTime gameTime)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (reloadTimer <= 0)
                {
                    reloadTimer = 0;
                    magazineCount = magazineSize;
                }
            }
            else if (firerateTimer > 0)
            {
                firerateTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (firerateTimer < 0)
                {
                    firerateTimer = 0;
                }
            }
        }

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
            
            magazineCount--;

            if (magazineCount <= 0)
                reloadTimer += reloadDelay;
            else
                firerateTimer += firerateDelay;
        }
    }
}
