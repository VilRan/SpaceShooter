using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Dynamic;
using SpaceShooter.Dynamic.Ships;
using System;
using System.Xml;

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

        public static Spawn Create(XmlElement spawn)
        {
            Difficulty difficulty = (Difficulty)int.Parse(spawn.GetAttribute("Difficulty"));

            int x = int.Parse(spawn.GetAttribute("X"));
            int y = int.Parse(spawn.GetAttribute("Y"));
            Vector2 position = new Vector2(x, y);

            string type = spawn.GetAttribute("Type");
            switch (type)
            {
                case AsteroidSpawn.String:
                    return new AsteroidSpawn(difficulty, position);
                case AdvancedFighterSpawn.String:
                    return new AdvancedFighterSpawn(difficulty, position);
                case KamikazeSpawn.String:
                    return new KamikazeSpawn(difficulty, position);
                case MinelayerSpawn.String:
                    return new MinelayerSpawn(difficulty, position);
                case AceFighterSpawn.String:
                    return new AceFighterSpawn(difficulty, position);
                case SineFighterSpawn.String:
                    return new SineFighterSpawn(difficulty, position);
                case BasicFighterSpawn.String:
                    return new BasicFighterSpawn(difficulty, position);
                case EliteFighterSpawn.String:
                    return new EliteFighterSpawn(difficulty, position);
                case BasicBomberSpawn.String:
                    return new BasicBomberSpawn(difficulty, position);
                case RepairDroneSpawn.String:
                    return new RepairDroneSpawn(difficulty, position);
                default:
                    throw new Exception("Unknown spawn type!");
            }
        }
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

    public class EliteFighterSpawn : Spawn
    {
        public const string String = "EliteFighter";

        public override string ObjectName { get { return String; } }

        public EliteFighterSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            return new EliteFighter(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }

    public class BasicBomberSpawn : Spawn
    {
        public const string String = "BasicBomber";

        public override string ObjectName { get { return String; } }

        public BasicBomberSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            return new BasicBomber(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }

    public class RepairDroneSpawn : Spawn
    {
        public const string String = "RepairDrone";

        public override string ObjectName { get { return String; } }

        public RepairDroneSpawn(Difficulty difficulty, Vector2 position)
            : base(difficulty, position)
        {

        }

        public override DynamicObject CreateObject(Level level)
        {
            return new RepairDrone(level, Position);
        }

        public override Texture2D GetTexture(AssetManager assets)
        {
            return assets.AsteroidTexture;
        }

    }
}
