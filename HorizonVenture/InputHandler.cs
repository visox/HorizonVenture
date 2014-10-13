using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture
{
    public static class InputManager
    {
        public static KeyboardState KeyboardState { get; private set; }
        public static MouseState MouseState { get; private set; }

        public static void UpdateInput()
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
        }


    }
}
