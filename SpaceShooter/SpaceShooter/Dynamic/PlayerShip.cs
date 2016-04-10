using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Weapons;
using Windows.UI.Xaml;

namespace SpaceShooter.Dynamic
{
    public class PlayerShip : DynamicObject
    {
        const float maxSpeed = 256;
        const float durability = 2000;
        const float collisionDamage = 1000;

        readonly Player player;
        Weapon activeWeapon;
        List<Weapon> weapons = new List<Weapon>();

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }

        public PlayerShip(AssetManager assets, Player player)
            : base(assets.PlayerShipTexture, null, durability)
        {
            this.player = player;
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

            Velocity = Level.Camera.Velocity;
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
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(1,0), this));
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

            if (controller.IsControlPressed(Control.Weapon1))
                activeWeapon = weapons[0];
            else if (controller.IsControlPressed(Control.Weapon2))
                activeWeapon = weapons[1];
            else if (controller.IsControlPressed(Control.Weapon3))
                activeWeapon = weapons[2];
            else if (controller.IsControlPressed(Control.Weapon4))
                activeWeapon = weapons[3];
            else if (controller.IsControlPressed(Control.Weapon5))
                activeWeapon = weapons[4];
            else if (controller.IsControlPressed(Control.Weapon6))
                activeWeapon = weapons[5];
            else if (controller.IsControlPressed(Control.Weapon7))
                activeWeapon = weapons[6];


            Position += Velocity * (float)e.GameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X < Level.PlayArea.Left)
                Position.X = Level.PlayArea.Left;
            if (Position.X > Level.PlayArea.Right)
                Position.X = Level.PlayArea.Right;
            if (Position.Y < Level.PlayArea.Top)
                Position.Y = Level.PlayArea.Top;
            if (Position.Y > Level.PlayArea.Bottom)
                Position.Y = Level.PlayArea.Bottom;

            foreach (Wall wall in Level.Walls)
            {
                if ((wall.Position - Position).LengthSquared() < HitRadius)
                {
                    Die();
                }
            }
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            e.Other.Damage(new DamageEventArgs(e, collisionDamage));
        }

        public override void Damage(DamageEventArgs e)
        {
            base.Damage(e);
            (Application.Current as App).GamePage.HealthbarValue = 100 * CurrentDurability / MaximumDurability;
        }
    }
}
