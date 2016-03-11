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
        
        List<GameObject> objects = new List<GameObject>();
        Stack<GameObject> spawnStack = new Stack<GameObject>();

        public IEnumerable<GameObject> Objects { get { return objects; } }

        public Level(SpaceShooterGame game)
        {
            Game = game;
            PlayerShip player = new PlayerShip(game.Assets.PlayerShipTexture);
            objects.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            if (Game.Random.Next(100) <= 10)
            {
                Asteroid asteroid = new Asteroid(Game.Assets.AsteroidTexture, Game.Random);
                objects.Add(asteroid);
            }
            
            foreach (GameObject obj in objects)
                obj.Update(gameTime, this);
            foreach (GameObject obj in objects)
                obj.CheckCollisions(this);

            objects.AddRange(spawnStack);
            objects.RemoveAll(obj => obj.IsDying);
            spawnStack.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in objects)
                obj.Draw(gameTime, spriteBatch);
        }

        public void SpawnObject(GameObject obj)
        {
            spawnStack.Push(obj);
        }
    }
}
