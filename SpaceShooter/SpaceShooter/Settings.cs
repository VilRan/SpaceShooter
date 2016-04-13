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
        public float MasterVolume = 1;
        public float MusicVolume { get { return MediaPlayer.Volume; } set { MediaPlayer.Volume = value; } }
        public float SoundVolume = 1;


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
