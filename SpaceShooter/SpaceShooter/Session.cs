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

        SpaceShooterGame game;

        public Session(SpaceShooterGame game)
        {
            this.game = game;
            Player = new Player(game.Assets);
        }

        public void PlayNextLevel()
        {
            ActiveLevel = new Level(game, game.Assets.TestLevelBlueprint);
        }
    }
}
