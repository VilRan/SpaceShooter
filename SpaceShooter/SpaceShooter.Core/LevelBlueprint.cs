using Microsoft.Xna.Framework;
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
                    case AsteroidSpawn.String:
                        Spawns.Add(new AsteroidSpawn(difficulty, position));
                        break;
                    case AdvancedFighterSpawn.String:
                        Spawns.Add(new AdvancedFighterSpawn(difficulty, position));
                        break;
                    case KamikazeSpawn.String:
                        Spawns.Add(new KamikazeSpawn(difficulty, position));
                        break;
                    case MinelayerSpawn.String:
                        Spawns.Add(new MinelayerSpawn(difficulty, position));
                        break;
                    case AceFighterSpawn.String:
                        Spawns.Add(new AceFighterSpawn(difficulty, position));
                        break;
                    case SineFighterSpawn.String:
                        Spawns.Add(new SineFighterSpawn(difficulty, position));
                        break;
                    case BasicFighterSpawn.String:
                        Spawns.Add(new BasicFighterSpawn(difficulty, position));
                        break;
                }
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

        public async void SaveToFile(string name)
        {
            Task<XmlDocument> xmlTask = Task.Run(() => generateXml());
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
                {
                    XmlDocument xml = await xmlTask;
                    xml.WriteTo(writer);
                }
            }
        }

        XmlDocument generateXml()
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

            return xml;
        }
    }
}
