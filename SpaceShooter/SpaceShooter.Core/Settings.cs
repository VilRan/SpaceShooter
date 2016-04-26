using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.UI.ViewManagement;

namespace SpaceShooter
{
    public class Settings
    {
        public Dictionary<string, Controller> Controllers = new Dictionary<string, Controller>();
        SpaceShooterGame game;
        float masterVolume = 1;
        float musicVolume = 1;
        float soundVolume = 1;
        float particleDensity = 1;

        public float MasterVolume { get { return masterVolume; } set { masterVolume = value; MusicVolume = musicVolume; SoundVolume = soundVolume; } }
        public float MusicVolume { get { return musicVolume; } set { musicVolume = value; MediaPlayer.Volume = musicVolume * MasterVolume; } }
        public float SoundVolume { get { return soundVolume; } set { soundVolume = value; SoundEffect.MasterVolume = soundVolume * MasterVolume; } }
        public float ParticleDensity { get { return particleDensity; } set { particleDensity = value; } }
        IPlatformAsync platform { get { return SpaceShooterGame.Platform; } }

        public Settings(SpaceShooterGame game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            MasterVolume = masterVolume;
        }

        public async void LoadFromFile()
        {
            XmlDocument xmlDocument = null;
            try
            {
                IPlatformFile file = await platform.TryGetPlatformFile("Settings.xml");
                xmlDocument = await platform.ReadXmlAsync(file);
            }
            catch
            {
                xmlDocument = await platform.ReadXmlAsync("Assets/Xml/DefaultSettings.xml");
            }

            foreach (XmlElement setting in xmlDocument.DocumentElement.ChildNodes.OfType<XmlElement>())
            {
                switch (setting.Name)
                {
                    case "Display":
                        string mode = setting.GetAttribute("Mode");
                        if (mode == "Fullscreen")
                            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
                        else
                            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
                        break;

                    case "Volume":
                        masterVolume = float.Parse(setting.GetAttribute("Master"), CultureInfo.InvariantCulture);
                        musicVolume = float.Parse(setting.GetAttribute("Music"), CultureInfo.InvariantCulture);
                        soundVolume = float.Parse(setting.GetAttribute("Sound"), CultureInfo.InvariantCulture);
                        break;

                    case "Particles":
                        particleDensity = float.Parse(setting.GetAttribute("Density"), CultureInfo.InvariantCulture);
                        break;

                    case "KeyBindings":
                        foreach (XmlElement controller in setting.ChildNodes.OfType<XmlElement>())
                        {
                            string controllerID = controller.GetAttribute("ID");
                            Controllers.Add(controllerID, new Controller(controller));
                        }
                        break;
                }
            }
        }

        public async Task SaveToFile()
        {
            await platform.WriteXmlAsync("Settings.xml", ToXml());
        }

        public XmlDocument ToXml()
        {
            XmlDocument xml = new XmlDocument();
            XmlElement settings = xml.CreateElement("Settings");
            xml.AppendChild(settings);

            XmlElement display = xml.CreateElement("Display");
            XmlAttribute mode = xml.CreateAttribute("Mode");
            if (game.IsFullscreen)
                mode.Value = "Fullscreen";
            else
                mode.Value = "Windowed";
            display.Attributes.Append(mode);
            settings.AppendChild(display);

            XmlElement volume = xml.CreateElement("Volume");
            XmlAttribute master = xml.CreateAttribute("Master");
            master.Value = "" + MasterVolume;
            XmlAttribute music = xml.CreateAttribute("Music");
            music.Value = "" + MusicVolume;
            XmlAttribute sound = xml.CreateAttribute("Sound");
            sound.Value = "" + SoundVolume;
            volume.Attributes.Append(master);
            volume.Attributes.Append(music);
            volume.Attributes.Append(sound);
            settings.AppendChild(volume);

            XmlElement particles = xml.CreateElement("Particles");
            XmlAttribute density = xml.CreateAttribute("Density");
            density.Value = "" + ParticleDensity;
            particles.Attributes.Append(density);
            settings.AppendChild(particles);
            
            XmlElement keyBindings = xml.CreateElement("KeyBindings");
            foreach (KeyValuePair<string, Controller> controller in Controllers)
            {
                keyBindings.AppendChild(controller.Value.ToXml(xml, controller.Key));
            }
            settings.AppendChild(keyBindings);

            return xml;
        }
    }
}
