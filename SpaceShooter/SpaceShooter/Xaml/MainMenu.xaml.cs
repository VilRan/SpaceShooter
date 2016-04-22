using Windows.ApplicationModel.Core;
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
        public MainMenu()
        {
            this.InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage gamePage = App.Current.GamePage;
            SpaceShooterGame game = gamePage.Game;
            
            if (game.State is LevelGameState)
            {
                if (game.Session.ActiveLevel != null)
                    App.Current.GamePage.NavigateTo();
                else
                    App.Current.GamePage.NavigateTo(new ShopPage(0));
            }
            else if (game.State is EditorGameState)
                App.Current.GamePage.NavigateTo();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.GamePage.NavigateTo(new NewGamePage());     
        }

        private void scoresButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.GamePage.NavigateTo(new HighscorePage());
        }

        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.GamePage.NavigateTo(new OptionsPage());
        }
    }
}
