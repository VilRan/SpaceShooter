﻿using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public LevelBlueprint(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public LevelBlueprint(XmlElement xml)
        {
            foreach (XmlElement spawn in xml)
            {
                Difficulty difficulty = (Difficulty)int.Parse(spawn.GetAttribute("Difficulty"));
                
                int x = int.Parse(spawn.GetAttribute("X"));
                int y = int.Parse(spawn.GetAttribute("Y"));
                Vector2 position = new Vector2(x, y);

                string spawnType = spawn.GetAttribute("Type");
                switch (spawnType)
                {
                    case "Asteroid":
                        Spawns.Add(new AsteroidSpawn(difficulty, position));
                        break;
                    case "Fighter":
                        Spawns.Add(new FighterSpawn(difficulty, position));
                        break;
                    case "Kamikaze":
                        Spawns.Add(new KamikazeSpawn(difficulty, position));
                        break;
                    case "Minelayer":
                        Spawns.Add(new MinelayerSpawn(difficulty, position));
                        break;
                }
            }
        }
        /*
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
            /*
            int testFighters = 100;
            for (int i = 0; i < testFighters; i++)
            {
                Vector2 position = new Vector2(1000 + ((float)Width / testFighters) * i, game.Random.Next(0, Height));
                FighterSpawn spawn = new FighterSpawn(Difficulty.Casual, position);
                Spawns.Add(spawn);
            }
            */
            /*
            int testKamikazes = 20;
            for (int i = 0; i < testKamikazes; i++)
            {
                Vector2 position = new Vector2(1000 + ((float)Width / testKamikazes) * i, game.Random.Next(0, Height));
                KamikazeSpawn spawn = new KamikazeSpawn(Difficulty.Casual, position);
                Spawns.Add(spawn);
            }
            *//*
            int testMinelayers = 20;
            for (int i = 0; i < testMinelayers; i++)
            {
                Vector2 position = new Vector2(1000 + ((float)Width / testMinelayers) * i, game.Random.Next(0, Height));
                MinelayerSpawn spawn = new MinelayerSpawn(Difficulty.Casual, position);
                Spawns.Add(spawn);
            }
        }
        */
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

        public async void SaveToFile(string name)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement level = xml.CreateElement("Level");
            XmlAttribute width = xml.CreateAttribute("Width");
            width.Value = "" + Width;
            XmlAttribute height = xml.CreateAttribute("Height");
            width.Value = "" + Height;
            level.Attributes.Append(width);
            level.Attributes.Append(height);
            xml.AppendChild(level);
            foreach (Spawn spawn in Spawns)
            {
                XmlElement spawnXml = xml.CreateElement("Spawn");
                XmlAttribute difficulty = xml.CreateAttribute("Difficulty");
                difficulty.Value = "" + (int)spawn.Difficulty;
                XmlAttribute x = xml.CreateAttribute("X");
                x.Value = "" + spawn.Position.X;
                XmlAttribute y = xml.CreateAttribute("Y");
                y.Value = "" + spawn.Position.Y;
                XmlAttribute type = xml.CreateAttribute("Type");
                type.Value = spawn.ObjectName;
                spawnXml.Attributes.Append(difficulty);
                spawnXml.Attributes.Append(x);
                spawnXml.Attributes.Append(y);
                spawnXml.Attributes.Append(type);
                level.AppendChild(spawnXml);
            }

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync(name + ".xml", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                Stream stream = fileStream.AsStreamForWrite();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.Indent = true;
                settings.IndentChars = "\t";
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                    xml.WriteTo(writer);
            }
        }
    }
}
