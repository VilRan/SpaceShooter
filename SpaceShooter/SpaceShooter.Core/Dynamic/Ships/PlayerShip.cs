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
        const float maxSpeed = 16 * TileSize;
        const float durability = 2000;
        const float collisionDamage = 1000;
        const int score = -100;

        public readonly Player Player;
        public ObservableCollection<InventoryItem> WeaponSlots = new ObservableCollection<InventoryItem>();
        int activeWeaponIndex = 0;
        bool isInvincible = false;

        public override int Score { get { return score; } }
        protected override Rectangle PlayArea { get { return Level.PlayArea; } }
        protected override Color Color { get { return isInvincible ? Color.Yellow : base.Color; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        Weapon activeWeapon { get { return WeaponSlots[activeWeaponIndex].Weapon; } }
        Point ammobarPosition { get { return HealthbarPosition + new Point(0, HealthbarSize.Y * 3 / 2); } }
        Point ammobarSize { get { return new Point(HealthbarSize.X, HealthbarSize.Y); } }

        public PlayerShip(AssetManager assets, Player player)
            : base(assets.PlayerShipTexture, null, Vector2.Zero, durability, Faction.Player)
        {
            Player = player;
            WeaponSlots.Add(new InventoryItem(new Machinegun(), 50));
        }

        public override void Draw(DrawEventArgs e)
        {
            base.Draw(e);
            DrawAmmobar(e);
        }

        private void DrawAmmobar(DrawEventArgs e)
        {
            Point coloredSize = new Point(ammobarSize.X * activeWeapon.MagazineCount / activeWeapon.MagazineSize, ammobarSize.Y);
            Rectangle destination = new Rectangle(ammobarPosition, ammobarSize);
            Rectangle coloredDestination = new Rectangle(ammobarPosition, coloredSize);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, destination, Color.DarkGray);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, coloredDestination, Color.Yellow);
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            Controller controller = Player.Controller;
            KeyboardState keyboard = Keyboard.GetState();

            TryMove(e);
            TryFire(e);
            TrySwitchWeapon();

            if (keyboard.IsKeyDown(Keys.I))
                isInvincible = true;
            else if (keyboard.IsKeyDown(Keys.U))
                isInvincible = false;
        }

        private void TryMove(UpdateEventArgs e)
        {
            Controller controller = Player.Controller;

            Velocity = Camera.Velocity;
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

        private void TryFire(UpdateEventArgs e)
        {
            activeWeapon.Update(e.GameTime);
            if (Player.Controller.IsControlDown(Action.Fire))
                activeWeapon.TryFire(new FireEventArgs(Level, Position, new Vector2(1, 0), this));
        }

        private void TrySwitchWeapon()
        {
            Controller controller = Player.Controller;

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

        public override void Damage(DamageEventArgs e)
        {
            if (!isInvincible)
                base.Damage(e);
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
