using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SpaceShooter
{
    public enum Action
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Fire,
        Reload,
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
        Pause,
        MainMenu,
        Fullscreen,
        Editor,
        Invalid,
    }

    public class Controller
    {
        public List<Keys>[] Bindings;
        KeyboardState previousKeyboard;

        public Controller(XmlElement xml)
        {
            int numberOfUniqueActions = Enum.GetValues(typeof(Action)).Length;
            Bindings = new List<Keys>[numberOfUniqueActions];
            for (int index = 0; index < numberOfUniqueActions; index++)
                Bindings[index] = new List<Keys>();

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
                            Bindings[(int)actionType].Add((Keys)keyID);
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
            return Bindings[(int)control].Any(key => keyboard.IsKeyDown(key));
        }

        public bool IsControlUp(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[(int)control].Any(key => keyboard.IsKeyUp(key));
        }

        public bool IsControlPressed(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[(int)control].Any(
                key => keyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key));
        }

        public bool IsControlReleased(Action control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return Bindings[(int)control].Any(
                key => keyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key));
        }

        public XmlElement ToXml(XmlDocument xml, string id)
        {
            XmlElement element = xml.CreateElement("Controller");
            XmlAttribute controllerID = xml.CreateAttribute("ID");
            controllerID.Value = id;
            element.Attributes.Append(controllerID);

            for (Action action = 0; action < Action.Invalid; action++)
            {
                if (Bindings[(int)action].Count == 0)
                    continue;

                XmlElement actionElement = xml.CreateElement("Action");
                XmlAttribute actionID = xml.CreateAttribute("ID");
                actionID.Value = action.ToString();
                actionElement.Attributes.Append(actionID);

                foreach (Keys key in Bindings[(int)action])
                {
                    XmlElement keyElement = xml.CreateElement("Key");
                    XmlAttribute keyID = xml.CreateAttribute("ID");
                    keyID.Value = "" + (int)key;
                    keyElement.Attributes.Append(keyID);

                    actionElement.AppendChild(keyElement);
                }

                element.AppendChild(actionElement);
            }

            return element;
        }
    }
}
