﻿using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Player
    {
        public PlayerShip Ship;
        public Controller Controller;
        //int money = 1000;

        public Player(AssetManager assets)
        {
            Ship = new PlayerShip(assets, this);
            Controller = new Controller();
        }
    }
}
