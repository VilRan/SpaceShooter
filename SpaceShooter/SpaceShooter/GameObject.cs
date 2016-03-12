using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    abstract class GameObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;

        public Vector2 ScreenPosition { get { return Position - Origin; } }
        public Vector2 Origin { get { return Texture.Bounds.Center.ToVector2(); } }

        public GameObject(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ScreenPosition, Color.White);
        }
    }
}
