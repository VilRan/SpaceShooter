using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SpaceShooter
{
    public class UWPInterface : IPlatformAsync
    {
        public IList<string> ReadLines(string fileName)
        {
            Task<IList<string>> readLinesTask = Task.Run(() => ReadLinesAsync(fileName));
            readLinesTask.Wait(); 
            return readLinesTask.Result;
        }

        public async Task<IList<string>> ReadLinesAsync(string fileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            IStorageItem storageItem = await storageFolder.TryGetItemAsync(fileName);
            StorageFile storageFile = storageItem as StorageFile;
            if (storageItem != null)
            {
                return await FileIO.ReadLinesAsync(storageFile);
            }
            return new List<string>();
        }

        public string ReadText(string fileName)
        {
            Task<string> readTextTask = Task.Run(() => ReadTextAsync(fileName));
            readTextTask.Wait();
            return ReadTextAsync(fileName).Result;
        }

        public async Task<string> ReadTextAsync(string fileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            IStorageItem storageItem = await storageFolder.TryGetItemAsync(fileName);
            StorageFile storageFile = storageItem as StorageFile;
            if (storageItem != null)
            {
                return await FileIO.ReadTextAsync(storageFile);
            }
            return "";
        }

        public void WriteText(string fileName, string text)
        {
            Task writeTextTask = Task.Run(() => WriteTextAsync(fileName, text));
            writeTextTask.Wait();
        }

        public async Task WriteTextAsync(string fileName, string text)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, text);
        }
    }
}
