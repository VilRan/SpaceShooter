using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    class Session
    {
        public Level ActiveLevel;

        int Score = 0;
        int Money = 1000;

        public Session(SpaceShooterGame game)
        {
            ActiveLevel = new Level(game);
        }
    }
}
