using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Dynamic;
using SpaceShooter.Dynamic.Ships;

namespace SpaceShooter.Weapons
{
    class DualMachinegun : Weapon
    {
        const float bulletSpeed = 32 * TileSize;
        const float bulletOffset = 12;
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

            if (e.Shooter is PlayerShip)
            {
                SoundEffectInstance sound = e.Assets.ShotSound.CreateInstance();
                sound.Volume = 0.25f * (float)(0.5 + 0.5 * e.Random.NextDouble());
                sound.Play();
            }
        }
    }
}
