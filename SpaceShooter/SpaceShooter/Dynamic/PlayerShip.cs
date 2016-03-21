using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Weapons;

namespace SpaceShooter.Dynamic
{
    public class PlayerShip : DynamicObject
    {
        const float maxSpeed = 256;

        Weapon activeWeapon;

        public PlayerShip(AssetManager assets)
            : base(assets.PlayerShipTexture, null)
        {
            Durability.Both = 100;
            Faction = Faction.Player;
            activeWeapon = new Machinegun();
        }

        public override void Update(GameTime gameTime)
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
            if (keyboard.IsKeyDown(Keys.Space))
                activeWeapon.TryFire(Level, Position);

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X < 0)
                Position.X = 0;
            if (Position.X > Level.Width)
                Position.X = Level.Width;
            if (Position.Y < 0)
                Position.Y = 0;
            if (Position.Y > Level.Height)
                Position.Y = Level.Height;
        }
    }
}
