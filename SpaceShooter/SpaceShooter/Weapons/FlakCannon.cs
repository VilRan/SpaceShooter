using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class FlakCannon : Weapon
    {
        public override int MagazineSize { get { return 50; } }
        public override double ReloadDelay { get { return 2; } }
        public override double FirerateDelay { get { return 0.35; } }

        public override void OnFire(FireEventArgs e)
        {
            FlakCannonShell shell = new FlakCannonShell(e.Level.Game.Assets, e.Position, new Vector2(1024, 0));
            e.Level.Objects.Add(shell);
        }
    }
}
