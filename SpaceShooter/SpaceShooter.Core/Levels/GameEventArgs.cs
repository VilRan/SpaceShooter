using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;

namespace SpaceShooter
{
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
    
    public class UpdateEventArgs
    {
        public readonly GameTime GameTime;

        public double ElapsedSeconds { get { return GameTime.ElapsedGameTime.TotalSeconds; } }

        public UpdateEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }

    public class DamageEventArgs
    {
        public readonly Collision Collision;
        public readonly float DamageAmount;

        public DamageEventArgs(Collision collision, float damageAmount)
        {
            Collision = collision;
            DamageAmount = damageAmount;
        }
    }

    public class DeathEventArgs
    {
        public DeathEventArgs()
        {

        }
    }
}
