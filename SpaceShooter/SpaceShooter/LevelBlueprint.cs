using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class LevelBlueprint
    {
        public readonly int Width;
        public readonly int Height;
        public List<Spawn> Spawns = new List<Spawn>();

        public LevelBlueprint(SpaceShooterGame game)
        {
            Width = 10240;
            Height = SpaceShooterGame.InternalResolution.Height;
            /*
            int testEnemies = 1000;
            Objects = new DynamicObject[testEnemies];
            for (int i = 0; i < testEnemies; i++)
            {
                Asteroid test = new Asteroid(null);
                test.Position = new Vector2(SpaceShooterGame.InternalResolution.Width + ((float)Width / testEnemies) * i, game.Random.Next(0, Height));
                test.Velocity = new Vector2(0, -128 / 2 + 128 * (float)game.Random.NextDouble());
                Objects[i] = test;
            }
            */
            int testFighters = 100;
            for (int i = 0; i < testFighters; i++)
            {
                Vector2 position = new Vector2(1000 + ((float)Width / testFighters) * i, game.Random.Next(0, Height));
                FighterSpawn spawn = new FighterSpawn(Difficulty.Casual, position);
                Spawns.Add(spawn);
            }
            
        }

        public IEnumerable<DynamicObject> SpawnObjects(Level level)
        {
            Difficulty difficulty = level.Session.Difficulty;
            foreach (Spawn spawn in Spawns)
            {
                if (spawn.Difficulty <= difficulty)
                {
                    yield return spawn.CreateObject(level);
                }
            }
        }
    }
}
