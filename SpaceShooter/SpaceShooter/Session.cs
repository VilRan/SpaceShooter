using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Session
    {
        public Player Player = new Player();
        public Level ActiveLevel;
        public int Score = 0;

        public Session(SpaceShooterGame game)
        {
            ActiveLevel = new Level(game, game.Assets.TestLevelBlueprint);
        }
    }
}
