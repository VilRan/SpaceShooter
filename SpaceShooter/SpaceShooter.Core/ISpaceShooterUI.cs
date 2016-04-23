using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter
{
    public interface ISpaceShooterUI
    {
        void NavigateToGame();
        void NavigateToMainMenu();
        void NavigateToHighscoreEntry();
        void SetHealthbar(double value);
    }
}
