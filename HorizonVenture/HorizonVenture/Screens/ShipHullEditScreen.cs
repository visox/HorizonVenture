using HorizonVenture.HorizonVenture.Blocks;
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
    public class ShipHullEditScreen : AbstractScreen
    {
        private PlayerShip _playerShip;
        public PlayerShip PlayerShip
        {
            get { return _playerShip; }
            set
            {
                _playerShip = value;
                _editedShip = new BlocksHolder(_game);
                _editedShip.addBlocks(_playerShip.BlocksHolder.GetBlocks());
            }
        }
        private BlocksHolder _editedShip;
        public Vector2 _screenCenter;
        public float Scale { get; set; }

        private float _scrollDelay;
        private static readonly float SCROLL_DELAY = 100f;
        private Boolean _isScreenMove = false;

        public ShipHullEditScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;
            _screenCenter = new Vector2((game.GetScreenSize().X) / 2, game.GetScreenSize().Y / 2);
            Scale = 1;
            _scrollDelay = 0;
        }

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.H, hKeyPressed);
            PlayerShip = _game.PlayerShip;

            InputManager.OnMouseLeftKeyPress += mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress += mouseRightKeyPressed;
            InputManager.OnMouseScrollChange += mouseScrollChanged;
            InputManager.OnMouseLeftKeyRelease += _wasMouseLeftKeyReleased;
            InputManager.OnMousePositionChanged += mousePositionChanged;
        }

        private void mousePositionChanged(object sender, InputManager.MousePositionChangedArgs e)
        {
            if (_isScreenMove)
            {
                _screenCenter.X += e.ChangeX;
                _screenCenter.Y += e.ChangeY;
            }
        }

        private void _wasMouseLeftKeyReleased(object sender, InputManager.MouseKeyReleaseArgs e)
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

                    _toCenter.X = ((_game.GetScreenSize().X) / 2)
                        + ((_screenCenter.X - ((_game.GetScreenSize().X) / 2)) / 2);
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

                    if ((((PlayerShip.BlocksHolder.GetWidth() * (BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)) / 2) + _screenCenter.X) < 0)
                    {
                        _screenCenter.X = 0;
                    }
                    if ((-((PlayerShip.BlocksHolder.GetWidth() * (BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)) / 2) + _screenCenter.X) >
                        _game.GetScreenSize().X)
                    {
                        _screenCenter.X = _game.GetScreenSize().X;
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

        private void mouseRightKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            _cursor = null;
        }

        private void mouseLeftKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            if (IsLeftMouseKeyPressMoveScreen())
            {
                _isScreenMove = true;
            }
        }

        private bool IsLeftMouseKeyPressMoveScreen()
        {
            return true;
        }

        protected override void UnInit()
        {
            InputManager.RemoveKeyPressHandlers(Keys.H, hKeyPressed);
            InputManager.OnMouseLeftKeyPress -= mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress -= mouseRightKeyPressed;
            InputManager.OnMouseScrollChange -= mouseScrollChanged;
            InputManager.OnMouseLeftKeyRelease -= _wasMouseLeftKeyReleased;
            InputManager.OnMousePositionChanged -= mousePositionChanged;
        }

        private void hKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _game.ShowInSpaceScreen();
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
                PlayerShip.BlocksHolder.Draw(spriteBatch, _screenCenter,
                    PlayerShip.BlocksHolder.GetCenter(), 0, Color.White, Scale);

                _editedShip.Draw(spriteBatch, _screenCenter,
                    _editedShip.GetCenter(), 0, new Color(Color.Red,0.5f /255.0f), Scale);
            }
        }

        protected override void DrawCursorTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cursor, _cursorPosition, null, _cursorDrawColor, 0, _cursorCenter,
               Scale, SpriteEffects.None, 0);
        }
    }
}
