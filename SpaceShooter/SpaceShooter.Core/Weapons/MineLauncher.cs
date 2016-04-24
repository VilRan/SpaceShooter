using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class MineLauncher : Weapon
    {
        const float mineSpeed = 1 * TileSize;
        const int magazineSize = 1;

        public override string Name { get { return "MineLauncher"; } }
        public override double ReloadDelay { get { return 7.0; } }
        public override double FirerateDelay { get { return 0.05; } }
        public override float ProjectileSpeed { get { return mineSpeed; } }

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
