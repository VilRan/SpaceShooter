using Microsoft.Xna.Framework;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System.Linq;

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
            DynamicObject target = GetNearestPlayer();
            if (target != null)
            {
                Vector2 shootingDirection = target.Position - Position;
                shootingDirection.Normalize();
                weapon.Update(e.GameTime);
                weapon.TryFire(new FireEventArgs(Level, Position, shootingDirection, this));
            }

            Velocity = new Vector2(64, 0);

            base.OnUpdate(e);
        }
    }
}
