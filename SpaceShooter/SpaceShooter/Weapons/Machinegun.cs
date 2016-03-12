using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    class Machinegun : Weapon
    {
        const double firerateDelay = 0.05;
        const double reloadDelay = 1.0;
        const int magazineSize = 50;

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

        public override void OnFire(Level level, Vector2 position)
        {
            Bullet bullet = new Bullet(level, position);
            level.SpawnObject(bullet);
            magazineCount--;

            if (magazineCount <= 0)
                reloadTimer += reloadDelay;
            else
                firerateTimer += firerateDelay;
        }
    }
}
