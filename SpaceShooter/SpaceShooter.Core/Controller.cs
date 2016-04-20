﻿using Microsoft.Xna.Framework.Input;
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
        Pause,
        MainMenu,
        Fullscreen,
        Editor,
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
    }
}