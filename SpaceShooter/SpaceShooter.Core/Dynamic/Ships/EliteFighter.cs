using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Player nearestPlayer = Level.Session.Players.
                Where(player => !player.Ship.IsDying).
                OrderBy(player => (player.Ship.Position - Position).LengthSquared()).
                FirstOrDefault();            

            if (nearestPlayer != null)
            {
                Vector2 shootingDirection = nearestPlayer.Ship.Position - Position;
                shootingDirection.Normalize();
                weapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));
            }                

            Velocity = new Vector2(0, 0);
            
            base.OnUpdate(e);
        }
    }
}
