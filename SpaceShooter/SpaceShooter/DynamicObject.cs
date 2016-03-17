using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public enum Faction
    {
        Player,
        Enemy
    }

    public abstract class DynamicObject : GameObject
    {
        private const int HitRadius = 16;
        private const int HitRadiusSquared = HitRadius * HitRadius;

        public Level Level;
        public Durability Durability = new Durability();
        public Faction Faction = Faction.Enemy;

        public bool IsDying { get { return Durability.Current <= 0; } }
        protected override Color Color { get { return Color.White; } }
        protected SpaceShooterGame Game { get { return Level.Game; } }

        public DynamicObject(Texture2D texture, Level level)
            : base(texture)
        {
            Level = level;
        }

        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Position.X < 0 || Position.X > 1024 || Position.Y < 0 || Position.Y > 768)
                Durability.Current = 0;
        }

        public void CheckCollisions(int startIndex)
        {
            for (int i = startIndex; i < Level.Objects.Count; i++)
            {
                DynamicObject other = Level.Objects[i];
                if (other.Faction == Faction)
                    continue;

                if ((other.Position - Position).LengthSquared() < HitRadiusSquared)
                {
                    OnCollision(other);
                    other.OnCollision(this);
                }
            }
        }

        public virtual void OnCollision(DynamicObject other)
        {

        }
    }
}
