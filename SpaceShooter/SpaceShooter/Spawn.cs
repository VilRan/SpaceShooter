using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using SpaceShooter.Dynamic.Ships;

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
        public const string String = "Asteroid";

        public override string ObjectName { get { return String; } }

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
        public const string String = "Fighter";

        public override string ObjectName { get { return String; } }

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
        public const string String = "Kamikaze";

        public override string ObjectName { get { return String; } }

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
        public const string String = "Minelayer";

        public override string ObjectName { get { return String; } }

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

    public class AceFighterSpawn : Spawn
    {
        public const string String = "Acefighter";

        public override string ObjectName { get { return String; } }

        public AceFighterSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            AceFighter acefighter = new AceFighter(level);
            acefighter.Position = Position;
            return acefighter;
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }
}
