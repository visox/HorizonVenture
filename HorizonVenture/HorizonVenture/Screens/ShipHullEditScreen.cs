using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Controls;
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

        private RightShipMaterialsPanel _rightShipMaterialsPanel;
        private float _scrollDelay;
        private static readonly float SCROLL_DELAY = 100f;

        private Texture2D pixel;

        public ShipHullEditScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;

            _scrollDelay = 0;

            pixel = new Texture2D(_game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        protected override void Init()
        {
            _screenCenter = new Vector2((_game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH) / 2, _game.GetScreenSize().Y / 2);
            Scale = 1;

            InputManager.AddKeyPressHandlers(Keys.H, hKeyPressed);

            InputManager.AddKeyPressHandlers(Keys.Up, UpKeyPressed);
            InputManager.AddKeyPressHandlers(Keys.Down, DownKeyPressed);
            InputManager.AddKeyPressHandlers(Keys.Left, LeftKeyPressed);
            InputManager.AddKeyPressHandlers(Keys.Right, RightKeyPressed);
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

            AddRightShipMaterialsPanel();
        }

        private static readonly int ARROW_POSITION_CHANGE = 10;

        private void UpKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _screenCenter.Y += ARROW_POSITION_CHANGE * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE;
        }

        private void DownKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _screenCenter.Y -= ARROW_POSITION_CHANGE * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE;
        }

        private void LeftKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _screenCenter.X += ARROW_POSITION_CHANGE * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE;
        }

        private void RightKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _screenCenter.X -= ARROW_POSITION_CHANGE * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE;
        }

        private void AddRightShipMaterialsPanel()
        {
            if (_controls.Contains(_rightShipMaterialsPanel))
            {
                _controls.Remove(_rightShipMaterialsPanel);
            }

            _rightShipMaterialsPanel = new RightShipMaterialsPanel(_game);
            _controls.Add(_rightShipMaterialsPanel);
        }


        private static readonly float MAX_DIST_TO_CENTER = 50;

        private void TryAddBlockByMouse()
        {
            string blockType = _rightShipMaterialsPanel.SelectedMaterial;

            if (String.IsNullOrEmpty(blockType))
                return;

            if (InputManager.MouseState.X >= _game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH)
                return;

            Vector2 toAddPosition = GetCursorPositionOnShip(_screenCenter, Scale);

            if (Math.Abs(PlayerShip.BlocksHolder.GetBlockCenter().X - toAddPosition.X + (_topLeftOffset.X)) <= MAX_DIST_TO_CENTER
                && Math.Abs(PlayerShip.BlocksHolder.GetBlockCenter().Y - toAddPosition.Y + (_topLeftOffset.Y)) <= MAX_DIST_TO_CENTER)
            {
                AbstractBlock block = GetBlockByPosition(toAddPosition);

                if (block == null)
                {
                    AbstractBlock toAdd = new Block(blockType);
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

        private void TryRemoveBlockByMouse()
        {
            if (InputManager.MouseState.X >= _game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH)
                return;


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

        private void mousePositionChanged(object sender, InputManager.MousePositionChangedArgs e)
        {
            if (InputManager.MouseState.LeftButton == ButtonState.Pressed)
            {
                TryAddBlockByMouse();
            }
            else if (InputManager.MouseState.RightButton == ButtonState.Pressed)
            {
                TryRemoveBlockByMouse();
            }
        }

        protected override Vector2 GetCursorPositionOnShip(Vector2 offset, float scale)
        {
            _cursorPositionOnShip.X = InputManager.MouseState.X - offset.X +
                (((PlayerShip.BlocksHolder.GetWidth() / 2) + (_topLeftOffset.X)) *
                Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale);
            _cursorPositionOnShip.Y = InputManager.MouseState.Y - offset.Y +
                (((PlayerShip.BlocksHolder.GetHeight() / 2) + (_topLeftOffset.Y)) *
                Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale);

            _cursorPositionOnShip.X /= Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale;
            _cursorPositionOnShip.Y /= Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * scale;

            _cursorPositionOnShip.X = (float)Math.Round(_cursorPositionOnShip.X);
            _cursorPositionOnShip.Y = (float)Math.Round(_cursorPositionOnShip.Y);

            return _cursorPositionOnShip;
        }

        private void _mouseLeftKeyReleased(object sender, InputManager.MouseKeyReleaseArgs e)
        {

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

                    _toCenter.X = ((_game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH) / 2)
                        + ((_screenCenter.X - ((_game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH) / 2)) / 2);
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
                        _game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH)
                    {
                        _screenCenter.X = _game.GetScreenSize().X - RightShipMaterialsPanel.DEFAULT_WIDTH;
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
            TryRemoveBlockByMouse();
        }

        private void mouseLeftKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            TryAddBlockByMouse();
        }

        protected override void UnInit()
        {
            InputManager.RemoveKeyPressHandlers(Keys.H, hKeyPressed);

            InputManager.RemoveKeyPressHandlers(Keys.Up, UpKeyPressed);
            InputManager.RemoveKeyPressHandlers(Keys.Down, DownKeyPressed);
            InputManager.RemoveKeyPressHandlers(Keys.Left, LeftKeyPressed);
            InputManager.RemoveKeyPressHandlers(Keys.Right, RightKeyPressed);

            InputManager.OnMouseLeftKeyPress -= mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress -= mouseRightKeyPressed;
            InputManager.OnMouseScrollChange -= mouseScrollChanged;
            InputManager.OnMouseLeftKeyRelease -= _mouseLeftKeyReleased;
            InputManager.OnMousePositionChanged -= mousePositionChanged;
        }

        private void hKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            // Dictionary<Vector2, AbstractBlock> toAdd = new Dictionary<Vector2, AbstractBlock>();

            float oldMinX = PlayerShip.BlocksHolder.GetMinX();
            float oldMinY = PlayerShip.BlocksHolder.GetMinY();

            float oldMaxX = PlayerShip.BlocksHolder.GetMaxX();
            float oldMaxY = PlayerShip.BlocksHolder.GetMaxY();

            foreach (Vector2 key in _removedBlocksShip)
            {
                _editedShip.Remove(key);
            }
            PlayerShip.BlocksHolder.ClearBlocks();
            PlayerShip.BlocksHolder.addBlocks(_editedShip);

            float newMinX = PlayerShip.BlocksHolder.GetMinX();
            float newMinY = PlayerShip.BlocksHolder.GetMinY();

            float newMaxX = PlayerShip.BlocksHolder.GetMaxX();
            float newMaxY = PlayerShip.BlocksHolder.GetMaxY();

            if (newMinX != oldMinX || newMinY != oldMinY
                || newMaxX != oldMaxX || newMaxY != oldMaxY)
            {
                PlayerShip.EntityComponents.ForEach(ec => ec.UpdatePositionOnShipByCenterChange(
                     ((newMinX - oldMinX) / 2) + ((newMaxX - oldMaxX) / 2),
                     ((newMinY - oldMinY) / 2) + ((newMaxY - oldMaxY) / 2)));
            }

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

            DrawBuildRectangle(spriteBatch);
        }

        private static int RECTANGLE_THICKNESS = 1;
        private static Color RECTANGLE_COLOR = Color.White;

        private void DrawBuildRectangle(SpriteBatch spriteBatch)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle((int)(_screenCenter.X  -
                ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                (int)(_screenCenter.Y - ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                (int)(2 * ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)), RECTANGLE_THICKNESS), RECTANGLE_COLOR);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle((int)(_screenCenter.X -
                ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                (int)(_screenCenter.Y - ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                RECTANGLE_THICKNESS, (int)(2 * ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale))), RECTANGLE_COLOR);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((int)(_screenCenter.X +
                ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                (int)(_screenCenter.Y - ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                RECTANGLE_THICKNESS,
                (int)(2 * ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale))), RECTANGLE_COLOR);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle((int)(_screenCenter.X -
                ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                (int)(_screenCenter.Y + ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                (int)(2 * ((MAX_DIST_TO_CENTER + RECTANGLE_THICKNESS) * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale)),
                                            RECTANGLE_THICKNESS), RECTANGLE_COLOR);
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
