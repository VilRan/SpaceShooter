using SpaceShooter.Xaml;

namespace SpaceShooter
{
    class UWPSpaceShooterUI : ISpaceShooterUI
    {
        public void NavigateToGame()
        {
            App.Current.GamePage.NavigateTo();
        }

        public void NavigateToHighscoreEntry()
        {
            GamePage gamePage = App.Current.GamePage;
            gamePage.NavigateTo(new EnterHighscorePage(gamePage.Game.Session.Score));
        }

        public void NavigateToMainMenu()
        {
            App.Current.GamePage.NavigateTo(new MainMenu());
        }
    }
}
