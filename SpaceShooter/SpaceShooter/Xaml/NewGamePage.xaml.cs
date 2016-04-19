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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewGamePage : Page
    {
        public NewGamePage()
        {
            this.InitializeComponent();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            Difficulty difficulty = Difficulty.Casual;
            if ((bool)casualRadioButton.IsChecked)
            {
                difficulty = Difficulty.Casual;
            }
            if ((bool)hardcoreRadioButton.IsChecked)
            {
                difficulty = Difficulty.Hardcore;
            }
            if ((bool)nightmareRadioButton.IsChecked)
            {
                difficulty = Difficulty.Nightmare;
            }
            int numberOfPlayers = 1;
            if ((bool)multiPlayerRadioButton.IsChecked)
            {
                numberOfPlayers = 2;
            }
            App.Current.GamePage.Game.StartNewSession(difficulty, numberOfPlayers);
            Window.Current.Content = new ShopPage(0);            
        }
    }
}
