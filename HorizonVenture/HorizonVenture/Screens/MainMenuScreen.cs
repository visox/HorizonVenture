using HorizonVenture.HorizonVenture.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace HorizonVenture.HorizonVenture.Screens
{
    class MainMenuScreen : AbstractScreen
    {

        private static readonly int BUTTON_WIDTH = 300;
        private static readonly int BUTTON_HEIGHT = 50;

        public MainMenuScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Gray;
        }

        protected override void Init()
        {
            

            if (_controls.Count != 0)
                return;

            Texture2D background = _game.GetContent().Load<Texture2D>(@"Controls\Buttons\background1");
            SpriteFont spriteFont = _game.GetContent().Load<SpriteFont>(@"Fonts\Button");



            Vector2 screenSize = _game.GetScreenSize();

            Button startGame = new Button(background, "New game", spriteFont,
                new Rectangle((int)((screenSize.X / 2) - (BUTTON_WIDTH / 2)), 120, BUTTON_WIDTH, BUTTON_HEIGHT));

            startGame.DrawBackgroundColor = Color.Green;
            startGame.Click += startGame_Click;

            _controls.Add(startGame);


            Button endGame = new Button(background, "Quit", spriteFont,
                new Rectangle((int)((screenSize.X / 2) - (BUTTON_WIDTH / 2)), (int)(screenSize.Y - 120), BUTTON_WIDTH, BUTTON_HEIGHT));

            endGame.DrawBackgroundColor = Color.Green;
            endGame.Click += endGame_Click;

            _controls.Add(endGame);

            
        }

        

        void endGame_Click(object sender, Button.ButtonclickArgs e)
        {
            _game.QuitGame();
        }

        void startGame_Click(object sender, Button.ButtonclickArgs e)
        {
            _game.ShowInSpaceScreen();
        }
    }
}
