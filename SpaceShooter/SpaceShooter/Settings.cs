using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel;
using Windows.Storage;

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
            Task loadKeyBindingsTask = Task.Run(() => loadKeyBindings());
            Task.WaitAll(loadKeyBindingsTask);
        }

        async void loadKeyBindings()
        {
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Xml/KeyBindings.xml"));
            var stream = await storageFile.OpenStreamForReadAsync();
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            foreach (XmlElement controllerElement in xmlDocument.DocumentElement.ChildNodes.OfType<XmlElement>())
            {
                string controllerID = controllerElement.GetAttribute("ID");
                Controllers.Add(controllerID, new Controller(controllerElement));
            }
        }
    }
}
