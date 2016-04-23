using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Weapons;
using System.Collections.ObjectModel;
using SpaceShooter.Particles;
using System.Linq;

namespace SpaceShooter.Dynamic.Ships
{
    public class PlayerShip : Ship
    {
        const float maxSpeed = 8 * TileSize;
        const float durability = 2000;
        const float collisionDamage = 1000;
        const int score = -100;

        public ObservableCollection<InventoryItem> WeaponSlots = new ObservableCollection<InventoryItem>();
        int activeWeaponIndex = 0;
        readonly Player player;
        bool isInvincible = false;

        public override int Score { get { return score; } }
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
            : base(assets.PlayerShipTexture, null, Vector2.Zero, durability, Faction.Player)
        {
            this.player = player;
            WeaponSlots.Add(new InventoryItem(new Machinegun(), 50));
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            Controller controller = player.Controller;
            KeyboardState keyboard = Keyboard.GetState();

            TryMove(e);

            activeWeapon.Update(e.GameTime);
            if (controller.IsControlDown(Action.Fire))
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(1, 0), this));

            TrySwitchWeapon();

            if (keyboard.IsKeyDown(Keys.I))
                isInvincible = true;
            else if (keyboard.IsKeyDown(Keys.U))
                isInvincible = false;
        }
        
        private void TryMove(UpdateEventArgs e)
        {
            Controller controller = player.Controller;

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

                if (thrust.X > 0)
                {
                    TimedParticle.Emit(Level, Position, Color.White, 0.1, 0.5, 200, MathHelper.Pi - MathHelper.Pi / 6, MathHelper.Pi + MathHelper.Pi / 6);
                    TimedParticle.Emit(Level, Position, Color.LightBlue, 0.5, 1, 200, MathHelper.Pi - MathHelper.Pi / 6, MathHelper.Pi + MathHelper.Pi / 6);
                    TimedParticle.Emit(Level, Position, Color.Blue, 0.5, 0.8, 200, MathHelper.Pi - MathHelper.Pi / 6, MathHelper.Pi + MathHelper.Pi / 6);
                }
            }

            foreach (Wall wall in Level.Walls)
            {
                float timeOfCollision;
                if (Collider.FindCollisionHorizontally(wall.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                {
                    Position.X += Velocity.X * timeOfCollision;
                    Velocity.X = 0;
                }
                if (Collider.FindCollisionVertically(wall.Collider, (float)e.ElapsedSeconds, out timeOfCollision))
                {
                    Position.Y += Velocity.Y * timeOfCollision;
                    Velocity.Y = 0;
                }
            }

            Position += Velocity * (float)e.ElapsedSeconds;
            Position.X = MathHelper.Clamp(Position.X, PlayArea.Left, PlayArea.Right);
            Position.Y = MathHelper.Clamp(Position.Y, PlayArea.Top, PlayArea.Bottom);
        }

        private void TrySwitchWeapon()
        {
            Controller controller = player.Controller;

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
            for (int index = 0; index < WeaponSlots.Count; index++)
                if (controller.IsControlDown(Action.Weapon1 + index))
                    activeWeaponIndex = index;
        }

        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }

        public override void Damage(DamageEventArgs e)
        {
            if (!isInvincible)
            {
                base.Damage(e);
                UI.SetHealthbar(100 * CurrentDurability / MaximumDurability);
            }
        }
        public override void Repair(float amount)
        {
            base.Repair(amount);
            UI.SetHealthbar(100 * CurrentDurability / MaximumDurability);
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

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            if (Session.Players.Where(p => p.IsAlive).Count() == 0)
            {
                UI.NavigateToHighscoreEntry();
            }
        }
    }
}
