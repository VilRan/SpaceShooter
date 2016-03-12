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
        public abstract void OnFire(Level level, Vector2 position);

        public void TryFire(Level level, Vector2 position)
        {
            if (CanFire)
                OnFire(level, position);
        }
    }
}
