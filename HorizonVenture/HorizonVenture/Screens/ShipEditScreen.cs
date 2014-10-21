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
    class ShipEditScreen : AbstractScreen
    {
        public PlayerShip PlayerShip { get; set; }
        public Vector2 ScreenCenter { get; set; }
        public float Scale { get; set; }
        public int ComponentsPanelIndex { get; set; }

        private Panel _rightShipComponentsPanel;

        public ShipEditScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;
            ScreenCenter = new Vector2((game.GetScreenSize().X - PANEL_WIDTH )/ 2, game.GetScreenSize().Y / 2);
            Scale = 1;
            ComponentsPanelIndex = 0;            
        }


      //  private static readonly int BUTTON_WIDTH = 300;

        private static readonly int BUTTON_HEIGHT = 50;

        private static readonly int PLUS_MINUS_WIDTH = 50;
        private static readonly int PANEL_WIDTH = 300;

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.S, sKeyPressed);
            PlayerShip = _game.PlayerShip;

            InputManager.OnMouseLeftKeyPress += mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress += mouseRightKeyPressed;

            InputManager.OnMouseScrollChange += mouseScrollChanged;

            if (_controls.Count != 0)
                return;

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            Button plusButton = new Button(background, "+", spriteFont,
                new Rectangle((int)(screenSize.X - PANEL_WIDTH - BUTTON_HEIGHT), BUTTON_HEIGHT, PLUS_MINUS_WIDTH, BUTTON_HEIGHT));

            plusButton.DrawBackgroundColor = Color.Aqua;
            plusButton.Click += plusButton_Click;

            _controls.Add(plusButton);

            Button minusButton = new Button(background, "-", spriteFont,
                new Rectangle((int)(screenSize.X - PANEL_WIDTH - BUTTON_HEIGHT), BUTTON_HEIGHT*3, PLUS_MINUS_WIDTH, BUTTON_HEIGHT));

            minusButton.DrawBackgroundColor = Color.Aqua;
            minusButton.Click += minusButton_Click;

            _controls.Add(minusButton);



            AddRightShipComponentsPanel();
        }

        private void mouseScrollChanged(object sender, InputManager.MouseScrollChangeArgs e)
        {
            if (e.Change > 0)
            {
                if (Scale > 1.0f / 8.0f)
                    Scale /= 2f;
            }
            else
            {
                if (Scale < 1)
                    Scale *= 2f;
            }
        }

        private Vector2 _cursorPositionOnShip = new Vector2(0, 0);

        private Vector2 GetCursorPositionOnShip()
        {
            _cursorPositionOnShip.X = InputManager.MouseState.X - ScreenCenter.X;
            _cursorPositionOnShip.Y = InputManager.MouseState.Y - ScreenCenter.Y;

            _cursorPositionOnShip.X /= Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale;
            _cursorPositionOnShip.Y /= Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE * Scale;

            _cursorPositionOnShip.X = (float)Math.Round(_cursorPositionOnShip.X);
            _cursorPositionOnShip.Y = (float)Math.Round(_cursorPositionOnShip.Y);

            return _cursorPositionOnShip;
        }



        private void mouseLeftKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            if (_selectedComponent == -1)
                return;

            if (_rightShipComponentsPanel.IsPointOverControl(new Point(InputManager.MouseState.X, InputManager.MouseState.Y)))
                return;

            Vector2 toAddPosition = GetCursorPositionOnShip();
          //  Vector2 toAddPosition = new Vector2(toAddPositionTmp.X, toAddPositionTmp.Y);

            AbstractEntityComponent aec = PlayerShip.OwnedComponents[_selectedComponent];

            aec.PositionOnEntity = toAddPosition;

            PlayerShip.EntityComponents.Add(aec);
            PlayerShip.OwnedComponents.Remove(aec);

            AddRightShipComponentsPanel();

            _selectedComponent = -1;
            _cursor = null;
        }

        private void mouseRightKeyPressed(object sender, InputManager.MouseKeyPressArgs e)
        {
            _selectedComponent = -1;
            _cursor = null;
        }

        private static readonly int COMPONENTS_SHOW_COUNT = 5;

        private void AddRightShipComponentsPanel()
        {
            if (_controls.Contains(_rightShipComponentsPanel))
            {
                _controls.Remove(_rightShipComponentsPanel);
            }

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background0");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            _rightShipComponentsPanel = new Panel(background,
                           new Rectangle((int)(screenSize.X - PANEL_WIDTH), 0, PANEL_WIDTH, (int)(screenSize.Y)));

            _rightShipComponentsPanel.DrawBackgroundColor = new Color(255, 255, 255, 128);

            _controls.Add(_rightShipComponentsPanel);


            AddComponentsToRightPanel();
        }

        private static readonly float COMPONENTS_IMAGEBUTTON_SIZE = 100;
        private static readonly float COMPONENTS_IMAGEBUTTON_MARGIN = 20;

        private void AddComponentsToRightPanel()
        {
            _rightShipComponentsPanel.Controls.Clear();

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            for (int i = ComponentsPanelIndex; i < COMPONENTS_SHOW_COUNT + ComponentsPanelIndex; i++)
            {
                if (i >= PlayerShip.OwnedComponents.Count)
                    break;

                Texture2D componentImage = PlayerShip.OwnedComponents[i].GetImage();

                ImageButton addComponentButton = new ImageButton(background, componentImage,
                    new Rectangle(
                        (int)(_rightShipComponentsPanel.Position.Right - COMPONENTS_IMAGEBUTTON_MARGIN - COMPONENTS_IMAGEBUTTON_SIZE),
                        (int)(_rightShipComponentsPanel.Position.Top + COMPONENTS_IMAGEBUTTON_MARGIN +
                            ((COMPONENTS_IMAGEBUTTON_SIZE + COMPONENTS_IMAGEBUTTON_MARGIN) * (i - ComponentsPanelIndex))),
                        (int)COMPONENTS_IMAGEBUTTON_SIZE, 
                        (int)COMPONENTS_IMAGEBUTTON_SIZE));

                addComponentButton.DrawBackgroundColor = new Color(50, 255, 50, 128);

                addComponentButton.Tag = i.ToString();

                float maxSize = Math.Max(componentImage.Height, componentImage.Width);
                float curScale = 1;

                while (maxSize * curScale > COMPONENTS_IMAGEBUTTON_SIZE)
                {
                    curScale *= 0.5f;
                }
                addComponentButton.Scale = curScale;

                addComponentButton.Click += addComponentButton_Click;

                _controls.Add(addComponentButton);
            }
        }

        protected override void UnInit()
        {
            base.UnInit();

            InputManager.RemoveKeyPressHandlers(Keys.S, sKeyPressed);

            InputManager.OnMouseLeftKeyPress -= mouseLeftKeyPressed;
            InputManager.OnMouseRightKeyPress -= mouseRightKeyPressed;

            InputManager.OnMouseScrollChange -= mouseScrollChanged;
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
                PlayerShip.DetailShipDraw(spriteBatch, ScreenCenter, Scale);
            }
        }

        void sKeyPressed(object sender, InputManager.KeyPressArgs e)
        {

            _game.ShowInSpaceScreen();
        }

        void plusButton_Click(object sender, Button.ButtonclickArgs e)
        {


            if (Scale < 1)
                Scale *= 2f;
        }

        void minusButton_Click(object sender, Button.ButtonclickArgs e)
        {

            if (Scale > 1.0f/8.0f)
                Scale /= 2f;
        }


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
