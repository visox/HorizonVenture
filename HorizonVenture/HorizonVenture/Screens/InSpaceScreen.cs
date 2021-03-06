﻿using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Screens
{
    public class InSpaceScreen : AbstractScreen
    {

        public HorizonVentureSpace HorizonVentureSpace { get; set; }

        private int _fps;
        private SpriteFont _sp;

        public InSpaceScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;

            _sp = _game.GetContent().Load<SpriteFont>(@"Controls\Buttons\Fonts\Button");
        }

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.S, sKeyPressed);
            InputManager.AddKeyPressHandlers(Keys.H, hKeyPressed);
        }

        private void hKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _game.ShowShipHullEditScreen();
        }


        protected override void UnInit()
        {
            base.UnInit();

            InputManager.RemoveKeyPressHandlers(Keys.S, sKeyPressed);
            InputManager.RemoveKeyPressHandlers(Keys.S, hKeyPressed);

        }
        void sKeyPressed(object sender, InputManager.KeyPressArgs e)
        {
            _game.ShowShipEditScreen();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackgroundColor();

            if (HorizonVentureSpace != null)
                HorizonVentureSpace.Draw(spriteBatch);

           

            DrawControls(spriteBatch);

            DrawFps(spriteBatch);
        }

        private void DrawFps(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_sp, String.Format("{0} speed {1}", _fps, _game.PlayerShip.Speed.Length()), new Vector2(0, 0), Color.White);
        }

        private void UpdateFps(GameTime gameTime)
        {
            _fps = (int)(1.0 / (gameTime.ElapsedGameTime.Milliseconds / 1000.0));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HorizonVentureSpace != null)
                HorizonVentureSpace.Update(gameTime);

            UpdateFps(gameTime);
        }
    }
}
