using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace SpaceShooter
{
    public class HighscoreCollection
    {
        public List<Highscore> items = new List<Highscore>();
        public IEnumerable<Highscore> Items { get { return items; } }

        public HighscoreCollection()
        {
            string fileString = File.ReadAllText("Assets/Highscore.txt");
            string[] lines = fileString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
                items.Add(new Highscore(line));
            items.Sort();
        }

        public void Add(Highscore highscore)
        {
            items.Add(highscore);
            items.Sort();
        }

        public void SaveToFile()
        {
            string text = "";
            foreach (var item in items)
                text += item.Name + ";" + item.Score + Environment.NewLine;
            File.WriteAllText("Assets/Highscore.txt", text);
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
