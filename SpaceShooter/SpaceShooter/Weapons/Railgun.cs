using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    class Railgun : Weapon
    {
        const float bulletSpeed = 4096;
        const int magazineSize = 1;
        
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }

        public Railgun()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            RailgunProjectile projectile = new RailgunProjectile(e.Level.Game.Assets, e.Position, new Vector2(bulletSpeed, 0));
            e.Level.Objects.Add(projectile);
        }
    }
}
