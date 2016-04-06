using SpaceShooter.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Shop
    {
        public ObservableCollection<ShopItem> Items = new ObservableCollection<ShopItem>();
        public ObservableCollection<ShopItem> Inventory = new ObservableCollection<ShopItem>();

        public Shop()
        {
            Items.Add(new ShopItem(new Machinegun(), 50));
            Items.Add(new ShopItem(new RocketLauncher(), 60));
        }
    }
    
}
