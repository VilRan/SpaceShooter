using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class MissileLauncher : Weapon
    {
        const float launchSpeed = 16 * TileSize;
        const int magazineSize = 50;

        public override string Name { get { return "Missile Launcher"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.5; } }
        public override float ProjectileSpeed { get { return launchSpeed; } }

        public MissileLauncher()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            GuidedMissile missile = new GuidedMissile(e.Level, e.Position, new Vector2(launchSpeed, 0));
            e.Level.Objects.Add(missile);
        }
    }
}
