using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    class DualMachinegun : Weapon
    {
        public override int MagazineSize { get { return 50; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }

        public override void OnFire(FireEventArgs e)
        {
            Vector2 firingPosition = new Vector2(0, 20);

            Bullet firstBullet = new Bullet(e.Level.Game.Assets, e.Position - firingPosition, new Vector2(1024, 0),e.Shooter.Faction);
            e.Level.Objects.Add(firstBullet);

            Bullet secondBullet = new Bullet(e.Level.Game.Assets, e.Position + firingPosition, new Vector2(1024, 0),e.Shooter.Faction);
            e.Level.Objects.Add(secondBullet);
        }
    }
}
