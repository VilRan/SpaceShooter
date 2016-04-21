using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class OptionsPage : Page, INotifyPropertyChanged
    {
        public Settings Settings { get { return App.Current.GamePage.Game.Settings; }}

        public double MasterVolume { get { return Settings.MasterVolume * 100; } set { Settings.MasterVolume = (float)value / 100; NotifyPropertyChanged(); } }

        public double MusicVolume { get { return Settings.MusicVolume * 100; } set { Settings.MusicVolume = (float)value / 100; NotifyPropertyChanged(); } }

        public double SoundVolume { get { return Settings.SoundVolume * 100; } set { Settings.SoundVolume = (float)value / 100; NotifyPropertyChanged(); } }

        public OptionsPage()
        {
            this.InitializeComponent();

            ApplicationView view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                screenModeComboBox.SelectedItem = fullscreenItem;
            }
            else
            {
                screenModeComboBox.SelectedItem = windowedItem;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void backToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainMenu();
        }

        private void masterVolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

        }

        private void screenModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((ComboBoxItem)e.AddedItems.First() == fullscreenItem)
            {
                App.EnterFullscreen();
            }
            else if ((ComboBoxItem)e.AddedItems.First() == windowedItem)
            {
                App.ExitFullscreen();
            }
        }

        void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
