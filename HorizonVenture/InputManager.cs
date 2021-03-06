﻿using Microsoft.Xna.Framework;
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
            CheckMouseKeysPressEvents();
            CheckMouseScrollChangeEvents();
        }

        private static Dictionary<Keys, bool> _wasKeyDown = new Dictionary<Keys, bool>();

        private static void CheckKeyPressEvents()
        {
            foreach (Keys k in (Keys[])Enum.GetValues(typeof(Keys)))
            {
                if (_keyPressHandlers.ContainsKey(k))
                {
                    if (!_wasKeyDown.ContainsKey(k))
                    {
                        _wasKeyDown.Add(k, false);
                    }

                    if (KeyboardState.IsKeyDown(k) && !_wasKeyDown[k])
                    {
                        if (_keyPressHandlers[k] != null)
                            _keyPressHandlers[k](null, new KeyPressArgs(k));

                        _wasKeyDown[k] = true;
                    }
                    if (KeyboardState.IsKeyUp(k)) 
                    {
                        _wasKeyDown[k] = false;
                    }
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

        private static bool _wasMouseLeftKeyPressed = false;
        private static bool _wasMouseRightKeyPressed = false;

        private static Vector2 _lastMousePosition = new Vector2(0, 0);

        private static void CheckMouseKeysPressEvents()
        {
            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                if (!_wasMouseLeftKeyPressed)
                {
                    if (OnMouseLeftKeyPress != null)
                        OnMouseLeftKeyPress(null, new MouseKeyPressArgs());
                }

                _wasMouseLeftKeyPressed = true;
            }
            if (MouseState.LeftButton == ButtonState.Released)
            {
 
                if (_wasMouseLeftKeyPressed)
                {
                    if (OnMouseLeftKeyRelease != null)
                        OnMouseLeftKeyRelease(null, new MouseKeyReleaseArgs());
                }

                _wasMouseLeftKeyPressed = false;
            }

            if (MouseState.RightButton == ButtonState.Pressed && !_wasMouseRightKeyPressed)
            {
                _wasMouseRightKeyPressed = true;
                if (OnMouseRightKeyPress != null)
                    OnMouseRightKeyPress(null, new MouseKeyPressArgs());
            }
            if (MouseState.RightButton == ButtonState.Released)
            {
                _wasMouseRightKeyPressed = false;
            }

            ////////////////
            if (_lastMousePosition.X != MouseState.X
                || _lastMousePosition.Y != MouseState.Y)
            {
                if (OnMousePositionChanged != null)
                {
                    OnMousePositionChanged(null, new MousePositionChangedArgs(MouseState.X - _lastMousePosition.X,
                        MouseState.Y - _lastMousePosition.Y));
                }

                _lastMousePosition.X = MouseState.X;
                _lastMousePosition.Y = MouseState.Y;
            }
        }

        public static MouseKeyPressHandler OnMouseLeftKeyPress;
        public static MouseKeyPressHandler OnMouseRightKeyPress;

        public delegate void MouseKeyPressHandler(object sender, MouseKeyPressArgs e);

        public class MouseKeyPressArgs : EventArgs
        {
            public MouseKeyPressArgs()
            {
            }
        }

        public static MouseKeyReleaseHandler OnMouseLeftKeyRelease;

        public delegate void MouseKeyReleaseHandler(object sender, MouseKeyReleaseArgs e);

        public class MouseKeyReleaseArgs : EventArgs
        {
            public MouseKeyReleaseArgs()
            {
            }
        }

        public static MousePositionChangedHandler OnMousePositionChanged;

        public delegate void MousePositionChangedHandler(object sender, MousePositionChangedArgs e);

        public class MousePositionChangedArgs : EventArgs
        {
            public float ChangeX { get; private set; }
            public float ChangeY { get; private set; }

            public MousePositionChangedArgs(float changeX, float changeY)
            {
                ChangeX = changeX;
                ChangeY = changeY;
            }
        }


        private static int _lastScrollWheelValue = 0;

        public static int MINIMAL_SCROLL_WHELL_CHANGE = 120;

        private static void CheckMouseScrollChangeEvents()
        {
            if (_lastScrollWheelValue != MouseState.ScrollWheelValue)
            {
                
                if (OnMouseScrollChange != null)
                    OnMouseScrollChange(null, new MouseScrollChangeArgs(MouseState.ScrollWheelValue - _lastScrollWheelValue));

                _lastScrollWheelValue = MouseState.ScrollWheelValue;
            }
        }

        public static MouseScrollChangeHandler OnMouseScrollChange;

        public delegate void MouseScrollChangeHandler(object sender, MouseScrollChangeArgs e);

        public class MouseScrollChangeArgs : EventArgs
        {
            public int Change{get; private set;}
 
            public MouseScrollChangeArgs(int change)
            {
                Change = change;
            }
        }


    }
}
