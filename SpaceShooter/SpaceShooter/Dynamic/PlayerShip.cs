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

        readonly Player player;
        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public PlayerShip(AssetManager assets, Player player)
            : base(assets.PlayerShipTexture)
        {
            this.player = player;
            Durability.Both = 2000;
            Faction = Faction.Player;
            weapons.Add(new Machinegun());
            weapons.Add(new Shotgun());
            weapons.Add(new RocketLauncher());
            weapons.Add(new MissileLauncher());
            weapons.Add(new FlakCannon());
            weapons.Add(new Railgun());
            weapons.Add(new DualMachinegun());
            activeWeapon = weapons[0];
        }

        public override void Update(UpdateEventArgs e)
        {
            Controller controller = player.Controller;
            KeyboardState keyboard = Keyboard.GetState();

            Velocity = e.Level.Camera.Velocity;
            if (controller.IsControlDown(Control.MoveUp))
                Velocity += new Vector2(0, -maxSpeed);
            if (controller.IsControlDown(Control.MoveDown))
                Velocity += new Vector2(0, maxSpeed);
            if (controller.IsControlDown(Control.MoveLeft))
                Velocity += new Vector2(-maxSpeed, 0);
            if (controller.IsControlDown(Control.MoveRight))
                Velocity += new Vector2(maxSpeed, 0);

            activeWeapon.Update(e.GameTime);
            if (controller.IsControlDown(Control.Fire))
                activeWeapon.TryFire(new FireEventArgs(e.Level, Position, new Vector2(1,0), this));
            if (controller.IsControlPressed(Control.PreviousWeapon))
            {
                int weaponIndex = weapons.IndexOf(activeWeapon) - 1;
                if (weaponIndex < 0)
                    weaponIndex = weapons.Count - 1;
                activeWeapon = weapons[weaponIndex];
            }
            if (controller.IsControlPressed(Control.NextWeapon))
            {
                int weaponIndex = weapons.IndexOf(activeWeapon) + 1;
                if (weaponIndex >= weapons.Count)
                    weaponIndex = 0;
                activeWeapon = weapons[weaponIndex];
            }

            if (keyboard.IsKeyDown(Keys.D1))
                activeWeapon = weapons[0];
            if (keyboard.IsKeyDown(Keys.D2))
                activeWeapon = weapons[1];
            if (keyboard.IsKeyDown(Keys.D3))
                activeWeapon = weapons[2];
            if (keyboard.IsKeyDown(Keys.D4))
                activeWeapon = weapons[3];
            if (keyboard.IsKeyDown(Keys.D5))
                activeWeapon = weapons[4];
            if (keyboard.IsKeyDown(Keys.D6))
                activeWeapon = weapons[5];
            if (keyboard.IsKeyDown(Keys.D7))
                activeWeapon = weapons[6];


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

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Durability.Current -= 1000;
        }
    }
}
