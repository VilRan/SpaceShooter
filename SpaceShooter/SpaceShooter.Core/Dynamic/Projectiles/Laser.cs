using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Dynamic.Projectiles
{
    class Laser : Projectile
    {
        const int speedOfLight = 299792458;
        const float collisionDamage = 100;
        const float durability = 10f;

        protected override float CollisionDamage { get { return collisionDamage; } }

        public Laser(Level level, Vector2 position, Vector2 velocity, Faction faction)
            : base(level.Game.Assets.LaserTexture, level, position, velocity, durability, faction)
        {

        }

        public override void Update(UpdateEventArgs e)
        {
            base.Update(e);
            Velocity = new Vector2(speedOfLight, 0);
        }

        public override void Draw(DrawEventArgs e)
        {
            Point start = (Position - Origin - Level.Camera.Position).ToPoint();
            Point size = Texture.Bounds.Size + new Point(speedOfLight, 0);
            Rectangle destination = new Rectangle(start, size);
            Rectangle source = new Rectangle(new Point(0, 0), size);
            e.SpriteBatch.Draw(Texture, destination, source, Color.Red);
        }
    }
}
