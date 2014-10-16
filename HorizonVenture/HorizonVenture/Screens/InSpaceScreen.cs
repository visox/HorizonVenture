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
    class InSpaceScreen : AbstractScreen
    {

        public HorizonVentureSpace HorizonVentureSpace { get; set; }

        public InSpaceScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;
        }

        protected override void Init()
        {
            InputManager.AddKeyPressHandlers(Keys.S, sKeyPressed);
        }


        protected override void UnInit()
        {
            base.UnInit();

            InputManager.RemoveKeyPressHandlers(Keys.S, sKeyPressed);

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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HorizonVentureSpace != null)
                HorizonVentureSpace.Update(gameTime);
        }
    }
}
