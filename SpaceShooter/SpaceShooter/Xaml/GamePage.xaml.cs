using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceShooter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page, INotifyPropertyChanged
    {
		public readonly SpaceShooterGame Game;
        Page subPage;
        double healthbarValue = 100;
        double ammobarValue = 100;
        int score = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ScoreViewValue { get { return score; } set { score = value; NotifyPropertyChanged(); } }
        public double HealthbarValue  { get { return healthbarValue; } set { healthbarValue = value; NotifyPropertyChanged(); } }
        public double AmmobarValue { get { return ammobarValue; } set { ammobarValue = value; NotifyPropertyChanged(); } }

        UIElementCollection Children { get { return swapChainPanel.Children; } }

		public GamePage()
        {
            this.InitializeComponent();

			// Create the game.
			var launchArguments = string.Empty;
            Game = MonoGame.Framework.XamlGame<SpaceShooterGame>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }

        public void NavigateTo()
        {
            Children.Remove(subPage);
            subPage = null;
            Game.IsPaused = false;
        }

        public void NavigateTo(Page page)
        {
            Game.IsPaused = true;
            Children.Remove(subPage);
            Children.Add(page);
            subPage = page;
        }

        void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
