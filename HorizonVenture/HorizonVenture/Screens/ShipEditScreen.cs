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

        private static readonly int BUTTON_WIDTH = 300;

        private static readonly int BUTTON_HEIGHT = 50;

        private static readonly int PLUS_MINUS_WIDTH = 50;
        private static readonly int PANEL_WIDTH = 300;

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.S, sKeyPressed);
            PlayerShip = _game.PlayerShip;

            if (_controls.Count != 0)
                return;

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Fonts\Button");
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

        private static readonly int COMPONENTS_SHOW_COUNT = 5;

        private void AddRightShipComponentsPanel()
        {
            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            _rightShipComponentsPanel = new Panel(background,
                           new Rectangle((int)(screenSize.X - PANEL_WIDTH), 0, PANEL_WIDTH, (int)(screenSize.Y)));

            _controls.Add(_rightShipComponentsPanel);


            AddComponentsToRightPanel();
        }

        private static readonly float COMPONENTS_IMAGEBUTTON_SIZE = 200;
        private static readonly float COMPONENTS_IMAGEBUTTON_MARGIN = 20;

        private void AddComponentsToRightPanel()
        {
            _rightShipComponentsPanel.Controls.Clear();

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Fonts\Button");
            Vector2 screenSize = _game.GetScreenSize();

            for (int i = ComponentsPanelIndex; i < COMPONENTS_SHOW_COUNT + ComponentsPanelIndex; i++)
            {
                if (i >= PlayerShip.OwnedComponents.Count)
                    break;

                Texture2D componentImage = PlayerShip.OwnedComponents[i].GetImage();

                ImageButton addComponentButton = new ImageButton(background, componentImage,
                    new Rectangle(
                        (int)(_rightShipComponentsPanel.Position.Left +
                            ((_rightShipComponentsPanel.Position.Width - COMPONENTS_IMAGEBUTTON_SIZE) / 2)),
                        (int)(_rightShipComponentsPanel.Position.Top + COMPONENTS_IMAGEBUTTON_MARGIN +
                            ((COMPONENTS_IMAGEBUTTON_SIZE + COMPONENTS_IMAGEBUTTON_MARGIN) * (i - ComponentsPanelIndex))),
                        (int)COMPONENTS_IMAGEBUTTON_SIZE, 
                        (int)COMPONENTS_IMAGEBUTTON_SIZE));

                addComponentButton.Tag = i.ToString();
                addComponentButton.Scale = 1f;

                addComponentButton.DrawBackgroundColor = Color.White;
                addComponentButton.Click += addComponentButton_Click;

                _controls.Add(addComponentButton);
            }
        }

        protected override void UnInit()
        {
            base.UnInit();

            InputManager.RemoveKeyPressHandlers(Keys.S, sKeyPressed);

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


        void addComponentButton_Click(object sender, Button.ButtonclickArgs e)
        {
            _cursor = PlayerShip.OwnedComponents[int.Parse(e.Tag)].GetImage();
            _cursorCenter.X = _cursor.Width / 2;
            _cursorCenter.Y = _cursor.Height / 2;
        }
    }
}
