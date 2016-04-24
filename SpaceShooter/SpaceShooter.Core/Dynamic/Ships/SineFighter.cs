using Microsoft.Xna.Framework;
using SpaceShooter.Weapons;
using System;

namespace SpaceShooter.Dynamic.Ships
{
    class SineFighter : Fighter
    {
        const float maxSpeed = 16 * TileSize;                
        const int score = 150;        
        double phase;
                
        public override int Score { get { return score; } }
        
        protected override Rectangle PlayArea { get { return ExtendedVerticalPlayArea; } }

        public SineFighter(Level level, Vector2 position)
            : base(level, position)
        {
            
        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            phase += Math.PI / 2 * e.ElapsedSeconds;
            float sine = (float)Math.Sin(phase);

            Velocity = new Vector2(0, sine * maxSpeed);

            weapon.TryFire(new FireEventArgs(Level, Position, new Vector2(-1, 0), this));

            base.OnUpdate(e);
        }
    }
}
