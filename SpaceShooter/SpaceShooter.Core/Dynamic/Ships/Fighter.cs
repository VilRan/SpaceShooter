using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SpaceShooter.Dynamic.PowerUps;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System;

namespace SpaceShooter.Dynamic.Ships
{
    abstract class Fighter : EnemyShip
    {
        const float durability = 500;
        const double repairKitDropChance = 0.3;
        const double droneControlDropChance = 0.9;
        const float collisionDamage = 100;

        protected Weapon weapon;
        protected override float CollisionDamage { get { return collisionDamage; } }

        public Fighter(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
            weapon = new Machinegun();
            weapon.MagazineCount = weapon.MagazineSize = 3;
        }

        public override void OnUpdate(UpdateEventArgs e)
        {            
            weapon.Update(e.GameTime);
            
            base.OnUpdate(e);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);

            if (Random.NextDouble() < repairKitDropChance)
            {
                RepairKit repairKit = new RepairKit(Level, Position, Vector2.Zero);
                Level.Objects.Add(repairKit);
            }
            if(Random.NextDouble() < droneControlDropChance)
            {
                DroneControl droneControl = new DroneControl(Level, Position, Vector2.Zero);
                Level.Objects.Add(droneControl);
            }
        }
    }
}
