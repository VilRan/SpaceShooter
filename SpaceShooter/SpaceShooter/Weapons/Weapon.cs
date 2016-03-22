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
        public abstract bool CanFire { get; }

        public abstract void Update(GameTime gameTime);
        public abstract void OnFire(FireEventArgs e);

        public void TryFire(FireEventArgs e)
        {
            if (CanFire)
                OnFire(e);
        }
    }

    public class FireEventArgs
    {
        public readonly Level Level;
        public readonly Vector2 Position;

        public FireEventArgs(Level level, Vector2 position)
        {
            Level = level;
            Position = position;
        }
    }
}
