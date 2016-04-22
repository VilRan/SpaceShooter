﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;

namespace SpaceShooter
{
    public class AssetManager
    {
        public const int TileSize = 32;

        public Texture2D PlayerShipTexture { private set; get; }
        public Texture2D AsteroidTexture { private set; get; }
        public Texture2D BulletTexture { private set; get; }
        public Texture2D LaserTexture { private set; get; }
        public Texture2D ParticleTexture { private set; get; }
        public Texture2D PixelTexture { private set; get; }
        public Texture2D TileTexture { private set; get; }
        public Texture2D EarthTexture { private set; get; }
        public Texture2D GridTexture { private set; get; }

        public Song MainMusic { private set; get; }
        public Song SomethingMusic { private set; get; }
        public Song RelaxMusic { private set; get; }
        public Song BossMusic { private set; get; }

        public SoundEffect ShotSound { private set; get; }
        public SoundEffect ExplosionSound { private set; get; }

        public LevelBlueprint TestLevelBlueprint { private set; get; }

        public AssetManager(ContentManager content)
        {
            Task loadLevelsTask = Task.Run(() => loadLevels());

            PlayerShipTexture = content.Load<Texture2D>("Textures/Ship");
            AsteroidTexture = content.Load<Texture2D>("Textures/Asteroid");
            BulletTexture = content.Load<Texture2D>("Textures/Bullet");
            LaserTexture = content.Load<Texture2D>("Textures/Laser.png");
            ParticleTexture = content.Load<Texture2D>("Textures/Particle");
            PixelTexture = content.Load<Texture2D>("Textures/Pixel");
            TileTexture = content.Load<Texture2D>("Textures/Tile.png");
            EarthTexture = content.Load<Texture2D>("Textures/EarthTransparent.png");
            GridTexture = content.Load<Texture2D>("Textures/Grid.png");

            MainMusic = content.Load<Song>("Music/Main");
            SomethingMusic = content.Load<Song>("Music/Something");
            RelaxMusic = content.Load<Song>("Music/Relax");
            BossMusic = content.Load<Song>("Music/Boss");

            ShotSound = content.Load<SoundEffect>("Sounds/Shot");
            ExplosionSound = content.Load<SoundEffect>("Sounds/Explosion");

            Task.WaitAll(loadLevelsTask);
        }

        async void loadLevels()
        {
            XmlDocument xmlDocument = new XmlDocument();
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Xml/TestLevel.xml"));
            using (Stream stream = await storageFile.OpenStreamForReadAsync())
                xmlDocument.Load(stream);

            TestLevelBlueprint = new LevelBlueprint(xmlDocument.DocumentElement);
        }
    }
}
