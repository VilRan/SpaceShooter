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
    public abstract class Spawn
    {
        public readonly Difficulty Difficulty;
        public readonly Vector2 Position;

        public abstract string ObjectName { get; }

        public Spawn(Difficulty difficulty, Vector2 position)
        {
            Difficulty = difficulty;
            Position = position;
        }

        public abstract DynamicObject CreateObject(Level level);
        public abstract Texture2D GetTexture(AssetManager assets);
    }

    public class AsteroidSpawn : Spawn
    {
        public override string ObjectName { get { return "Asteroid"; } }

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

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }
    }

    public class FighterSpawn : Spawn
    {
        public override string ObjectName { get { return "Fighter"; } }

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

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }
    }

    public class KamikazeSpawn : Spawn
    {
        public override string ObjectName { get { return "Kamikaze"; } }

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

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }
    }

    public class MinelayerSpawn : Spawn
    {
        public override string ObjectName { get { return "Minelayer"; } }

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

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }
    }
}
