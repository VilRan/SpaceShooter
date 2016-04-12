using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage gamePage = App.Current.GamePage;
            Window.Current.Content = gamePage;

            SpaceShooterGame game = gamePage.Game;
            game.IsDeactived = false;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage gamePage = App.Current.GamePage;
            Window.Current.Content = gamePage;

            SpaceShooterGame game = gamePage.Game;
            game.StartNewSession(Difficulty.Nightmare);
            game.Session.StartNextLevel();
            game.IsDeactived = false;
        }

        private void scoresButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Frame.Navigate(typeof(MainMenu));
        }
    }
}
