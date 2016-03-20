using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Level
    {
        public readonly SpaceShooterGame Game;
        public readonly LevelBlueprint Blueprint;
        public List<DynamicObject> Objects = new List<DynamicObject>();
        public List<Particle> Particles = new List<Particle>();

        public Level(SpaceShooterGame game, LevelBlueprint blueprint)
        {
            Game = game;
            Blueprint = blueprint;
            PlayerShip player = game.Session.Player.Ship;
            player.Level = this;
            player.Position = new Vector2(64, 384);
            Objects.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            if (Game.Random.Next(100) <= 10)
            {
                Asteroid asteroid = new Asteroid(this);
                Objects.Add(asteroid);
            }
            
            for (int i = 0; i < Objects.Count; i++)
            {
                DynamicObject obj = Objects[i];
                obj.CheckCollisions(gameTime, i + 1);
                obj.Update(gameTime);
                if (obj.IsDying)
                {
                    Objects.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (DynamicObject obj in Objects)
                obj.Draw(gameTime, spriteBatch);
            foreach (Particle particle in Particles)
                particle.Draw(gameTime, spriteBatch);
            Particles.RemoveAll(particle => particle.Lifespan <= 0.0);
        }

        public void SpawnObject(DynamicObject obj)
        {
            Objects.Add(obj);
        }
    }
}
