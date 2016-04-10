using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    class Machinegun : Weapon
    {
        const float bulletSpeed = 1024;
        const int magazineSize = 50;

        public override string Name { get { return "Machinegun"; } }
        public override double ReloadDelay { get { return 1.0; } }
        public override double FirerateDelay { get { return 0.05; } }

        public Machinegun()
            : base(magazineSize)
        {
            
        }

        public override void OnFire(FireEventArgs e)
        {
            Bullet bullet = new Bullet(e.Level, e.Position, e.Direction * bulletSpeed, e.Shooter.Faction);
            e.Level.Objects.Add(bullet);
            
            SoundEffectInstance sound = e.Assets.ShotSound.CreateInstance();
            sound.Volume = (float)(0.5 + 0.5 * e.Random.NextDouble());
            sound.Play();
        }
    }
}
