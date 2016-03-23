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
        public List<DynamicObject> Inactive = new List<DynamicObject>();
        public List<Particle> Particles = new List<Particle>();
        public Camera Camera { private set; get; }
        
        public Rectangle PlayArea { get { return Camera.Bounds; } }

        public Level(SpaceShooterGame game, LevelBlueprint blueprint)
        {
            Game = game;
            Blueprint = blueprint;
            Inactive.AddRange(Blueprint.GetObjects());
            PlayerShip player = game.Session.Player.Ship;
            player.Position = new Vector2(64, 384);
            Objects.Add(player);
            Camera = new Camera() { Velocity = new Vector2(128, 0), Size = new Vector2(1024, 768) };
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            
            for (int i = 0; i < Inactive.Count; i++)
            {
                DynamicObject obj = Inactive[i];
                obj.TryActive(this);
                if (obj.IsActive)
                {
                    Objects.Add(obj);
                    Inactive.Remove(obj);
                    i--;
                }
            }

            for (int i = 0; i < Objects.Count; i++)
            {
                DynamicObject obj = Objects[i];
                obj.CheckCollisions(gameTime, this, i + 1);
                obj.Update(new UpdateEventArgs(this, gameTime));
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
                obj.Draw(new DrawEventArgs(this, gameTime, spriteBatch));
            foreach (Particle particle in Particles)
                particle.Draw(new DrawEventArgs(this, gameTime, spriteBatch));
            Particles.RemoveAll(particle => particle.Lifespan <= 0.0);
        }
    }
}
