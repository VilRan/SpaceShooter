using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceShooter.Xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OptionsPage : Page, INotifyPropertyChanged
    {
        GamePage GamePage { get { return App.Current.GamePage; } }
        SpaceShooterGame Game { get { return GamePage.Game; } }
        Settings Settings { get { return Game.Settings; } }
        public double MasterVolume { get { return Settings.MasterVolume * 100; } set { Settings.MasterVolume = (float)value / 100; NotifyPropertyChanged(); } }
        public double MusicVolume { get { return Settings.MusicVolume * 100; } set { Settings.MusicVolume = (float)value / 100; NotifyPropertyChanged(); } }
        public double SoundVolume { get { return Settings.SoundVolume * 100; } set { Settings.SoundVolume = (float)value / 100; NotifyPropertyChanged(); } }
        public double ParticleDensity { get { return Settings.ParticleDensity * 100; } set { Settings.ParticleDensity = (float)value / 100; NotifyPropertyChanged(); } }

        public OptionsPage()
        {
            this.InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;

            ApplicationView view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
                screenModeComboBox.SelectedItem = fullscreenItem;
            else
                screenModeComboBox.SelectedItem = windowedItem;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void backToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.NavigateTo(new MainMenu());
        }

        private void screenModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.First() == fullscreenItem)
                App.EnterFullscreen();
            else if (e.AddedItems.First() == windowedItem)
                App.ExitFullscreen();
        }

        void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
