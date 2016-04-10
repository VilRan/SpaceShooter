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
        public PlayerShip Ship;
        public Controller Controller;
        public List<ShopItem> Inventory = new List<ShopItem>();
        //int money = 1000;

        public Player(AssetManager assets, Controller controller)
        {
            Ship = new PlayerShip(assets, this);
            Controller = controller;
        }
    }
}
