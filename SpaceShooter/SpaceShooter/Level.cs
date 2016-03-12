using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Level
    {

        public readonly SpaceShooterGame Game;
        
        List<DynamicObject> objects = new List<DynamicObject>();
        List<Particle> particles = new List<Particle>();
        Stack<DynamicObject> spawnStack = new Stack<DynamicObject>();

        public IEnumerable<DynamicObject> Objects { get { return objects; } }

        public Level(SpaceShooterGame game)
        {
            Game = game;
            PlayerShip player = new PlayerShip(this);
            objects.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            if (Game.Random.Next(100) <= 10)
            {
                Asteroid asteroid = new Asteroid(this);
                objects.Add(asteroid);
            }
            
            foreach (DynamicObject obj in objects)
                obj.Update(gameTime);
            foreach (DynamicObject obj in objects)
                obj.CheckCollisions();

            objects.AddRange(spawnStack);
            objects.RemoveAll(obj => obj.IsDying);
            spawnStack.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (DynamicObject obj in objects)
                obj.Draw(gameTime, spriteBatch);
            foreach (Particle particle in particles)
                particle.Draw(gameTime, spriteBatch);
            particles.RemoveAll(particle => particle.Lifespan <= 0.0);
        }

        public void SpawnObject(DynamicObject obj)
        {
            spawnStack.Push(obj);
        }

        public void SpawnParticles(IEnumerable<Particle> particles)
        {
            this.particles.AddRange(particles);
        }
    }
}
