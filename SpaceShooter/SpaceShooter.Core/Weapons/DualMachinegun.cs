using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;

namespace SpaceShooter.Weapons
{
    class DualMachinegun : Weapon
    {
        const float bulletSpeed = 32 * TileSize;
        const float bulletOffset = 20;
        const int magazineSize = 50;
        
        public override string Name { get { return "Dual Machinegun"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }

        public DualMachinegun()
            :base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            Vector2 firingPosition = new Vector2(0, bulletOffset);

            Bullet firstBullet = new Bullet(e.Level, e.Position - firingPosition, new Vector2(bulletSpeed, 0),e.Shooter.Faction);
            e.Level.Objects.Add(firstBullet);

            Bullet secondBullet = new Bullet(e.Level, e.Position + firingPosition, new Vector2(bulletSpeed, 0),e.Shooter.Faction);
            e.Level.Objects.Add(secondBullet);
        }
    }
}
