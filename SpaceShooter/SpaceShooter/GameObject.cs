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
        
        public Vector2 Origin { get { return Texture.Bounds.Center.ToVector2(); } }
        protected abstract Color Color { get; }

        public GameObject(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void Draw(DrawEventArgs e)
        {
            Vector2 screenPosition = Position - Origin - e.Level.Camera.Position;
            e.SpriteBatch.Draw(Texture, screenPosition, Color);
        }
    }

    public class DrawEventArgs
    {
        public readonly Level Level;
        public readonly GameTime GameTime;
        public readonly SpriteBatch SpriteBatch;

        public DrawEventArgs(Level level, GameTime gameTime, SpriteBatch spriteBatch)
        {
            Level = level;
            GameTime = gameTime;
            SpriteBatch = spriteBatch;
        }
    }
}
