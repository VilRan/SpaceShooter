using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class Railgun : Weapon
    {
        const float bulletSpeed = 128 * TileSize;
        const int magazineSize = 1;

        public override string Name { get { return "Railgun"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }
        public override float ProjectileSpeed { get { return bulletSpeed; } }

        public Railgun()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            RailgunProjectile projectile = new RailgunProjectile(e.Level, e.Position, new Vector2(bulletSpeed, 0));
            e.Level.Objects.Add(projectile);
        }
    }
}
