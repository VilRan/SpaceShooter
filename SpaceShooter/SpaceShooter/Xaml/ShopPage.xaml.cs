using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SpaceShooter;
using System.Collections.ObjectModel;
using SpaceShooter.Dynamic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShopPage : Page
    {
        InventoryItem draggedItem = null;

        public Player Player { get { return App.Current.GamePage.Game.Session.Players[0]; } }
        public PlayerShip Ship { get { return Player.Ship; } }
        public Shop Shop { get { return Player.Shop; } }
        public ObservableCollection<InventoryItem> Weapons { get { return Ship.WeaponSlots; } }

        public ShopPage()
        {
            this.InitializeComponent();
        }

        private void items_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0] as InventoryItem;
        }

        private void inventory_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0] as InventoryItem;
        }

        private void weapons_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (Weapons.Count <= 1)
                e.Cancel = true;
            else
                draggedItem = e.Items[0] as InventoryItem;
        }

        private void items_Drop(object sender, DragEventArgs e)
        {
            if (draggedItem != null && !shop.Items.Contains(draggedItem))
            {
                Shop.Inventory.Remove(draggedItem);
                Shop.Items.Add(draggedItem);
                Ship.TryRemoveWeapon(draggedItem);
                draggedItem = null;
            }
        }

        private void inventory_Drop(object sender, DragEventArgs e)
        {
            if (draggedItem != null && !inventory.Items.Contains(draggedItem))
            {
                Shop.Inventory.Add(draggedItem);
                Shop.Items.Remove(draggedItem);
                Ship.TryRemoveWeapon(draggedItem);
                draggedItem = null;
            }
        }

        private void weapons_Drop(object sender, DragEventArgs e)
        {
            if (draggedItem != null && !inventory.Items.Contains(draggedItem))
            {
                Shop.Inventory.Remove(draggedItem);
                Shop.Items.Remove(draggedItem);
                Weapons.Add(draggedItem);
                draggedItem = null;
            }
        }

        private void items_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void inventory_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void weapons_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = App.Current.GamePage;
            SpaceShooterGame game = App.Current.GamePage.Game;
            game.Session.StartNextLevel();
            game.State = new LevelGameState(game);
            game.IsDeactived = false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainMenu();
        }
    }
}
