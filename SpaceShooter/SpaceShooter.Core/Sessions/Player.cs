using SpaceShooter.Dynamic.Ships;

namespace SpaceShooter
{
    public class Player
    {
        public Shop Shop;
        public PlayerShip Ship;
        public Controller Controller;
        double money = 250;

        public double Money { get { return money; } set { money = value; } }

        public Player(AssetManager assets, Controller controller)
        {
            Shop = new Shop();
            Ship = new PlayerShip(assets, this);
            Controller = controller;
        }
    }
}
