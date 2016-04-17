using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Dynamic
{
    class FlakCannonShell : Projectile
    {
        const float durability = 10;
        const float collisionDamage = 1;

        double flyingTime = 0.6;

        protected override Color Color { get { return Color.LightGray; } }
        protected override float CollisionDamage { get { return collisionDamage; } }

        public FlakCannonShell(Level level, Vector2 position, Vector2 velocity)
            : base(level.Game.Assets.BulletTexture, level, durability)
        {
            Position = position;
            Velocity = velocity;
            Faction = Faction.Player;
        }

        public override void Update(UpdateEventArgs e)
        {
            if (flyingTime > 0)
            {
                flyingTime -= e.ElapsedSeconds;
            }
            else
            {
                Die();
            }
            base.Update(e);
        }
        
        public override void OnDeath(DeathEventArgs e)
        {
            Fragment.Emit(Level, Faction, Position, 40, 80);
        }
    }
}
