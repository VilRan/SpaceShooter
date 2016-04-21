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
