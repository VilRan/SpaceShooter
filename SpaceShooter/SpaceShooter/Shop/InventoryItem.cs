using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
