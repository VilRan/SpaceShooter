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
        List<Weapon> weapons = new List<Weapon>();

        public PlayerShip(AssetManager assets)
            : base(assets.PlayerShipTexture)
        {
            Durability.Both = 100;
            Faction = Faction.Player;
            weapons.Add(new Machinegun());
            weapons.Add(new Shotgun());
            activeWeapon = weapons[0];
        }

        public override void Update(UpdateEventArgs e)
        {
            KeyboardState keyboard = Keyboard.GetState();

            Velocity = e.Level.Camera.Velocity;
            if (keyboard.IsKeyDown(Keys.Up))
                Velocity += new Vector2(0, -maxSpeed);
            if (keyboard.IsKeyDown(Keys.Down))
                Velocity += new Vector2(0, maxSpeed);
            if (keyboard.IsKeyDown(Keys.Left))
                Velocity += new Vector2(-maxSpeed, 0);
            if (keyboard.IsKeyDown(Keys.Right))
                Velocity += new Vector2(maxSpeed, 0);

            activeWeapon.Update(e.GameTime);
            if (keyboard.IsKeyDown(Keys.Space))
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position));
            if (keyboard.IsKeyDown(Keys.D1))
                activeWeapon = weapons[0];
            if (keyboard.IsKeyDown(Keys.D2))
                activeWeapon = weapons[1];


            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X < e.Level.PlayArea.Left)
                Position.X = e.Level.PlayArea.Left;
            if (Position.X > e.Level.PlayArea.Right)
                Position.X = e.Level.PlayArea.Right;
            if (Position.Y < e.Level.PlayArea.Top)
                Position.Y = e.Level.PlayArea.Top;
            if (Position.Y > e.Level.PlayArea.Bottom)
                Position.Y = e.Level.PlayArea.Bottom;
        }
    }
}
