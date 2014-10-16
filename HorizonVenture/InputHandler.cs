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

            CheckKeyPressEvents();
        }

        private static void CheckKeyPressEvents()
        {
            foreach (Keys k in (Keys[])Enum.GetValues(typeof(Keys)))
            {
                if (_keyPressHandlers.ContainsKey(k))
                {
                    if (KeyboardState.IsKeyDown(k))
                        if (_keyPressHandlers[k] != null)
                            _keyPressHandlers[k](null, new KeyPressArgs(k));
                }
            }
        }

        public static void AddKeyPressHandlers(Keys key, KeyPressHandler handler)
        {
            if (!_keyPressHandlers.ContainsKey(key))
            {
                _keyPressHandlers.Add(key, null);
            }

            _keyPressHandlers[key] = (KeyPressHandler)_keyPressHandlers[key] + handler;
        
        }

        public static void RemoveKeyPressHandlers(Keys key, KeyPressHandler handler)
        {
            if (!_keyPressHandlers.ContainsKey(key))
            {
                return;
            }

            _keyPressHandlers[key] = (KeyPressHandler)_keyPressHandlers[key] - handler;

        }

        private static Dictionary<Keys, KeyPressHandler> _keyPressHandlers =
            new Dictionary<Keys, KeyPressHandler>();

        public delegate void KeyPressHandler(object sender, KeyPressArgs e);
        
        public class KeyPressArgs : EventArgs
        {
            public Keys Key { get; set; }

            public KeyPressArgs(Keys key)
            {
                Key = key;
            }
        }
    }
}
