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

        public ShipEditScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;
            ScreenCenter = new Vector2((game.GetScreenSize().X - PANEL_WIDTH )/ 2, game.GetScreenSize().Y / 2);
        }

        private static readonly int BUTTON_WIDTH = 300;

        private static readonly int BUTTON_HEIGHT = 50;

        private static readonly int PLUS_MINUS_WIDTH = 50;
        private static readonly int PANEL_WIDTH = 300;

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.S, sKeyPressed);

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


            Panel rightShipComponentsPanel = new Panel(background, 
                new Rectangle((int)(screenSize.X - PANEL_WIDTH), 0, PANEL_WIDTH, (int)(screenSize.Y)));

           

            _controls.Add(rightShipComponentsPanel);
            
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
        }

        private void DrawShip(SpriteBatch spriteBatch)
        {
            if (PlayerShip != null)
            {
                PlayerShip.DetailShipDraw(spriteBatch, ScreenCenter, 1);
            }
        }

        void sKeyPressed(object sender, InputManager.KeyPressArgs e)
        {

        }

        void plusButton_Click(object sender, Button.ButtonclickArgs e)
        {
           
        }

        void minusButton_Click(object sender, Button.ButtonclickArgs e)
        {

        }
    }
}
