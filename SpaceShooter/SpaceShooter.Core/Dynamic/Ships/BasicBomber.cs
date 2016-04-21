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
    class BasicBomber : EnemyShip
    {
        const float durability = 500;
        const float collisionDamage = 100;
        const int score = 300;

        Weapon weapon;
        public override int Score { get { return score; } }
        protected override float CollisionDamage { get { return collisionDamage; } }
        public BasicBomber(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
            weapon = new RocketLauncher();
            weapon.MagazineSize = 1;
            weapon.MagazineCount = 1;
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
                weapon.Update(e.GameTime);
                weapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));
            }

            Velocity = new Vector2(64, 0);

            base.OnUpdate(e);
        }

        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }
    }
}
