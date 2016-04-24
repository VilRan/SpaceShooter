using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class RocketLauncher : Weapon
    {
        const float launchSpeed = 16 * TileSize;
        const int magazineSize = 3;

        public override string Name { get { return "Rocket Launcher"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.3; } }
        public override float ProjectileSpeed { get { return launchSpeed; } }

        public RocketLauncher()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            Rocket rocket = new Rocket(e.Level, e.Position, e.Direction * launchSpeed, e.Shooter.Faction);
            e.Level.Objects.Add(rocket);
        }
    }
}
