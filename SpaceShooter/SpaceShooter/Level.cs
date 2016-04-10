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
        public List<DynamicObject> Objects = new List<DynamicObject>();
        public List<DynamicObject> Inactive = new List<DynamicObject>();
        public List<Wall> Walls = new List<Wall>();
        public List<Particle> Particles = new List<Particle>();
        public Camera Camera { private set; get; }
        
        public Rectangle PlayArea { get { return Camera.Bounds; } }
        public SpaceShooterGame Game { get { return Session.Game; } }

        public Level(Session session, LevelBlueprint blueprint)
        {
            Session = session;
            Camera = new Camera() { Velocity = new Vector2(128, 0), Size = new Vector2(SpaceShooterGame.InternalResolution.Width, SpaceShooterGame.InternalResolution.Height) };

            if (Session.Players.Count > 1)
            {
                Vector2 playerStartTop = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height / 4);
                Vector2 playerStartBottom = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height - PlayArea.Height / 4);
                Vector2 playerStartStep = (playerStartBottom - playerStartTop) / (Game.Session.Players.Count - 1);
                for (int i = 0; i < Session.Players.Count; i++)
                {
                    Player player = Session.Players[i];
                    player.Ship.Level = this;
                    player.Ship.Position = playerStartTop + playerStartStep * i;
                    Objects.Add(player.Ship);
                }
            }
            else
            {
                Player player = Session.Players[0];
                player.Ship.Level = this;
                player.Ship.Position = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height / 2);
                Objects.Add(player.Ship);
            }
            
            Inactive.AddRange(blueprint.SpawnObjects(this));

            for (int y = 0; y < 640; y += 32)
            {
                var wall = new Wall(Game.Assets);
                wall.Position = new Vector2(1024, y);
                Walls.Add(wall);
            }

            Random random = Game.Random;
            for (int i = 0; i < 1000; i++)
            {
                var dust = new DustParticle(Game.Assets.PixelTexture, random);
                dust.Position = new Vector2(random.Next(PlayArea.Width), random.Next(PlayArea.Height));
                Particles.Add(dust);
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            
            for (int i = 0; i < Inactive.Count; i++)
                if (Inactive[i].TryActivate())
                    i--;

            UpdateEventArgs updateEventArgs = new UpdateEventArgs(gameTime);
            for (int i = 0; i < Objects.Count; i++)
            {
                DynamicObject obj = Objects[i];
                obj.CheckCollisions(gameTime, i + 1);
                obj.Update(updateEventArgs);
                if (obj.IsDying)
                {
                    obj.OnDeath(new DeathEventArgs());
                    Objects.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawEventArgs drawEventArgs = new DrawEventArgs(this, gameTime, spriteBatch);
            foreach (Wall wall in Walls)
                wall.Draw(drawEventArgs);
            foreach (Particle particle in Particles)
                particle.Draw(drawEventArgs);
            foreach (DynamicObject obj in Objects)
                obj.Draw(drawEventArgs);
            Particles.RemoveAll(particle => particle.IsRemoving);
        }
    }
}
