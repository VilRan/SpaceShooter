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
            Camera = new Camera(Vector2.Zero, SpaceShooterGame.InternalResolution.Size.ToVector2(), new Vector2(128, 0));

            if (Session.Players.Count > 1)
            {
                Vector2 playerStartTop = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height / 4);
                Vector2 playerStartBottom = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height - PlayArea.Height / 4);
                Vector2 playerStartStep = (playerStartBottom - playerStartTop) / (Game.Session.Players.Count - 1);
                for (int playerIndex = 0; playerIndex < Session.Players.Count; playerIndex++)
                    AddPlayer(playerIndex, playerStartTop + playerStartStep * playerIndex);
            }
            else
            {
                AddPlayer(0, new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height / 2));
            }
            
            Inactive.AddRange(blueprint.SpawnObjects(this));
            
            for (int x = 512; x < 2000; x += AssetManager.TileSize)
            {
                Walls.Add(new Wall(Game.Assets, new Vector2(x, 128)));
            }
            for (int y = 0; y < 640; y += AssetManager.TileSize)
            {
                Walls.Add(new Wall(Game.Assets, new Vector2(1000, y)));
            }

            var earth = new BackgroundParticle(Game.Assets.EarthTexture, new Vector2(PlayArea.Width / 3, PlayArea.Height * 3 / 4), 0.9f);
            Particles.Add(earth);

            Random random = Game.Random;
            for (int i = 0; i < 1000; i++)
            {
                Particles.Add(new DustParticle(
                    Game.Assets.PixelTexture,
                    new Vector2(random.Next(PlayArea.Width), random.Next(PlayArea.Height)),
                    (float)random.NextDouble(),
                    (float)random.NextDouble()));
            }

            //Particles.Add(new ExplosionParticle(this,Game.Assets.ParticleTexture,
            //   new Vector2( 550 , 550 ),
            //   VectorUtility.CreateRandom(random, 300)));
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
                if (obj.IsRemoving)
                {
                    if (obj.IsDying)
                         obj.OnDeath(new DeathEventArgs());
                    Objects.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp);

            DrawEventArgs drawEventArgs = new DrawEventArgs(this, gameTime, spriteBatch);
            foreach (Particle particle in Particles)
                particle.Draw(drawEventArgs);
            foreach (Wall wall in Walls)
                wall.Draw(drawEventArgs);
            foreach (DynamicObject obj in Objects)
                obj.Draw(drawEventArgs);
            Particles.RemoveAll(particle => particle.IsRemoving);

            spriteBatch.End();
        }

        void AddPlayer(int playerIndex, Vector2 position)
        {
            Player player = Session.Players[playerIndex];
            player.Ship.Level = this;
            player.Ship.Position = position;
            Objects.Add(player.Ship);
        }
    }
}
