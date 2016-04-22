using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class FlakCannon : Weapon
    {
        const float bulletSpeed = 32 * TileSize;
        const int magazineSize = 50;

        public override string Name { get { return "Flak Cannon"; } }
        public override double ReloadDelay { get { return 2; } }
        public override double FirerateDelay { get { return 0.35; } }

        public FlakCannon()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            FlakCannonShell shell = new FlakCannonShell(e.Level, e.Position, new Vector2(bulletSpeed, 0));
            e.Level.Objects.Add(shell);
        }
    }
}
