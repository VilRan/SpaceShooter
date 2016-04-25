using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

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

        public async Task<XmlDocument> ReadXmlAsync(string fileName)
        {
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + fileName));
            return await readXmlAsync(storageFile);
        }

        public async Task<XmlDocument> ReadXmlAsync(IPlatformFile file)
        {
            StorageFile storageFile = ((UWPFile)file).StorageFile;
            return await readXmlAsync(storageFile);
        }

        async Task<XmlDocument> readXmlAsync(StorageFile storageFile)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (Stream stream = await storageFile.OpenStreamForReadAsync())
                xmlDocument.Load(stream);
            return xmlDocument;
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

        public async Task WriteXmlAsync(string fileName, XmlDocument xml)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await WriteXmlAsync(storageFile, xml);
        }

        public async Task WriteXmlAsync(IPlatformFile file, XmlDocument xml)
        {
            StorageFile storageFile = ((UWPFile)file).StorageFile;
            await WriteXmlAsync(storageFile, xml);
        }

        async Task WriteXmlAsync(StorageFile storageFile, XmlDocument xml)
        {
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                Stream stream = fileStream.AsStreamForWrite();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.Indent = true;
                settings.IndentChars = "\t";
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    xml.WriteTo(writer);
                }
            }
        }

        public async Task<IPlatformFile> TryGetPlatformFile(string fileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            IStorageItem storageItem = await storageFolder.TryGetItemAsync(fileName);
            StorageFile storageFile = storageItem as StorageFile;
            if (storageFile != null) 
                return new UWPFile(storageFile);
            return null;
        }

        public async Task<IPlatformFile> PickSaveFileAsync(params string[] fileTypes)
        {
            FileSavePicker filePicker = new FileSavePicker();
            filePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            if (fileTypes.Length > 0)
                filePicker.DefaultFileExtension = fileTypes[0];
            foreach (string fileType in fileTypes)
                filePicker.FileTypeChoices.Add(fileType, new List<string>() { fileType });
            StorageFile storageFile = await filePicker.PickSaveFileAsync();
            return new UWPFile(storageFile);
        }

        public async Task<IPlatformFile> PickOpenFileAsync(params string[] fileTypes)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            foreach (string fileType in fileTypes)
                filePicker.FileTypeFilter.Add(fileType);
            StorageFile storageFile = await filePicker.PickSingleFileAsync();
            return new UWPFile(storageFile);
        }

        public void ToggleFullscreen()
        {
            App.ToggleFullscreen();
        }
    }

    public class UWPFile : IPlatformFile
    {
        public readonly StorageFile StorageFile;

        public UWPFile(StorageFile storageFile)
        {
            StorageFile = storageFile;
        }
    }
}
