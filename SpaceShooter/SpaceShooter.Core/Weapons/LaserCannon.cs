using SpaceShooter.Dynamic.Projectiles;

namespace SpaceShooter.Weapons
{
    class LaserCannon : Weapon
    {
        const float speedOfLight = 299792458;
        const int magazineSize = 100;

        public override string Name { get { return "Laser Cannon"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.00; } }
        public override float ProjectileSpeed { get { return speedOfLight; } }

        public LaserCannon()
            : base(magazineSize)
        {

        }

        public override void OnFire(FireEventArgs e)
        {
            Laser laser = new Laser(e.Level, e.Position, e.Direction * speedOfLight, e.Shooter.Faction);
            e.Level.Objects.Add(laser);
        }
    }
}
