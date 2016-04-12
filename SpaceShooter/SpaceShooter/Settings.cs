using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class Settings
    {
        public Controller Keyboard1 = new Controller();
        public Controller Keyboard2 = new Controller();

        public Settings()
        {
            Keyboard1.Mappings[Control.MoveUp][0] = Keys.Up;
            Keyboard1.Mappings[Control.MoveDown][0] = Keys.Down;
            Keyboard1.Mappings[Control.MoveLeft][0] = Keys.Left;
            Keyboard1.Mappings[Control.MoveRight][0] = Keys.Right;
            Keyboard1.Mappings[Control.Fire][0] = Keys.Space;
            Keyboard1.Mappings[Control.MoveUp][1] = Keys.NumPad8;
            Keyboard1.Mappings[Control.MoveDown][1] = Keys.NumPad2;
            Keyboard1.Mappings[Control.MoveLeft][1] = Keys.NumPad4;
            Keyboard1.Mappings[Control.MoveRight][1] = Keys.NumPad6;
            Keyboard1.Mappings[Control.Fire][1] = Keys.Enter;
            Keyboard1.Mappings[Control.PreviousWeapon][1] = Keys.NumPad7;
            Keyboard1.Mappings[Control.NextWeapon][1] = Keys.NumPad9;
            Keyboard1.Mappings[Control.Weapon1][0] = Keys.D1;
            Keyboard1.Mappings[Control.Weapon2][0] = Keys.D2;
            Keyboard1.Mappings[Control.Weapon3][0] = Keys.D3;
            Keyboard1.Mappings[Control.Weapon4][0] = Keys.D4;
            Keyboard1.Mappings[Control.Weapon5][0] = Keys.D5;
            Keyboard1.Mappings[Control.Weapon6][0] = Keys.D6;
            Keyboard1.Mappings[Control.Weapon7][0] = Keys.D7;
            Keyboard1.Mappings[Control.Weapon8][0] = Keys.D8;
            
            Keyboard2.Mappings[Control.MoveUp][0] = Keys.W;
            Keyboard2.Mappings[Control.MoveDown][0] = Keys.S;
            Keyboard2.Mappings[Control.MoveLeft][0] = Keys.A;
            Keyboard2.Mappings[Control.MoveRight][0] = Keys.D;
            Keyboard2.Mappings[Control.Fire][0] = Keys.LeftControl;
            Keyboard2.Mappings[Control.PreviousWeapon][0] = Keys.Q;
            Keyboard2.Mappings[Control.NextWeapon][0] = Keys.E;
            Keyboard2.Mappings[Control.Weapon1][0] = Keys.D1;
            Keyboard2.Mappings[Control.Weapon2][0] = Keys.D2;
            Keyboard2.Mappings[Control.Weapon3][0] = Keys.D3;
            Keyboard2.Mappings[Control.Weapon4][0] = Keys.D4;
            Keyboard2.Mappings[Control.Weapon5][0] = Keys.D5;
            Keyboard2.Mappings[Control.Weapon6][0] = Keys.D6;
            Keyboard2.Mappings[Control.Weapon7][0] = Keys.D7;
            Keyboard2.Mappings[Control.Weapon8][0] = Keys.D8;
        }
    }
}
