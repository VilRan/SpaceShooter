using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Dynamic.Ships
{
    public abstract class Ship : DynamicObject
    {
        public abstract int Score { get;}
        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }
        protected virtual bool IsHealthbarVisible { get { return CurrentDurability < MaximumDurability; } }
        protected Point HealthbarSize { get { return new Vector2(HitRadius * 4, HitRadius / 4).ToPoint(); } }
        protected Point HealthbarPosition { get { return (Position + new Vector2(-HealthbarSize.X / 2, HitRadius * 2) - Camera.Position).ToPoint(); } }

        public Ship(Texture2D texture, Level level, Vector2 position, float durability, Faction faction)
            : base(texture, level, position, Vector2.Zero, durability, faction)
        {

        }

        public override void Draw(DrawEventArgs e)
        {
            base.Draw(e);
            if (IsHealthbarVisible)
                DrawHealthbar(e);
        }

        void DrawHealthbar(DrawEventArgs e)
        {
            Point coloredSize = new Point((int)(HealthbarSize.X * CurrentDurability / MaximumDurability), HealthbarSize.Y);
            Rectangle destination = new Rectangle(HealthbarPosition, HealthbarSize);
            Rectangle coloredDestination = new Rectangle(HealthbarPosition, coloredSize);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, destination, Color.DarkGray);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, coloredDestination, Color.Red);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            Level.Session.Score += Score;
        }
    }
}
