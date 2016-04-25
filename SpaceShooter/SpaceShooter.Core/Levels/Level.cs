using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using SpaceShooter.Dynamic.Ships;
using SpaceShooter.Particles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter
{
    public class Level
    {
        public const int TileSize = AssetManager.TileSize;

        public readonly Session Session;
        public List<DynamicObject> Objects = new List<DynamicObject>();
        public List<DynamicObject> Inactive = new List<DynamicObject>();
        public List<Wall> Walls = new List<Wall>();
        public List<Particle> Particles = new List<Particle>();
        public Camera Camera { private set; get; }
        readonly LevelBlueprint blueprint;
        
        public Rectangle PlayArea { get { return Camera.Bounds; } }
        public SpaceShooterGame Game { get { return Session.Game; } }
        public ISpaceShooterUI UI { get { return SpaceShooterGame.UI; } }

        public Level(Session session, LevelBlueprint blueprint)
        {
            Session = session;
            Camera = new Camera(Vector2.Zero, SpaceShooterGame.InternalResolution.Size.ToVector2(), new Vector2(8 * TileSize, 0));
            this.blueprint = blueprint;

            if (Session.Players.Count > 1)
            {
                Vector2 playerStartTop = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height / 4);
                Vector2 playerStartBottom = new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height - PlayArea.Height / 4);
                Vector2 playerStartStep = (playerStartBottom - playerStartTop) / (Game.Session.Players.Count - 1);
                for (int playerIndex = 0; playerIndex < Session.Players.Count; playerIndex++)
                    AddPlayer(playerIndex, playerStartTop + playerStartStep * playerIndex);
            }
            else if (Session.Players.Count == 1)
            {
                AddPlayer(0, new Vector2(PlayArea.Left + PlayArea.Width / 8, PlayArea.Height / 2));
            }
            
            Inactive.AddRange(blueprint.SpawnObjects(this));
            Particles.AddRange(blueprint.CloneBackground());

            Random random = Game.Random;
            for (int i = 0; i < 1000; i++)
            {
                Particles.Add(new RepeatingBackgroundParticle(
                    Game.Assets.PixelTexture,
                    new Vector2(random.Next(PlayArea.Width), random.Next(PlayArea.Height)),
                    (float)random.NextDouble(),
                    (float)random.NextDouble()));
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime, blueprint.Bounds);
            
            for (int index = 0; index < Inactive.Count; index++)
                if (Inactive[index].TryActivate())
                    index--;

            UpdateEventArgs updateEventArgs = new UpdateEventArgs(gameTime);
            for (int index = 0; index < Objects.Count; index++)
            {
                DynamicObject obj = Objects[index];
                obj.Update(updateEventArgs, index + 1);
                if (obj.IsRemoving)
                {
                    if (obj.IsDying)
                         obj.OnDeath(new DeathEventArgs());
                    Objects.RemoveAt(index);
                    OnObjectRemoved(obj);
                    index--;
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

        void OnObjectRemoved(DynamicObject obj)
        {
            if (obj is EnemyShip)
            {
                int enemyCount = Inactive.Where(o => o is EnemyShip).Count() + Objects.Where(o => o is EnemyShip).Count();
                if (enemyCount == 0)
                {
                    UI.NavigateToShop();
                }
            }
        }
    }
}
