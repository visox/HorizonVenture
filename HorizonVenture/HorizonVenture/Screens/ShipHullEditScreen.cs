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
                if (_playerShip == null || !ReferenceEquals(_playerShip, value))
                {
                    _playerShip = value;
                    _editedShip = new Dictionary<Vector2, AbstractBlock>();
                    _editedShip = Helper.CloneDictionaryCloningValues(PlayerShip.BlocksHolder.GetBlocks());
                    _removedBlocksShip = new List<Vector2>();
                    _addedBlocksShip = new List<Vector2>();
                }
            }
        }
        private Dictionary<Vector2, AbstractBlock> _editedShip;
        private List<Vector2> _removedBlocksShip;
        private List<Vector2> _addedBlocksShip;
        public Vector2 _screenCenter;
        public float Scale { get; set; }

        private float _scrollDelay;
        private static readonly float SCROLL_DELAY = 100f;
        private Boolean _isScreenMove = false;

        private float _leftMousePressReleaseTime;

        private static readonly float PRESS_RELEASE_MOUSE_CHANGE_HULL = 200;

        public ShipHullEditScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Green;
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
            InputManager.OnMouseLeftKeyRelease += _mouseLeftKeyReleased;
            InputManager.OnMousePositionChanged += mousePositionChanged;

            _editedShip = new Dictionary<Vector2, AbstractBlock>();
            _editedShip = Helper.CloneDictionaryCloningValues(PlayerShip.BlocksHolder.GetBlocks());
            _removedBlocksShip = new List<Vector2>();
            _addedBlocksShip = new List<Vector2>();
        }

        private void mousePositionChanged(object sender, InputManager.MousePositionChangedArgs e)
        {
            if (_isScreenMove)
            {
                _screenCenter.X += e.ChangeX;
                _screenCenter.Y += e.ChangeY;
            }
        }

        protected override Vector2 GetCursorPositionOnShip(Vector2 offset, float scale)
        {
            _cursorPositionOnShip.X = InputManager.MouseState.X - offset.X +
                (((PlayerShip.BlocksHolder.GetWidth() / 2) - _topLeftOffset.X) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale)/* + (Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE / 2)*/;
            _cursorPositionOnShip.Y = InputManager.MouseState.Y - offset.Y +
                (((PlayerShip.BlocksHolder.GetHeight() / 2) - _topLeftOffset.Y) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale)/* + (Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE / 2)*/;

            _cursorPositionOnShip.X /= Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale;
            _cursorPositionOnShip.Y /= Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale;

            _cursorPositionOnShip.X = (float)Math.Round(_cursorPositionOnShip.X);
            _cursorPositionOnShip.Y = (float)Math.Round(_cursorPositionOnShip.Y);

            return _cursorPositionOnShip;
        }

        private void _mouseLeftKeyReleased(object sender, InputManager.MouseKeyReleaseArgs e)
        {
            if (InTimeToAddBlock())
            {
                Vector2 toAddPosition = GetCursorPositionOnShip(_screenCenter, Scale);

                if (PlayerShip.BlocksHolder.GetMinX() + toAddPosition.X <= 100
                    && PlayerShip.BlocksHolder.GetMaxX() - 100 <= toAddPosition.X
                    && PlayerShip.BlocksHolder.GetMinY() + toAddPosition.Y <= 100
                    && PlayerShip.BlocksHolder.GetMaxY() - 100 <= toAddPosition.Y)
                {
                    AbstractBlock block = GetBlockByPosition(toAddPosition);

                    if (block == null)
                    {
                        AbstractBlock toAdd = new Block("metal1");
                        _editedShip[toAddPosition] = toAdd;
                        _addedBlocksShip.Add(toAddPosition);
                    }
                    else if (_removedBlocksShip.Contains(toAddPosition))
                    {
                        AbstractBlock blockShip = PlayerShip.BlocksHolder.GetBlockByPosition(toAddPosition);

                        if (blockShip != null)
                        {
                            _removedBlocksShip.Remove(toAddPosition);
                        }
                    }
                }

            }

            if (_isScreenMove)
            {
                _isScreenMove = false;
            }
        }

        private bool InTimeToAddBlock()
        {
            return _leftMousePressReleaseTime <= PRESS_RELEASE_MOUSE_CHANGE_HULL;
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

        public AbstractBlock GetBlockByPosition(Vector2 position)
        {
            List<Vector2> key = _editedShip.Keys.Where(k => position.X == k.X && position.Y == k.Y).ToList();
            if (key.Count() == 0)
                return null;

            return _editedShip[key.ElementAt(0)];
        }

        private void mouseRightKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            _cursor = null;

            Vector2 toRemovePosition = GetCursorPositionOnShip(_screenCenter, Scale);

            AbstractBlock block = GetBlockByPosition(toRemovePosition);

            if (block != null)
            {
                if (_editedShip.ContainsKey(toRemovePosition))
                {

                    AbstractBlock blockShip = PlayerShip.BlocksHolder.GetBlockByPosition(toRemovePosition);

                    if (!_removedBlocksShip.Contains(toRemovePosition) &&
                        blockShip != null)
                    {
                        _removedBlocksShip.Add(toRemovePosition);
                    }

                    if (blockShip == null)
                    {
                        _addedBlocksShip.Remove(toRemovePosition);
                        _removedBlocksShip.Remove(toRemovePosition);
                        _editedShip.Remove(toRemovePosition);
                    }
                }
            }

        }



        private void mouseLeftKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            _leftMousePressReleaseTime = 0;

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
            InputManager.OnMouseLeftKeyRelease -= _mouseLeftKeyReleased;
            InputManager.OnMousePositionChanged -= mousePositionChanged;
        }

        private void hKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            // Dictionary<Vector2, AbstractBlock> toAdd = new Dictionary<Vector2, AbstractBlock>();



            foreach (Vector2 key in _removedBlocksShip)
            {
                _editedShip.Remove(key);
            }
            PlayerShip.BlocksHolder.ClearBlocks();
            PlayerShip.BlocksHolder.addBlocks(_editedShip);

            _game.ShowInSpaceScreen();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //   UpdateShip(gameTime);

            UpdateScrollDelay(gameTime);

            UpdateLeftMouseButtonPressTime(gameTime);
        }

        private void UpdateLeftMouseButtonPressTime(GameTime gameTime)
        {
            if (InputManager.MouseState.LeftButton == ButtonState.Pressed)
            {
                _leftMousePressReleaseTime += gameTime.ElapsedGameTime.Milliseconds;
            }
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

        Vector2 _topLeftOffset = new Vector2();

        private void DrawShip(SpriteBatch spriteBatch)
        {
            _topLeftOffset.X = PlayerShip.BlocksHolder.GetMinX();
            _topLeftOffset.Y = PlayerShip.BlocksHolder.GetMinY();

            if (PlayerShip != null)
            {
                // PlayerShip.BlocksHolder.Draw(spriteBatch, _screenCenter,
                //    PlayerShip.BlocksHolder.GetCenter(), 0, Color.White, Scale);

                foreach (Vector2 key in _editedShip.Keys)
                {
                    AbstractBlock ab = _editedShip[key];

                    Color drawColor = Color.White;

                    if (_removedBlocksShip.Contains(key))
                    {
                        drawColor = new Color(Color.Red, 0.1f / 255.0f);
                    }

                    if (_addedBlocksShip.Contains(key))
                    {
                        drawColor = new Color(Color.Green, 0.5f / 255.0f);
                    }


                    spriteBatch.Draw(ab.getTexture(_game), new Vector2(_screenCenter.X + ((key.X - _topLeftOffset.X) * BlocksHolder.SCALE_1_BLOCK_SIZE * Scale),
                            _screenCenter.Y + ((key.Y - _topLeftOffset.Y) * BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)), null, drawColor, 0,
                            PlayerShip.BlocksHolder.GetCenter(),
                        Scale, SpriteEffects.None, 0);
                }
            }
        }

        protected override void DrawCursorTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cursor, _cursorPosition, null, _cursorDrawColor, 0, _cursorCenter,
               Scale, SpriteEffects.None, 0);
        }
    }
}
