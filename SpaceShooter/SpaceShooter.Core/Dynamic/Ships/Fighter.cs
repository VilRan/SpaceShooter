using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Particles;
using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SpaceShooter.Dynamic.Ships
{
    abstract class Fighter : EnemyShip
    {
        const float durability = 500;
        const double repairKitDropChance = 0.3;
        const double droneControlDropChance = 0.8;
        const float collisionDamage = 100;

        protected Weapon weapon;
        protected override float CollisionDamage { get { return collisionDamage; } }
        public Fighter(Level level, Vector2 position)
            : base(level.Game.Assets.AsteroidTexture, level, position, durability)
        {
            weapon = new Machinegun();
            weapon.MagazineSize = 3;
            weapon.MagazineCount = 3;
        }
        public override void OnUpdate(UpdateEventArgs e)
        {            
            weapon.Update(e.GameTime);
            

            base.OnUpdate(e);
        }
        public override void OnCollision(Collision e)
        {
            base.OnCollision(e);
            TimedParticle.Emit(Level, e.CollisionPosition, Color.White, 0.25, 1.0, 1024, 20, 40);
        }

        public override void OnDeath(DeathEventArgs e)
        {
            base.OnDeath(e);
            SoundEffectInstance sound = Level.Game.Assets.ExplosionSound.CreateInstance();
            sound.Volume = (float)(0.5 + 0.5 * Level.Game.Random.NextDouble());
            sound.Play();
            ExplosionParticle explosion = new ExplosionParticle(Level, Game.Assets.ParticleTexture, Position, Velocity);
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
