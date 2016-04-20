using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public abstract class GameObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        
        public virtual Vector2 Origin { get { return Texture.Bounds.Center.ToVector2(); } }
        public abstract ObjectCategory Category { get; }
        protected abstract Color Color { get; }


        public GameObject(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
        }

        public virtual void Draw(DrawEventArgs e)
        {
            Vector2 screenPosition = Position - Origin - e.Level.Camera.Position;
            e.SpriteBatch.Draw(Texture, screenPosition, Color);
        }
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
