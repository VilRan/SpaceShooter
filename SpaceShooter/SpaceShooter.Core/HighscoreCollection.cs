using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace SpaceShooter
{
    public class HighscoreCollection
    {
        public List<Highscore> Items = new List<Highscore>();
        
        public HighscoreCollection()
        {
            string fileString = File.ReadAllText("Assets/Highscore.txt");

            string[] lines = fileString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                Items.Add(new Highscore(line));
            }

            Items.Sort();
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
