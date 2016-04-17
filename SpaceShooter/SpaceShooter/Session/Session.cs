using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Session
    {
        public Difficulty Difficulty;
        public List<Player> Players = new List<Player>();
        public Level ActiveLevel;
        public int Score = 0;
        public SpaceShooterGame Game;

        public Session(SpaceShooterGame game, Difficulty difficulty)
        {
            Difficulty = difficulty;
            Game = game;
            
            Players.Add(new Player(game.Assets, game.Settings.Controllers["Keyboard1"]));
            Players.Add(new Player(game.Assets, game.Settings.Controllers["Keyboard2"]));
        }

        public void StartNextLevel()
        {
            foreach (Player player in Players)
                player.Ship.Repair(10000);
            ActiveLevel = new Level(this, Game.Assets.TestLevelBlueprint);
        }
    }
}
