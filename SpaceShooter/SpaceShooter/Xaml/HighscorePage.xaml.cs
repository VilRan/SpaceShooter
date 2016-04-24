using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HighscorePage : Page
    {
        ObservableCollection<Highscore> highscores;

        public ObservableCollection<Highscore> Highscores { get { return highscores; } }
        GamePage GamePage { get { return App.Current.GamePage; } }
        SpaceShooterGame Game { get { return GamePage.Game; } }

        public HighscorePage()
        {
            this.InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            highscores = new ObservableCollection<Highscore>(Game.Highscores.Items);
        }    

        private void backToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.NavigateTo(new MainMenu());
        }
    }
}
