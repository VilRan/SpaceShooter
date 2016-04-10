using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceShooter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page, INotifyPropertyChanged
    {
		public readonly SpaceShooterGame Game;
        double healthbarValue = 100;

        public event PropertyChangedEventHandler PropertyChanged;

        public double HealthbarValue
        {
            get
            {
                return healthbarValue;
            }

            set
            {
                healthbarValue = value;
                NotifyPropertyChanged();
            }
        }

		public GamePage()
        {
            this.InitializeComponent();

			// Create the game.
			var launchArguments = string.Empty;
            Game = MonoGame.Framework.XamlGame<SpaceShooterGame>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
            MediaPlayer.Play(Game.Assets.SomethingMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsMuted = false;
        }

        void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
