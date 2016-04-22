using SpaceShooter.Weapons;

namespace SpaceShooter
{
    public class InventoryItem
    {
        public Weapon Weapon;
        public double Price;

        public InventoryItem(Weapon weapon, double price)
        {
            Weapon = weapon;
            Price = price;
        }
    }
}
