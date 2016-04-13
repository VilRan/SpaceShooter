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

            foreach (XmlElement set in xmlDocument.DocumentElement.ChildNodes.OfType<XmlElement>())
            {
                string setID = set.GetAttribute("ID");
                Controller controller = new Controller();
                Controllers.Add(setID, controller);

                foreach (XmlElement action in set.ChildNodes.OfType<XmlElement>())
                {
                    string actionID = action.GetAttribute("ID");
                    Control actionType = Control.Invalid;
                    Enum.TryParse(actionID, out actionType);
                    if (actionType != Control.Invalid)
                    {
                        int i = 0;
                        foreach (XmlElement key in action.ChildNodes.OfType<XmlElement>())
                        {
                            int keyID;
                            if (int.TryParse(key.GetAttribute("ID"), out keyID))
                            {
                                controller.Mappings[actionType][i] = (Keys)keyID;
                            }
                            i++;
                        }
                    }
                }
            }
        }
    }
}
