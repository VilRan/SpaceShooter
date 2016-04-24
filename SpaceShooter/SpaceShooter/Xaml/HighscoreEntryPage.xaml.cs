﻿using Windows.UI.Xaml;
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
            GamePage gamepage = App.Current.GamePage;
            gamepage.Game.Highscores.Add(new Highscore(nameBox.Text, score));
            gamepage.NavigateTo(new HighscorePage());
        }
    }
}