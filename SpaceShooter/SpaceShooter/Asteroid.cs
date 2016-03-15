using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Asteroid : DynamicObject
    {
        const float speed = 128;

        public Asteroid(Level level)
            : base(level.Game.Assets.AsteroidTexture, level, new Durability(300))
        {
            Random random = Game.Random;
            
            Position = new Vector2(1024, (float)random.NextDouble() * 768);
            Velocity = new Vector2(-speed, -speed / 2 + speed * (float)random.NextDouble());
        }

        public override void OnCollision(DynamicObject other)
        {
            other.Durability.Current -= 100;

            Random random = Game.Random;
            int n = random.Next(20, 40);
            Particle[] particles = new Particle[n];
            for (int i = 0; i < n; i++)
            {
                Particle particle = new Particle(Game.Assets.ParticleTexture);

                particle.Position = other.Position;

                double direction = random.NextDouble() * Math.PI * 2;
                double speed = random.NextDouble() * 1000;
                particle.Velocity = new Vector2((float)(Math.Cos(direction) * speed), (float)(Math.Sin(direction) * speed));

                particle.Lifespan = random.NextDouble();

                particles[i] = particle;
            }
            Level.Particles.AddRange(particles);
        }
    }
}
