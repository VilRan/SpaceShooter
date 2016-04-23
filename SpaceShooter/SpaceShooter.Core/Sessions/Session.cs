using System.Collections.Generic;

namespace SpaceShooter
{
    public class Session
    {
        public Difficulty Difficulty;
        public List<Player> Players = new List<Player>();
        public Level ActiveLevel;
        public SpaceShooterGame Game;
        int score = 0;

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                App.Current.GamePage.ScoreViewValue = value;
            }
        }

        public Session(SpaceShooterGame game, Difficulty difficulty, int numberOfPlayers)
        {
            Difficulty = difficulty;
            Game = game;
                        
            Players.Add(new Player(game.Assets, game.Settings.Controllers["Keyboard1"]));

            if (numberOfPlayers >= 2)
            {
                Players.Add(new Player(game.Assets, game.Settings.Controllers["Keyboard2"]));
            }            
        }

        public void StartNextLevel()
        {
            foreach (Player player in Players)
                player.Ship.Repair(10000);
            ActiveLevel = new Level(this, Game.Assets.TestLevelBlueprint);
        }
    }
}
