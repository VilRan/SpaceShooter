using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
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
        float masterVolume = 1;
        float musicVolume = 1;
        float soundVolume = 1;

        public float MasterVolume { get { return masterVolume; } set { masterVolume = value; MusicVolume = musicVolume; SoundVolume = soundVolume; } }
        public float MusicVolume { get { return musicVolume; } set { musicVolume = value; MediaPlayer.Volume = musicVolume * MasterVolume; } }
        public float SoundVolume { get { return soundVolume; } set { soundVolume = value; SoundEffect.MasterVolume = soundVolume * MasterVolume; } }


        public Settings()
        {
            Task loadTask = Task.Run(() => load());
            Task.WaitAll(loadTask);
        }

        async void load()
        {
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Xml/Settings.xml"));
            var stream = await storageFile.OpenStreamForReadAsync();
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

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
    }
}
