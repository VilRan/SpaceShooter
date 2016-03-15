using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Session
    {
        public readonly SpaceShooterGame Game;
        public Player Player = new Player();
        public Level ActiveLevel;
        public int Score = 0;

        public Session(SpaceShooterGame game)
        {
            Game = game;
            ActiveLevel = new Level(this, game.Assets.TestLevelBlueprint);
        }
    }
}
