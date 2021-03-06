﻿using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using SpaceShooter.Particles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SpaceShooter
{
    public class LevelBlueprint
    {
        public int Width { private set; get; }
        public int Height { private set; get; }
        public List<Spawn> Spawns = new List<Spawn>();
        public List<BackgroundParticle> Background = new List<BackgroundParticle>();

        public Rectangle Bounds { get { return new Rectangle(0, 0, Width, Height); } }

        /// <summary>
        /// Generates a new empty blueprint.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public LevelBlueprint(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Generates a new random level.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="random"></param>
        /// <param name="session"></param>
        public LevelBlueprint(int width, int height, Random random, Session session)
            : this(width, height)
        {
            if (width < 5000)
                throw new Exception("A random level must be at least 5000 pixels wide.");

            int challengeRating = session.LevelNumber;
            WeightedList<string> spawnTypes = Spawn.GetWeightedListOfTypes(challengeRating);

            int minX = SpaceShooterGame.InternalResolution.Right;
            int maxX = Width - minX - AssetManager.TileSize * 2;

            int casualEnemies = 20 + 20 * challengeRating;
            casualEnemies *= session.Players.Count;
            for (int i = 0; i < casualEnemies; i++)
                AddSpawn(Difficulty.Casual, random, minX, maxX, spawnTypes);

            int hardcoreEnemies = 20 + 20 * challengeRating;
            hardcoreEnemies *= session.Players.Count;
            for (int i = 0; i < hardcoreEnemies; i++)
                AddSpawn(Difficulty.Hardcore, random, minX, maxX, spawnTypes);

            int nightmareEnemies = 20 + 20 * challengeRating;
            nightmareEnemies *= session.Players.Count;
            for (int i = 0; i < nightmareEnemies; i++)
                AddSpawn(Difficulty.Nightmare, random, minX, maxX, spawnTypes);
        }

        private void AddSpawn(Difficulty difficulty, Random random, int minX, int maxX, WeightedList<string> spawnTypes)
        {
            Vector2 position = new Vector2(random.Next(minX, maxX), random.Next(0, Height));
            Spawn spawn = Spawn.Create(difficulty, position, spawnTypes.SelectRandom(random));
            Spawns.Add(spawn);
        }

        /// <summary>
        /// Generates a blueprint based on a "Level" xml element.
        /// </summary>
        /// <param name="xml"></param>
        public LevelBlueprint(XmlElement xml)
            : this(int.Parse(xml.GetAttribute("Width")), int.Parse(xml.GetAttribute("Height")))
        {
            foreach (XmlElement spawn in xml)
            {
                Spawns.Add(Spawn.Create(spawn));
            }
        }

        /// <summary>
        /// Returns all objects matching the current session's difficulty level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the static background particles, as well as creating and returning dynamically generated particles.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<BackgroundParticle> CreateBackground(Level level)
        {
            SpaceShooterGame game = level.Game;
            Settings settings = game.Settings;
            Random random = game.Random;
            int minParticles = (int)(500 * settings.ParticleDensity);
            int maxParticles = (int)(1000 * settings.ParticleDensity);
            int numberOfParticles = random.Next(minParticles, maxParticles);
            int maxX = level.PlayArea.Width;
            int maxY = level.PlayArea.Height;
            for (int i = 0; i < numberOfParticles; i++)
            {
                yield return new RepeatingBackgroundParticle(
                    game.Assets.PixelTexture,
                    new Vector2(random.Next(maxX), random.Next(maxY)),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            }

            foreach (BackgroundParticle particle in Background)
            {
                yield return (BackgroundParticle)particle.Clone();
            }
        }

        /// <summary>
        /// Stores all relevant data about the blueprint into an XML document.
        /// </summary>
        /// <returns></returns>
        public XmlDocument ToXmlDocument()
        {
            XmlDocument xml = new XmlDocument();
            XmlElement level = xml.CreateElement("Level");
            XmlAttribute width = xml.CreateAttribute("Width");
            width.Value = "" + Width;
            XmlAttribute height = xml.CreateAttribute("Height");
            height.Value = "" + Height;
            level.Attributes.Append(width);
            level.Attributes.Append(height);
            xml.AppendChild(level);
            foreach (Spawn spawn in Spawns)
            {
                XmlElement spawnXml = xml.CreateElement("Spawn");
                XmlAttribute difficulty = xml.CreateAttribute("Difficulty");
                difficulty.Value = "" + (int)spawn.Difficulty;
                XmlAttribute x = xml.CreateAttribute("X");
                x.Value = "" + (int)spawn.Position.X;
                XmlAttribute y = xml.CreateAttribute("Y");
                y.Value = "" + (int)spawn.Position.Y;
                XmlAttribute type = xml.CreateAttribute("Type");
                type.Value = spawn.ObjectName;
                spawnXml.Attributes.Append(difficulty);
                spawnXml.Attributes.Append(x);
                spawnXml.Attributes.Append(y);
                spawnXml.Attributes.Append(type);
                level.AppendChild(spawnXml);
            }
            return xml;
        }
    }
}
