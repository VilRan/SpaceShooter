﻿using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter
{
    public class Session
    {
        public Difficulty Difficulty;
        public List<Player> Players = new List<Player>();
        public Level ActiveLevel;
        public SpaceShooterGame Game;
        int score = 0;
        int level = 0;

        public int Score
        {
            get { return score; }
            set { score = value; App.Current.GamePage.ScoreViewValue = value; }
        }
        public int LevelNumber { get { return level; } }
        public IEnumerable<Player> PlayersAlive { get { return Players.Where(p => p.Ship.IsAlive); } }

        public Session(SpaceShooterGame game, Difficulty difficulty, int numberOfPlayers)
        {
            Difficulty = difficulty;
            Game = game;

            if (numberOfPlayers >= 1)
                Players.Add(new Player(game.Assets, game.Settings.Controllers["Keyboard1"]));
            if (numberOfPlayers >= 2)
                Players.Add(new Player(game.Assets, game.Settings.Controllers["Keyboard2"]));
        }

        public void StartNextLevel()
        {
            level++;
            foreach (Player player in Players)
            {
                player.Ship.Repair(10000);
                player.Money += 50;
            }
            ActiveLevel = new Level(this, new LevelBlueprint(16000, 1080, Game.Random, this));
        }
    }
}
