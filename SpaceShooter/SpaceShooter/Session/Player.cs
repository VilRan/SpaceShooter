using SpaceShooter.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Player
    {
        public Shop Shop;
        public PlayerShip Ship;
        public Controller Controller;
        //int money = 1000;

        public Player(AssetManager assets, Controller controller)
        {
            Shop = new Shop();
            Ship = new PlayerShip(assets, this);
            Controller = controller;
        }
    }
}
