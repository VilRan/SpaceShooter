﻿using Microsoft.Xna.Framework.Input;
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
        Dictionary<Control, Keys[]> controls = new Dictionary<Control, Keys[]>();
        KeyboardState previousKeyboard;

        public Controller()
        {
            foreach (Control control in Enum.GetValues(typeof(Control)))
            {
                controls.Add(control, new Keys[2]);
            }
            controls[Control.MoveUp][0] = Keys.Up;
            controls[Control.MoveUp][1] = Keys.W;
            controls[Control.MoveDown][0] = Keys.Down;
            controls[Control.MoveDown][1] = Keys.S;
            controls[Control.MoveLeft][0] = Keys.Left;
            controls[Control.MoveLeft][1] = Keys.A;
            controls[Control.MoveRight][0] = Keys.Right;
            controls[Control.MoveRight][1] = Keys.D;
            controls[Control.Fire][0] = Keys.Space;
            controls[Control.PreviousWeapon][0] = Keys.Q;
            controls[Control.NextWeapon][0] = Keys.E;
            controls[Control.Weapon1][0] = Keys.D1;
            controls[Control.Weapon2][0] = Keys.D2;
            controls[Control.Weapon3][0] = Keys.D3;
            controls[Control.Weapon4][0] = Keys.D4;
            controls[Control.Weapon5][0] = Keys.D5;
            controls[Control.Weapon6][0] = Keys.D6;
            controls[Control.Weapon7][0] = Keys.D7;
            controls[Control.Weapon8][0] = Keys.D8;

            previousKeyboard = Keyboard.GetState();
        }

        public void Update()
        {
            previousKeyboard = Keyboard.GetState();
        }

        public bool IsControlDown(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return controls[control].Any(key => keyboard.IsKeyDown(key));
        }

        public bool IsControlUp(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return controls[control].Any(key => keyboard.IsKeyUp(key));
        }

        public bool IsControlPressed(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return controls[control].Any(
                key => keyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key));
        }

        public bool IsControlReleased(Control control)
        {
            KeyboardState keyboard = Keyboard.GetState();
            return controls[control].Any(
                key => keyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key));
        }
    }
}
