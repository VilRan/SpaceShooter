using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Dynamic.Projectiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Weapons
{
    class LaserCannon : Weapon
    {
        const float laserSpeed = 0 * TileSize;
        const int magazineSize = 100;

        public override string Name { get { return "Laser Cannon"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.00; } }

        public LaserCannon()
            : base(magazineSize)
        {

        }

        public override void OnFire(FireEventArgs e)
        {
            Laser laser = new Laser(e.Level, e.Position, e.Direction * laserSpeed, e.Shooter.Faction);
            e.Level.Objects.Add(laser);
        }
    }
}
