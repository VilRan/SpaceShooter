﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Player
    {
        public PlayerShipData ShipData { private set; get; }
        int money = 1000;

        public Player()
        {
            ShipData = new PlayerShipData();
        }
    }
}
