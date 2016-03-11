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
        public List<GameObject> Objects = new List<GameObject>();

        public readonly SpaceShooterGame Game;

        Stack<GameObject> spawnStack = new Stack<GameObject>();


        public Level(SpaceShooterGame game)
        {
            Game = game;
            PlayerShip player = new PlayerShip(game.Assets.PlayerShipTexture);
            Objects.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            if (Game.Random.Next(25) == 0)
            {
                Asteroid asteroid = new Asteroid(Game.Assets.AsteroidTexture, Game.Random);
                Objects.Add(asteroid);
            }
            
            foreach (GameObject obj in Objects)
                obj.Update(gameTime, this);
            foreach (GameObject obj in Objects)
                obj.CheckCollisions(this);

            Objects.AddRange(spawnStack);
            Objects.RemoveAll(obj => obj.IsDying);
            spawnStack.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in Objects)
                obj.Draw(gameTime, spriteBatch);
        }

        public void SpawnObject(GameObject obj)
        {
            spawnStack.Push(obj);
        }
    }
}
