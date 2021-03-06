﻿using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Controls;
using HorizonVenture.HorizonVenture.EntityComponents;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Screens
{
    public class ShipComponentsEditScreen : AbstractScreen
    {
        public PlayerShip PlayerShip { get; set; }


        public Vector2 _screenCenter;
        public float Scale { get; set; }


        private RightShipComponentsPanel _rightShipComponentsPanel;

        private float _scrollDelay;
        private static readonly float SCROLL_DELAY = 100f;

        public ShipComponentsEditScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;
            
            _scrollDelay = 0;
        }


      //  private static readonly int BUTTON_WIDTH = 300;

        private static readonly int BUTTON_HEIGHT = 50;

        private static readonly int PLUS_MINUS_WIDTH = 50;
   //     private static readonly int PANEL_WIDTH = 300;

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.S, sKeyPressed);
            PlayerShip = _game.PlayerShip;

            InputManager.OnMouseLeftKeyPress += mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress += mouseRightKeyPressed;

            InputManager.OnMouseScrollChange += mouseScrollChanged;

            InputManager.OnMouseLeftKeyRelease += _mouseLeftKeyReleased;
            InputManager.OnMousePositionChanged += mousePositionChanged;

            _screenCenter = new Vector2((_game.GetScreenSize().X - RightShipComponentsPanel.DEFAULT_WIDTH) / 2, _game.GetScreenSize().Y / 2);
            Scale = 1;

            if (_controls.Count != 0)
                return;

          /*  Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            Button plusButton = new Button(background, "+", spriteFont,
                new Rectangle((int)(screenSize.X - RightShipComponentsPanel.DEFAULT_WIDTH - BUTTON_HEIGHT), BUTTON_HEIGHT, PLUS_MINUS_WIDTH, BUTTON_HEIGHT));

            plusButton.DrawBackgroundColor = Color.Aqua;
            plusButton.Click += plusButton_Click;

            _controls.Add(plusButton);

            Button minusButton = new Button(background, "-", spriteFont,
                new Rectangle((int)(screenSize.X - RightShipComponentsPanel.DEFAULT_WIDTH - BUTTON_HEIGHT), BUTTON_HEIGHT * 3, PLUS_MINUS_WIDTH, BUTTON_HEIGHT));

            minusButton.DrawBackgroundColor = Color.Aqua;
            minusButton.Click += minusButton_Click;

            _controls.Add(minusButton);*/



            AddRightShipComponentsPanel();
        }

        private void mousePositionChanged(object sender, InputManager.MousePositionChangedArgs e)
        {
            if (_isScreenMove)
            {
                _screenCenter.X += e.ChangeX;
                _screenCenter.Y += e.ChangeY;
            }
        }

        private void _mouseLeftKeyReleased(object sender, InputManager.MouseKeyReleaseArgs e)
        {
            if (_isScreenMove)
            {
                _isScreenMove = false;
            }
        }

        private Vector2 _toCenter = new Vector2(0, 0);

        private void mouseScrollChanged(object sender, InputManager.MouseScrollChangeArgs e)
        {
            if (_scrollDelay > 0)
                return;

            _scrollDelay = SCROLL_DELAY;

            if (e.Change < 0)
            {
                if (Scale > 1.0f / 8.0f)
                {
                    Scale /= 2f;

                    _toCenter.X = ((_game.GetScreenSize().X - RightShipComponentsPanel.DEFAULT_WIDTH) / 2)
                        + ((_screenCenter.X - ((_game.GetScreenSize().X - RightShipComponentsPanel.DEFAULT_WIDTH) / 2)) / 2);
                    _toCenter.Y = (_game.GetScreenSize().Y / 2)
                        + ((_screenCenter.Y - (_game.GetScreenSize().Y / 2)) / 2);

                    _screenCenter.X -= _screenCenter.X - _toCenter.X;
                    _screenCenter.Y -= _screenCenter.Y - _toCenter.Y;
                }
            }
            else
            {
                if (Scale < 1)
                {
                    Scale *= 2f;

                    _screenCenter.X += _screenCenter.X - _cursorPosition.X;
                    _screenCenter.Y += _screenCenter.Y - _cursorPosition.Y;

                    if((((PlayerShip.BlocksHolder.GetWidth() * (BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)) / 2) + _screenCenter.X) < 0)
                    {
                        _screenCenter.X = 0;
                    }
                    if ((-((PlayerShip.BlocksHolder.GetWidth() * (BlocksHolder.SCALE_1_BLOCK_SIZE * Scale))  / 2) + _screenCenter.X) >
                        _game.GetScreenSize().X - RightShipComponentsPanel.DEFAULT_WIDTH)
                    {
                        _screenCenter.X = _game.GetScreenSize().X - RightShipComponentsPanel.DEFAULT_WIDTH;
                    }
                    //////
                    if ((((PlayerShip.BlocksHolder.GetHeight() * (BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)) / 2) + _screenCenter.Y) < 0)
                    {
                        _screenCenter.Y = 0;
                    }
                    if ((-((PlayerShip.BlocksHolder.GetHeight() * (BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)) / 2) + _screenCenter.Y) >
                        _game.GetScreenSize().Y)
                    {
                        _screenCenter.Y = _game.GetScreenSize().Y;
                    }
                }
            }
        }

        

        private Boolean IsLeftMouseKeyPressAddComponent()
        {
            if (_selectedComponent == -1)
                return false;

            if (_rightShipComponentsPanel.IsPointOverControl(new Point(InputManager.MouseState.X, InputManager.MouseState.Y)))
                return false;

            return true;

        }

        private Boolean IsLeftMouseKeyPressMoveScreen()
        { 
            if(_selectedComponent != -1)
                return false;

            if (_rightShipComponentsPanel.IsPointOverControl(new Point(InputManager.MouseState.X, InputManager.MouseState.Y)))
                return false;

            return true;
        }

        private Boolean _isScreenMove = false;

        private void mouseLeftKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {

            if (IsLeftMouseKeyPressAddComponent())
            {
                Vector2 toAddPosition = GetCursorPositionOnShip(_screenCenter, Scale);

                AbstractEntityComponent aec = PlayerShip.OwnedComponents[_selectedComponent];

                aec.PositionOnEntity = toAddPosition;

                PlayerShip.EntityComponents.Add(aec);
                aec.OnShipInit();
                PlayerShip.OwnedComponents.Remove(aec);

                AddRightShipComponentsPanel();

                _selectedComponent = -1;
                _cursor = null;
            }

            if (IsLeftMouseKeyPressMoveScreen())
            {
                _isScreenMove = true;
            }
        }

        private void mouseRightKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            _selectedComponent = -1;
            _cursor = null;
        }

        

        private void AddRightShipComponentsPanel()
        {
            if (_controls.Contains(_rightShipComponentsPanel))
            {
                _controls.Remove(_rightShipComponentsPanel);
            }

            _rightShipComponentsPanel = new RightShipComponentsPanel(_game);
            _rightShipComponentsPanel.Click += addComponentButton_Click;
            _controls.Add(_rightShipComponentsPanel);
        }

        private void addComponentButton_Click(object sender, RightShipComponentsPanel.ComponentClickArgs e)
        {
            _cursor = PlayerShip.OwnedComponents[_rightShipComponentsPanel.SelectedComponent].GetImage();
            _cursorCenter.X = _cursor.Width / 2;
            _cursorCenter.Y = _cursor.Height / 2;

            _selectedComponent = _rightShipComponentsPanel.SelectedComponent;
        }


        protected override void UnInit()
        {
            base.UnInit();

            InputManager.RemoveKeyPressHandlers(Keys.S, sKeyPressed);

            InputManager.OnMouseLeftKeyPress -= mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress -= mouseRightKeyPressed;

            InputManager.OnMouseScrollChange -= mouseScrollChanged;

            InputManager.OnMouseLeftKeyRelease -= _mouseLeftKeyReleased;
            InputManager.OnMousePositionChanged -= mousePositionChanged;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

         //   UpdateShip(gameTime);

            UpdateScrollDelay(gameTime);
        }

        private void UpdateScrollDelay(GameTime gameTime)
        {
            if (_scrollDelay > 0)
                _scrollDelay = Math.Max(0, _scrollDelay - gameTime.ElapsedGameTime.Milliseconds);
        }

        private void UpdateShip(GameTime gameTime)
        {
            PlayerShip.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackgroundColor();

            DrawShip(spriteBatch);

            DrawControls(spriteBatch);

            DrawCursor(spriteBatch);
        }



        private void DrawShip(SpriteBatch spriteBatch)
        {
            if (PlayerShip != null)
            {
                PlayerShip.DetailShipDraw(spriteBatch, _screenCenter, Scale);
            }
        }

        void sKeyPressed(object sender, InputManager.KeyPressArgs e)
        {

            _game.ShowInSpaceScreen();
        }

      /*  void plusButton_Click(object sender, Button.ButtonclickArgs e)
        {


            if (Scale < 1)
                Scale *= 2f;
        }

        void minusButton_Click(object sender, Button.ButtonclickArgs e)
        {

            if (Scale > 1.0f/8.0f)
                Scale /= 2f;
        }
        */

        protected override void DrawCursorTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cursor, _cursorPosition, null, _cursorDrawColor, 0, _cursorCenter,
               Scale, SpriteEffects.None, 0);
        }


        private int _selectedComponent = -1;

        void addComponentButton_Click(object sender, Button.ButtonclickArgs e)
        {

            _cursor = PlayerShip.OwnedComponents[int.Parse(e.Tag)].GetImage();
            _cursorCenter.X = _cursor.Width / 2;
            _cursorCenter.Y = _cursor.Height / 2;

            _selectedComponent = int.Parse(e.Tag);
        }
    }
}
