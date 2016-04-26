using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    public abstract class GameObject
    {
        public const int TileSize = Level.TileSize;

        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        
        public virtual Vector2 Origin { get { return Texture.Bounds.Center.ToVector2(); } }
        public abstract ObjectCategory Category { get; }
        protected abstract Color Color { get; }
        protected virtual float Rotation { get { return 0; } }
        protected virtual float Scale { get { return 1; } }

        public GameObject(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
        }

        public virtual void Draw(DrawEventArgs e)
        {
            Vector2 screenPosition = Position - e.Level.Camera.Position;
            e.SpriteBatch.Draw(Texture, screenPosition, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }

        public abstract GameObject Clone();
    }

    public enum ObjectCategory
    {
        Tile,
        Particle,
        Projectile,
        Ship,
        PowerUp
    }
}
