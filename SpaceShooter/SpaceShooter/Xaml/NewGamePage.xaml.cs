using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewGamePage : Page
    {
        GamePage GamePage { get { return App.Current.GamePage; } }
        SpaceShooterGame Game { get { return GamePage.Game; } }

        public NewGamePage()
        {
            this.InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            Difficulty difficulty = Difficulty.Casual;
            if ((bool)hardcoreRadioButton.IsChecked)
                difficulty = Difficulty.Hardcore;
            else if ((bool)nightmareRadioButton.IsChecked)
                difficulty = Difficulty.Nightmare;

            int numberOfPlayers = 1;
            if ((bool)multiPlayerRadioButton.IsChecked)
                numberOfPlayers = 2;

            Game.StartNewSession(difficulty, numberOfPlayers);
            GamePage.NavigateTo(new ShopPage(0));
        }
    }
}
