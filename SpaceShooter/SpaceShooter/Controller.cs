using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public enum Control
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Fire,
        PreviousWeapon,
        NextWeapon,
        Weapon1,
        Weapon2,
        Weapon3,
        Weapon4,
        Weapon5,
        Weapon6,
        Weapon7,
        Weapon8,
    }

    public class Controller
    {
        public Dictionary<Control, Keys[]> Mappings = new Dictionary<Control, Keys[]>();
        KeyboardState previousKeyboard;

        public Controller()
        {
            foreach (Control control in Enum.GetValues(typeof(Control)))
            {
                Mappings.Add(control, new Keys[2]);
            }
            previousKeyboard = Keyboard.GetState();
        }

        public void Update()
        {
            previousKeyboard = Keyboard.GetState();
        }

        public bool IsControlDown(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Mappings[control].Any(key => keyboard.IsKeyDown(key));
        }

        public bool IsControlUp(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Mappings[control].Any(key => keyboard.IsKeyUp(key));
        }

        public bool IsControlPressed(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Mappings[control].Any(
                key => keyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key));
        }

        public bool IsControlReleased(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Mappings[control].Any(
                key => keyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key));
        }
    }
}
