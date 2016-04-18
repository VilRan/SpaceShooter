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
using SpaceShooter.Particles;

namespace SpaceShooter.Dynamic.Ships
{
    public class PlayerShip : Ship
    {
        const float maxSpeed = 256;
        const float durability = 2000;
        const float collisionDamage = 1000;

        public ObservableCollection<InventoryItem> WeaponSlots = new ObservableCollection<InventoryItem>();
        int activeWeaponIndex = 0;
        readonly Player player;
        bool isInvincible = false;
        
        protected override Rectangle PlayArea { get { return Level.PlayArea; } }
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
        protected override float CollisionDamage { get { return collisionDamage; } }
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
            Vector2 thrust = Vector2.Zero;
            if (controller.IsControlDown(Action.MoveUp))
                thrust += new Vector2(0, -1);
            if (controller.IsControlDown(Action.MoveDown))
                thrust += new Vector2(0, 1);
            if (controller.IsControlDown(Action.MoveLeft))
                thrust += new Vector2(-1, 0);
            if (controller.IsControlDown(Action.MoveRight))
                thrust += new Vector2(1, 0);
            if (thrust != Vector2.Zero)
            {
                thrust.Normalize();
                thrust *= maxSpeed;
                Velocity += thrust;
            }

            activeWeapon.Update(e.GameTime);
            if (controller.IsControlDown(Action.Fire))
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(1,0), this));
            if (controller.IsControlPressed(Action.PreviousWeapon))
            {
                activeWeaponIndex--;
                if (activeWeaponIndex < 0)
                    activeWeaponIndex = WeaponSlots.Count - 1;
            }
            if (controller.IsControlPressed(Action.NextWeapon))
            {
                activeWeaponIndex++;
                if (activeWeaponIndex >= WeaponSlots.Count)
                    activeWeaponIndex = 0;
            }

            if (controller.IsControlPressed(Action.Weapon1))
                activeWeaponIndex = 0;
            else if (WeaponSlots.Count > 1 && controller.IsControlPressed(Action.Weapon2))
                activeWeaponIndex = 1;
            else if (WeaponSlots.Count > 2 && controller.IsControlPressed(Action.Weapon3))
                activeWeaponIndex = 2;
            else if (WeaponSlots.Count > 3 && controller.IsControlPressed(Action.Weapon4))
                activeWeaponIndex = 3;
            else if (WeaponSlots.Count > 4 && controller.IsControlPressed(Action.Weapon5))
                activeWeaponIndex = 4;
            else if (WeaponSlots.Count > 5 && controller.IsControlPressed(Action.Weapon6))
                activeWeaponIndex = 5;
            else if (WeaponSlots.Count > 6 && controller.IsControlPressed(Action.Weapon7))
                activeWeaponIndex = 6;

            if (keyboard.IsKeyDown(Keys.I))
                isInvincible = true;
            else if (keyboard.IsKeyDown(Keys.U))
                isInvincible = false;
            
            foreach (Wall wall in Level.Walls)
            {
                bool doBreak = false;
                float timeOfCollision;
                if (Collider.FindCollisionHorizontally(wall.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                {
                    Position.X += Velocity.X * timeOfCollision;
                    Velocity.X = 0;
                    doBreak = true;
                }
                if (Collider.FindCollisionVertically(wall.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                {
                    Position.Y += Velocity.Y * timeOfCollision;
                    Velocity.Y = 0;
                    doBreak = true;
                }
                if (doBreak)
                    break;
            }
            Position += Velocity * (float)e.ElapsedSeconds;

            if (Position.X < PlayArea.Left)
                Position.X = PlayArea.Left;
            if (Position.X > PlayArea.Right)
                Position.X = PlayArea.Right;
            if (Position.Y < PlayArea.Top)
                Position.Y = PlayArea.Top;
            if (Position.Y > PlayArea.Bottom)
                Position.Y = PlayArea.Bottom;
        }

        public override void OnCollision(CollisionEventArgs e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
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
