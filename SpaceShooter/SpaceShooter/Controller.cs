using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpaceShooter
{
    public enum Action
    {
        Invalid,
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
        public Dictionary<Action, List<Keys>> Bindings = new Dictionary<Action, List<Keys>>();
        KeyboardState previousKeyboard;

        public Controller(XmlElement xml)
        {
            foreach (Action action in Enum.GetValues(typeof(Action)))
                Bindings.Add(action, new List<Keys>());

            foreach (XmlElement action in xml.ChildNodes.OfType<XmlElement>())
            {
                string actionID = action.GetAttribute("ID");
                Action actionType = Action.Invalid;
                Enum.TryParse(actionID, out actionType);
                if (actionType != Action.Invalid)
                {
                    foreach (XmlElement key in action.ChildNodes.OfType<XmlElement>())
                    {
                        int keyID;
                        if (int.TryParse(key.GetAttribute("ID"), out keyID))
                        {
                            Bindings[actionType].Add((Keys)keyID);
                        }
                    }
                }
            }
        }

        public void Update()
        {
            previousKeyboard = Keyboard.GetState();
        }

        public bool IsControlDown(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[control].Any(key => keyboard.IsKeyDown(key));
        }

        public bool IsControlUp(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[control].Any(key => keyboard.IsKeyUp(key));
        }

        public bool IsControlPressed(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[control].Any(
                key => keyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key));
        }

        public bool IsControlReleased(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[control].Any(
                key => keyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key));
        }
    }
}
