using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Weapons;

namespace SpaceShooter
{
    class PlayerShip : GameObject
    {
        const float maxSpeed = 256;

        Weapon activeWeapon;

        public PlayerShip(Texture2D texture)
            : base(texture)
        {
            HP = 100;
            Position = new Vector2(64, 384);
            Faction = Faction.Player;
            activeWeapon = new Machinegun();
        }

        public override void Update(GameTime gameTime, Level level)
        {
            KeyboardState keyboard = Keyboard.GetState();

            Velocity = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.Up))
                Velocity += new Vector2(0, -maxSpeed);
            if (keyboard.IsKeyDown(Keys.Down))
                Velocity += new Vector2(0, maxSpeed);
            if (keyboard.IsKeyDown(Keys.Left))
                Velocity += new Vector2(-maxSpeed, 0);
            if (keyboard.IsKeyDown(Keys.Right))
                Velocity += new Vector2(maxSpeed, 0);

            activeWeapon.Update(gameTime);
            if (keyboard.IsKeyDown(Keys.Space) && activeWeapon.CanFire)
                activeWeapon.Fire(level, Position);

            base.Update(gameTime, level);
        }
    }
}
