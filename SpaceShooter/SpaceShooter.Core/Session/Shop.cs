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
        public ObservableCollection<InventoryItem> Items = new ObservableCollection<InventoryItem>();
        public ObservableCollection<InventoryItem> Inventory = new ObservableCollection<InventoryItem>();

        public Shop()
        {
            Items.Add(new InventoryItem(new Machinegun(), 50));
            Items.Add(new InventoryItem(new Shotgun(), 100));
            Items.Add(new InventoryItem(new RocketLauncher(), 100));
            Items.Add(new InventoryItem(new FlakCannon(), 100));
            Items.Add(new InventoryItem(new DualMachinegun(), 100));
            Items.Add(new InventoryItem(new MissileLauncher(), 150));
            Items.Add(new InventoryItem(new Railgun(), 250));
        }
    }
    
}
