﻿using SpaceShooter.States;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        GamePage GamePage { get { return App.Current.GamePage; } }
        SpaceShooterGame Game { get { return GamePage.Game; } }

        public MainMenu()
        {
            this.InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;

            if (Game.State is MenuGameState || Game.Session.PlayersAlive.Count() == 0)
                continueButton.IsEnabled = false;
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            if (Game.State is ShopGameState)
                GamePage.NavigateTo(new ShopPage(0));
            else
                GamePage.NavigateTo();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.NavigateTo(new NewGamePage());     
        }

        private void scoresButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.NavigateTo(new HighscorePage());
        }

        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.NavigateTo(new OptionsPage());
        }
    }
}
