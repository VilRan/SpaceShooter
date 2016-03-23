using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    class Machinegun : Weapon
    {
        public override int MagazineSize { get { return 50; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }

        public override void OnFire(FireEventArgs e)
        {
            Bullet bullet = new Bullet(e.Level.Game.Assets, e.Position, new Vector2(1024, 0));
            e.Level.Objects.Add(bullet);
        }
    }
}
