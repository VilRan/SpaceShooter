using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Dynamic.Ships
{
    public abstract class Ship : DynamicObject
    {
        public abstract int Score { get;}
        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }
        protected virtual bool IsHealthbarVisible { get { return CurrentDurability < MaximumDurability; } }

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
            Point size = new Vector2(HitRadius * 4, HitRadius / 4).ToPoint();
            Point coloredSize = new Point((int)(size.X * CurrentDurability / MaximumDurability), size.Y);
            Point position = (Position + new Vector2(-size.X / 2, HitRadius * 2) - Level.Camera.Position).ToPoint();
            Rectangle destination = new Rectangle(position, size);
            Rectangle coloredDestination = new Rectangle(position, coloredSize);
            Color color = Color.Lerp(Color.Red, Color.Green, (float)(CurrentDurability / MaximumDurability));
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, destination, Color.DarkGray);
            e.SpriteBatch.Draw(Game.Assets.PixelTexture, coloredDestination, color);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            Level.Session.Score += Score;
        }
    }
}
