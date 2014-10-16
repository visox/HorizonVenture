using HorizonVenture.HorizonVenture.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Screens
{
    abstract class AbstractScreen
    {
        protected HorizonVentureGame _game;
        protected Color _backgroundColor = Color.Black;

        public static AbstractScreen CurrentScreen { get; private set; }

        protected List<Control> _controls;

        protected AbstractScreen(HorizonVentureGame game)
        {
            _game = game;
            _controls = new List<Control>();
        }

        public void Show()
        {
            Init();
            CurrentScreen = this;
            
        }

        protected abstract void Init();

        protected virtual void UnInit()
        { 
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Control c in _controls)
                c.Update(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawBackgroundColor();

            DrawControls(spriteBatch);
        }

        protected void DrawControls(SpriteBatch spriteBatch)
        {
            foreach (Control c in _controls)
                c.Draw(spriteBatch);
        }

        protected void DrawBackgroundColor()
        {
            _game.GetGraphicsDevice().Clear(_backgroundColor);            
        }
    }
}
