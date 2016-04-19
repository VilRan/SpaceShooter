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
    public sealed partial class HighscorePage : Page
    {
        int score;
        public string highcoresToTextBlock { get { return File.ReadAllText("Assets/Higscores.txt"); }}

        public HighscorePage()
        {
            this.InitializeComponent();
        }

        public string HighscoreView
        {
            get
            {
                return highcoresToTextBlock;
            }
        }

        private void backToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainMenu();
        }
    }
}
