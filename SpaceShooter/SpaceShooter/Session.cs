using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Session
    {
        public Player Player;
        public Level ActiveLevel;
        public int Score = 0;
        public SpaceShooterGame Game;

        public Session(SpaceShooterGame game)
        {
            this.Game = game;
            Player = new Player(game.Assets);
        }

        public void PlayNextLevel()
        {
            Player.Ship.Repair(10000);
            ActiveLevel = new Level(this, Game.Assets.TestLevelBlueprint);
        }
    }
}
