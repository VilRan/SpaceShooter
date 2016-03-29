using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class MissileLauncher : Weapon
    {
        public override int MagazineSize { get { return 5; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.5; } }

        public override void OnFire(FireEventArgs e)
        {
            GuidedMissile missile = new GuidedMissile(e.Level.Game.Assets, e.Position, new Vector2(512, 0));
            e.Level.Objects.Add(missile);
        }
    }
}
