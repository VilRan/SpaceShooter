using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public abstract class Spawn
    {
        public readonly Difficulty Difficulty;
        public readonly Vector2 Position;

        public Spawn(Difficulty difficulty, Vector2 position)
        {
            Difficulty = difficulty;
            Position = position;
        }

        public abstract DynamicObject CreateObject(Level level);
    }

    public class AsteroidSpawn : Spawn
    {
        public AsteroidSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {
        }

        public override DynamicObject CreateObject(Level level)
        {
            Asteroid asteroid = new Asteroid(level);
            asteroid.Position = Position;
            return asteroid;
        }
    }

    public class FighterSpawn : Spawn
    {
        public FighterSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            Fighter fighter = new Fighter(level);
            fighter.Position = Position;
            return fighter;
        }
    }

    public class KamikazeSpawn : Spawn
    {
        public KamikazeSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            Kamikaze kamikaze = new Kamikaze(level);
            kamikaze.Position = Position;
            return kamikaze;
        }
    }

    public class MinelayerSpawn : Spawn
    {
        public MinelayerSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            Minelayer minelayer = new Minelayer(level);
            minelayer.Position = Position;
            return minelayer;
        }
    }
}
