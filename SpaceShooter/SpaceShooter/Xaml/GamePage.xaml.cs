﻿using System.ComponentModel;
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
        int score = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ScoreViewValue { get { return score; } set { score = value; NotifyPropertyChanged(); } }

        UIElementCollection Children { get { return swapChainPanel.Children; } }

		public GamePage()
        {
            this.InitializeComponent();
            
			var launchArguments = string.Empty;
            Game = MonoGame.Framework.XamlGame<SpaceShooterGame>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }

        /// <summary>
        /// Removes any sub-page and unpauses the game.
        /// </summary>
        public void NavigateTo()
        {
            if (subPage != null)
            {
                Children.Remove(subPage);
                subPage = null;
            }
            Game.IsPaused = false;
        }

        /// <summary>
        /// Pauses the game and overlays a new sup-page.
        /// </summary>
        /// <param name="page"></param>
        public void NavigateTo(Page page)
        {
            Game.IsPaused = true;
            if (subPage != null)
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
