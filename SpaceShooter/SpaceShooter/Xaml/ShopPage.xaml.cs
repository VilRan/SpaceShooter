using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using SpaceShooter.Dynamic.Ships;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShopPage : Page, INotifyPropertyChanged
    {
        enum ItemOrigin { Shop, Inventory, WeaponSlot }

        InventoryItem draggedItem = null;
        ItemOrigin draggedItemOrigin;
        int playerIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        GamePage GamePage { get { return App.Current.GamePage; } }
        SpaceShooterGame Game { get { return GamePage.Game; } }
        Player Player { get { return Game.Session.Players[playerIndex]; } }
        PlayerShip Ship { get { return Player.Ship; } }
        Shop Shop { get { return Player.Shop; } }
        ObservableCollection<InventoryItem> Weapons { get { return Ship.WeaponSlots; } }
        string PlayerString { get { return "Player " + (playerIndex + 1); } }
        double Money { get { return Player.Money; } set { Player.Money = value; NotifyPropertyChanged(); } }

        public ShopPage(int playerIndex)
        {
            this.InitializeComponent();
            this.playerIndex = playerIndex;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;

            if (Game.State is LevelGameState)
                backButton.IsEnabled = false;
        }

        private void shop_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0] as InventoryItem;
            draggedItemOrigin = ItemOrigin.Shop;
        }

        private void inventory_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0] as InventoryItem;
            draggedItemOrigin = ItemOrigin.Inventory;
        }

        private void weapons_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (Weapons.Count <= 1)
                e.Cancel = true;
            else
            {
                draggedItem = e.Items[0] as InventoryItem;
                draggedItemOrigin = ItemOrigin.WeaponSlot;
            }
        }

        private void shop_Drop(object sender, DragEventArgs e)
        {
            if (draggedItem != null && !shop.Items.Contains(draggedItem))
            {
                if (draggedItemOrigin != ItemOrigin.Shop)
                    Money += draggedItem.Price;
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
                if (draggedItemOrigin == ItemOrigin.Shop)
                    Money -= draggedItem.Price;
                Shop.Inventory.Add(draggedItem);
                Shop.Items.Remove(draggedItem);
                Ship.TryRemoveWeapon(draggedItem);
                draggedItem = null;
            }
        }

        private void weapons_Drop(object sender, DragEventArgs e)
        {
            if (draggedItem != null && !weapons.Items.Contains(draggedItem))
            {
                if (draggedItemOrigin == ItemOrigin.Shop)
                    Money -= draggedItem.Price;
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

            if (draggedItemOrigin != ItemOrigin.Shop || Money >= draggedItem.Price)
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void weapons_DragOver(object sender, DragEventArgs e)
        {
            if (draggedItemOrigin != ItemOrigin.Shop || Money >= draggedItem.Price)
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            Session session = Game.Session;
            if (playerIndex < session.Players.Count - 1)
            {
                GamePage.NavigateTo(new ShopPage(playerIndex + 1));
            }
            else
            {
                session.StartNextLevel();
                Game.State = new LevelGameState(Game);
                App.Current.GamePage.NavigateTo();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.NavigateTo(new MainMenu());
        }
        
        void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
