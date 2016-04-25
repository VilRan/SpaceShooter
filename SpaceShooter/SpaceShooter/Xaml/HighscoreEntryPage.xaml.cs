using SpaceShooter.States;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnterHighscorePage : Page
    {
        int score;

        string scoreString { get { return "Score: " + score; } }
        GamePage GamePage { get { return App.Current.GamePage; } }
        SpaceShooterGame Game { get { return GamePage.Game; } }

        public EnterHighscorePage(int score)
        {
            this.InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            this.score = score;
        }

        private void saveScoreButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndExit();
        }

        private void nameBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                SaveAndExit();
        }

        void SaveAndExit()
        {
            Game.Highscores.Add(new Highscore(nameBox.Text, score));
            GamePage.NavigateTo(new HighscorePage());
            Game.State = new MenuGameState(Game);
        }
    }
}
