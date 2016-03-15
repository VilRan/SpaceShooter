﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Level
    {
        public readonly Session Session;
        public readonly LevelBlueprint Blueprint;
        public List<DynamicObject> Objects = new List<DynamicObject>();
        public List<Particle> Particles = new List<Particle>();

        Stack<DynamicObject> spawnStack = new Stack<DynamicObject>();

        public SpaceShooterGame Game { get { return Session.Game; } }

        public Level(Session session, LevelBlueprint blueprint)
        {
            Session = session;
            Blueprint = blueprint;
            PlayerShip player = new PlayerShip(this, Session.Player.ShipData);
            Objects.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            if (Game.Random.Next(100) <= 10)
            {
                Asteroid asteroid = new Asteroid(this);
                Objects.Add(asteroid);
            }
            
            foreach (DynamicObject obj in Objects)
                obj.Update(gameTime);
            Objects.AddRange(spawnStack);
            spawnStack.Clear();

            for (int i = 0; i < Objects.Count - 1; i++)
                Objects[i].CheckCollisions(i + 1);
            Objects.RemoveAll(obj => obj.IsDying);
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
            spawnStack.Push(obj);
        }
    }
}
