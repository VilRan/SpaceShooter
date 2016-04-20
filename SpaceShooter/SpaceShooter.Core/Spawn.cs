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
            return new Asteroid(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }
    }

    public class AdvancedFighterSpawn : Spawn
    {
        public const string String = "AdvancedFighter";

        public override string ObjectName { get { return String; } }

        public AdvancedFighterSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            return new AdvancedFighter(level, Position);
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
            return new Kamikaze(level, Position);
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
            return new Minelayer(level, Position);
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
            return new AceFighter(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }

    public class SineFighterSpawn : Spawn
    {
        public const string String = "SineFighter";

        public override string ObjectName { get { return String; } }

        public SineFighterSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            return new SineFighter(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }

    public class BasicFighterSpawn : Spawn
    {
        public const string String = "BasicFighter";

        public override string ObjectName { get { return String; } }

        public BasicFighterSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            return new BasicFighter(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }
}
