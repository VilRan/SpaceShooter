using Microsoft.Xna.Framework;
using SpaceShooter.Dynamic;
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

        public Rectangle Bounds { get { return new Rectangle(0, 0, Width, Height); } }

        public LevelBlueprint(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public LevelBlueprint(int width, int height, Random random)
            : this(width, height)
        {

        }

        public LevelBlueprint(XmlElement xml)
            : this(int.Parse(xml.GetAttribute("Width")), int.Parse(xml.GetAttribute("Height")))
        {
            foreach (XmlElement spawn in xml)
            {
                Spawns.Add(Spawn.Create(spawn));
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

        public XmlDocument ToXml()
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
