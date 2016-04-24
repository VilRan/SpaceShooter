using System;
using SpaceShooter.Xaml;

namespace SpaceShooter
{
    class UWPSpaceShooterUI : ISpaceShooterUI
    {
        GamePage gamePage { get { return App.Current.GamePage; } }

        public void NavigateToGame()
        {
            gamePage.NavigateTo();
        }

        public void NavigateToHighscoreEntry()
        {
            gamePage.NavigateTo(new EnterHighscorePage(gamePage.Game.Session.Score));
        }

        public void NavigateToMainMenu()
        {
            gamePage.NavigateTo(new MainMenu());
        }
    }
}
