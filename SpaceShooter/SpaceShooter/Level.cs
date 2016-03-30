using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using SpaceShooter.Particles;
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
        public List<DynamicObject> Inactive = new List<DynamicObject>();
        public List<Particle> Particles = new List<Particle>();
        public Camera Camera { private set; get; }
        
        public Rectangle PlayArea { get { return Camera.Bounds; } }
        public SpaceShooterGame Game { get { return Session.Game; } }

        public Level(Session session, LevelBlueprint blueprint)
        {
            Session = session;
            Blueprint = blueprint;
            Inactive.AddRange(Blueprint.GetObjects());
            Camera = new Camera() { Velocity = new Vector2(256, 0), Size = new Vector2(1024, 768) };
            PlayerShip player = Game.Session.Player.Ship;
            player.Position = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Top + PlayArea.Height / 2);
            Objects.Add(player);

            Random random = Game.Random;
            for (int i = 0; i < 1000; i++)
            {
                BackgroundParticle dust = new BackgroundParticle(Game.Assets.PixelTexture, (float)random.NextDouble());
                dust.Position = new Vector2(random.Next(PlayArea.Width), random.Next(PlayArea.Height));
                Particles.Add(dust);
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            
            for (int i = 0; i < Inactive.Count; i++)
                if (Inactive[i].TryActivate(this))
                    i--;

            UpdateEventArgs updateEventArgs = new UpdateEventArgs(this, gameTime);
            for (int i = 0; i < Objects.Count; i++)
            {
                DynamicObject obj = Objects[i];
                obj.CheckCollisions(gameTime, this, i + 1);
                obj.Update(updateEventArgs);
                if (obj.IsDying)
                {
                    obj.OnDeath(new DeathEventArgs(this));
                    Objects.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawEventArgs drawEventArgs = new DrawEventArgs(this, gameTime, spriteBatch);
            foreach (DynamicObject obj in Objects)
                obj.Draw(drawEventArgs);
            foreach (Particle particle in Particles)
                particle.Draw(drawEventArgs);
            Particles.RemoveAll(particle => particle.IsRemoving);
        }
    }
}
