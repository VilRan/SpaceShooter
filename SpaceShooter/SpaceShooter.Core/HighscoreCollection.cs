using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace SpaceShooter
{
    public class HighscoreCollection
    {
        public ObservableCollection<Highscore> Items = new ObservableCollection<Highscore>();
        
        public HighscoreCollection()
        {
            string fileString = File.ReadAllText("Assets/Highscore.txt");

            string[] lines = fileString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                Items.Add(new Highscore(line));
            }

        }
    }

    public class Highscore
    {
        public string Name;
        public int Score;

        public Highscore(string line)
        {
            string[] items = line.Split(';');
            Name = items[0];
            Score = int.Parse(items[1]);
        }
    }
}
