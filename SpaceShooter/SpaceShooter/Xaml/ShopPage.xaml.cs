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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShopPage : Page
    {
        ShopItem draggedItem = null;

        public Shop Shop { get { return (Application.Current as App).GamePage.Game.Session.Shop; } }

        public ShopPage()
        {
            this.InitializeComponent();
        }

        private void items_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0] as ShopItem;
        }

        private void inventory_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggedItem = e.Items[0] as ShopItem;
        }

        private void items_Drop(object sender, DragEventArgs e)
        {

            if (draggedItem != null && !items.Items.Contains(draggedItem))
            {
                Shop.Items.Add(draggedItem);
                Shop.Inventory.Remove(draggedItem);
                draggedItem = null;
            }

        }

        private void inventory_Drop(object sender, DragEventArgs e)
        {
            
            if (draggedItem != null && !inventory.Items.Contains(draggedItem))
            {
                Shop.Inventory.Add(draggedItem);
                Shop.Items.Remove(draggedItem);
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

    }
}
