using Microsoft.Xna.Framework;
using SpaceShooter.Weapons;
using System.Linq;

namespace SpaceShooter.Dynamic.Ships
{
    class EliteFighter : Fighter
    {
        const int score = 300;
        public override int Score { get { return score; } }
        public EliteFighter(Level level, Vector2 position)
            : base(level, position)
        {

        }

        public override void OnUpdate(UpdateEventArgs e)
        {
            DynamicObject target = GetNearestPlayer();
            if (target != null)
            {
                Vector2 shootingDirection = target.Position - Position;
                shootingDirection.Normalize();
                weapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));
            }
            
            base.OnUpdate(e);
        }
    }
}
