using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    enum Faction
    {
        Player,
        Enemy
    }

    abstract class GameObject
    {
        private const int HitRadius = 16;
        private const int HitRadiusSquared = HitRadius * HitRadius;

        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public float HP;
        public Faction Faction = Faction.Enemy;
        
        public bool IsDying { get { return HP <= 0; } }

        public GameObject(Texture2D texture)
        {
            Texture = texture;
        }

        public virtual void Update(GameTime gameTime, Level level)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Position.X < 0 || Position.X > 1024 || Position.Y < 0 || Position.Y > 768)
                HP = 0;
        }

        public void CheckCollisions(Level level)
        {
            foreach (GameObject other in level.Objects)
            {
                if (other.Faction == Faction)
                    continue;

                if ((other.Position - Position).LengthSquared() < HitRadiusSquared)
                {
                    OnCollision(other);
                }
            }
        }

        public virtual void OnCollision(GameObject other)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position - Texture.Bounds.Center.ToVector2(), Color.White);
        }
    }
}
