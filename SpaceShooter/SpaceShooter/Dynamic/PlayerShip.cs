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
using System.Collections.ObjectModel;

namespace SpaceShooter.Dynamic
{
    public class PlayerShip : DynamicObject
    {
        const float maxSpeed = 256;
        const float durability = 2000;
        const float collisionDamage = 1000;

        public ObservableCollection<InventoryItem> WeaponSlots = new ObservableCollection<InventoryItem>();
        int activeWeaponIndex = 0;
        readonly Player player;
        bool isInvincible = false;

        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }
        protected override Color Color
        {
            get
            {
                if (!isInvincible)
                    return base.Color;
                else
                    return Color.Yellow;
            }
        }
        Weapon activeWeapon { get { return WeaponSlots[activeWeaponIndex].Weapon; } }

        public PlayerShip(AssetManager assets, Player player)
            : base(assets.PlayerShipTexture, null, durability)
        {
            this.player = player;
            Faction = Faction.Player;
            WeaponSlots.Add(new InventoryItem(new Machinegun(), 50));
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
                activeWeaponIndex--;
                if (activeWeaponIndex < 0)
                    activeWeaponIndex = WeaponSlots.Count - 1;
            }
            if (controller.IsControlPressed(Control.NextWeapon))
            {
                activeWeaponIndex++;
                if (activeWeaponIndex >= WeaponSlots.Count)
                    activeWeaponIndex = 0;
            }

            if (controller.IsControlPressed(Control.Weapon1))
                activeWeaponIndex = 0;
            else if (WeaponSlots.Count > 1 && controller.IsControlPressed(Control.Weapon2))
                activeWeaponIndex = 1;
            else if (WeaponSlots.Count > 2 && controller.IsControlPressed(Control.Weapon3))
                activeWeaponIndex = 2;
            else if (WeaponSlots.Count > 3 && controller.IsControlPressed(Control.Weapon4))
                activeWeaponIndex = 3;
            else if (WeaponSlots.Count > 4 && controller.IsControlPressed(Control.Weapon5))
                activeWeaponIndex = 4;
            else if (WeaponSlots.Count > 5 && controller.IsControlPressed(Control.Weapon6))
                activeWeaponIndex = 5;
            else if (WeaponSlots.Count > 6 && controller.IsControlPressed(Control.Weapon7))
                activeWeaponIndex = 6;

            if (keyboard.IsKeyDown(Keys.I))
                isInvincible = true;
            else if (keyboard.IsKeyDown(Keys.U))
                isInvincible = false;

            Position += Velocity * (float)e.ElapsedSeconds;

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
            if (!isInvincible)
            {
                base.Damage(e);
                (Application.Current as App).GamePage.HealthbarValue = 100 * CurrentDurability / MaximumDurability;
            }
        }

        public bool TryRemoveWeapon(InventoryItem weapon)
        {
            if (WeaponSlots.Count > 1 && WeaponSlots.Contains(weapon))
            {
                WeaponSlots.Remove(weapon);
                if (activeWeaponIndex <= WeaponSlots.Count)
                    activeWeaponIndex = WeaponSlots.Count - 1;
                return true;
            }
            return false;
        }
    }
}
