using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Dynamic;
using SpaceShooter.Dynamic.Ships;

namespace SpaceShooter.Weapons
{
    class Machinegun : Weapon
    {
        const float bulletSpeed = 32 * TileSize;
        const int magazineSize = 50;

        public override string Name { get { return "Machinegun"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }
        public override float ProjectileSpeed { get { return bulletSpeed; } }

        public Machinegun()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            Bullet bullet = new Bullet(e.Level, e.Position, e.Direction * bulletSpeed, e.Shooter.Faction);
            e.Level.Objects.Add(bullet);
            
            if (e.Shooter is PlayerShip)
            {
                SoundEffectInstance sound = e.Assets.ShotSound.CreateInstance();
                sound.Volume = 0.25f * (float)(0.5 + 0.5 * e.Random.NextDouble());
                sound.Play();
            }
        }
    }
}
