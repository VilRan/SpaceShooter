using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Weapons
{
    abstract class Weapon
    {
        double reloadTimer = 0.0;
        double firerateTimer = 0.0;
        int magazineCount;

        public abstract int MagazineSize { get; }
        public abstract double ReloadDelay { get; }
        public abstract double FirerateDelay { get; }
        public bool CanFire { get { return firerateTimer <= 0 && magazineCount > 0; } }

        public Weapon()
        {
            magazineCount = MagazineSize;
        }

        public abstract void OnFire(FireEventArgs e);

        public void Update(GameTime gameTime)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (reloadTimer <= 0)
                {
                    reloadTimer = 0;
                    magazineCount = MagazineSize;
                }
            }
            else if (firerateTimer > 0)
            {
                firerateTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (firerateTimer < 0)
                {
                    firerateTimer = 0;
                }
            }
        }
        
        public void TryFire(FireEventArgs e)
        {
            if (CanFire)
            {
                OnFire(e);
                magazineCount--;
                if (magazineCount <= 0)
                    reloadTimer += ReloadDelay;
                else
                    firerateTimer += FirerateDelay;
            }
        }
    }

    public class FireEventArgs
    {
        public readonly Level Level;
        public readonly Vector2 Position;
        public readonly Random Random;

        public FireEventArgs(Level level, Vector2 position, Random random)
        {
            Level = level;
            Position = position;
            Random = random;
        }
    }
}
