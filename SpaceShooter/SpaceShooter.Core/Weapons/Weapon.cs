using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;

namespace SpaceShooter.Weapons
{
    public abstract class Weapon
    {
        public const int TileSize = AssetManager.TileSize;

        public int MagazineCount;
        public int MagazineSize;
        double reloadTimer = 0.0;
        double firerateTimer = 0.0;

        public abstract string Name { get; }
        public abstract double ReloadDelay { get; }
        public abstract double FirerateDelay { get; }
        public abstract float ProjectileSpeed { get; }
        public bool CanFire { get { return firerateTimer <= 0 && MagazineCount > 0; } }

        public Weapon(int magazineSize)
        {
            MagazineSize = magazineSize;
            MagazineCount = MagazineSize;
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
                    MagazineCount = MagazineSize;
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
                MagazineCount--;
                if (MagazineCount <= 0)
                    reloadTimer += ReloadDelay;
                else
                    firerateTimer += FirerateDelay;
            }
        }

        public void TryReload()
        {
            if (MagazineCount < MagazineSize)
            {
                MagazineCount = 0;
                reloadTimer = ReloadDelay;
            }
        }
    }

    public class FireEventArgs
    {
        public readonly Level Level;
        public readonly Vector2 Position;
        public readonly Vector2 Direction;
        public readonly DynamicObject Shooter;

        public Random Random { get { return Level.Game.Random; } }
        public AssetManager Assets { get { return Level.Game.Assets; } }

        public FireEventArgs(Level level, Vector2 position, Vector2 direction, DynamicObject shooter)
        {
            Level = level;
            Position = position;
            Direction = direction;
            Shooter = shooter;
        }
    }
}
