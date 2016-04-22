using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.Dynamic.Ships
{
    public abstract class Ship : DynamicObject
    {
        public abstract int Score { get;}
        public override ObjectCategory Category { get { return ObjectCategory.Ship; } }


        public Ship(Texture2D texture, Level level, Vector2 position, float durability, Faction faction)
            : base(texture, level, position, Vector2.Zero, durability, faction)
        {

        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            Level.Session.Score += Score;
        }
    }
}
