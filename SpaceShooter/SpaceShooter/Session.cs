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
        public Player Player;
        public Shop Shop;
        public Level ActiveLevel;
        public int Score = 0;
        public SpaceShooterGame Game;

        public Session(SpaceShooterGame game, Difficulty difficulty)
        {
            Difficulty = difficulty;
            Game = game;
            Shop = new Shop();
            Player = new Player(game.Assets);
        }

        public void PlayNextLevel()
        {
            Player.Ship.Repair(10000);
            ActiveLevel = new Level(this, Game.Assets.TestLevelBlueprint);
        }
    }
}
