using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    class MineLauncher : Weapon
    {
        const float mineSpeed = 32;
        const int magazineSize = 1;

        public override string Name { get { return "MineLauncher"; } }
        public override double ReloadDelay { get { return 7.0; } }
        public override double FirerateDelay { get { return 0.05; } }

        public MineLauncher()
            : base(magazineSize)
        {

        }

        public override void OnFire(FireEventArgs e)
        {
            Mine mine = new Mine(e.Level, e.Position, new Vector2(-mineSpeed, 0), e.Shooter.Faction);
            e.Level.Objects.Add(mine);
        }
    }
}
