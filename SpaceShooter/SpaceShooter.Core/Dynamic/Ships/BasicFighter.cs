using Microsoft.Xna.Framework;
using SpaceShooter.Weapons;

namespace SpaceShooter.Dynamic.Ships
{
    class BasicFighter : Fighter
    {
        const float maxSpeed = 8 * TileSize;        
        const int score = 50;        
        public override int Score { get { return score; } }
        
        public BasicFighter(Level level, Vector2 position)
            : base(level, position)
        {
            
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            Velocity = new Vector2(0, 0);
            weapon.TryFire(new FireEventArgs(Level, Position, new Vector2(-1, 0), this));

            base.OnUpdate(e);
        }                
    }
}
