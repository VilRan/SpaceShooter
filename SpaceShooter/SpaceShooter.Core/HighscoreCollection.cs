using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SpaceShooter
{
    public class HighscoreCollection
    {
        public List<Highscore> items = new List<Highscore>();
        public IEnumerable<Highscore> Items { get { return items; } }

        public HighscoreCollection()
        {
            Task loadTask = new Task(() => LoadFromFile());
            loadTask.RunSynchronously();
        }

        public void Add(Highscore highscore)
        {
            items.Add(highscore);
            items.Sort();
        }

        public async void LoadFromFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            IStorageItem storageItem = await storageFolder.TryGetItemAsync("Highscore.txt");
            StorageFile storageFile = storageItem as StorageFile;
            if (storageItem != null)
            {
                foreach (string line in await FileIO.ReadLinesAsync(storageFile))
                    items.Add(new Highscore(line));
                items.Sort();
            }
        }

        public async void SaveToFile()
        {
            Task<string> stringTask = Task.Run(() => ToString());
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.CreateFileAsync("Highscore.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, await stringTask);
        }

        public override string ToString()
        {
            string s = "";
            foreach (var item in items)
                s += item.Name + ";" + item.Score + Environment.NewLine;
            return s;
        }
    }

    public class Highscore : IComparable<Highscore>
    {
        public string Name;
        public int Score;

        public Highscore(string line)
        {
            string[] items = line.Split(';');
            Name = items[0];
            Score = int.Parse(items[1]);
        }

        public Highscore(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public int CompareTo(Highscore other)
        {
            if (Score < other.Score)
                return 1;
            if (Score > other.Score)
                return -1;
            return 0;
        }
    }
}
