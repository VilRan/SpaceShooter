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

        static IPlatformAsync Platform { get { return SpaceShooterGame.Platform; } }

        public HighscoreCollection()
        {

        }

        public void Add(Highscore highscore)
        {
            items.Add(highscore);
            items.Sort();
        }

        public async Task LoadFromFile()
        {
            IList<string> lines = await Platform.ReadLinesAsync("Highscore.txt");
            foreach (string line in lines)
                items.Add(new Highscore(line));
            items.Sort();
        }

        public async Task SaveToFile()
        {
            await Platform.WriteTextAsync("Highscore.txt", ToString());
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
