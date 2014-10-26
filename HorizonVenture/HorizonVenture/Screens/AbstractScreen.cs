using HorizonVenture.HorizonVenture.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Screens
{
    public abstract class AbstractScreen
    {
        protected HorizonVentureGame _game;
        protected Color _backgroundColor = Color.Black;
        

        public static AbstractScreen CurrentScreen { get; private set; }

        protected List<Control> _controls;

        public delegate void InitHandler(object sender, EventArgs e);
        public delegate void UnInitHandler(object sender, EventArgs e);

        public InitHandler OnInit;
        public UnInitHandler OnUnInit;

        public Boolean IsActive { get; private set; }

        protected AbstractScreen(HorizonVentureGame game)
        {
            _game = game;
            _controls = new List<Control>();
            IsActive = false;
        }

        public void Show()
        {
            if (CurrentScreen != null && CurrentScreen.Equals(this))
                return;

            if (CurrentScreen != null)
                CurrentScreen.RunUnInit();
            
            RunInit();           

            CurrentScreen = this;

        }

        private void RunInit()
        {
            IsActive = true;
            Init();

            if (OnInit != null)
                OnInit(this, new EventArgs());

            
        }

        private void RunUnInit()
        {
            

            IsActive = false;
            UnInit();

            if (OnUnInit != null)
                OnUnInit(this, new EventArgs());
        }

        protected abstract void Init();

        protected virtual void UnInit()
        { 
        }

        
        public virtual void Update(GameTime gameTime)
        {
            foreach (Control c in _controls)
                c.Update(gameTime);

            UpdateCoursorPosition();
        }

        protected void UpdateCoursorPosition()
        {
            _cursorPosition.X = InputManager.MouseState.X;
            _cursorPosition.Y = InputManager.MouseState.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawBackgroundColor();

            DrawControls(spriteBatch);

            DrawCursor(spriteBatch);
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

        protected Vector2 _cursorCenter = new Vector2(0, 0);
        protected Vector2 _cursorPosition = new Vector2(0, 0);
        protected Texture2D _cursor = null;
        protected Color _cursorDrawColor = Color.White;

        protected virtual void DrawCursor(SpriteBatch spriteBatch)
        {
            if (_cursor != null)
            {
                DrawCursorTexture(spriteBatch);
            }
        }

        protected virtual void DrawCursorTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_cursor, _cursorPosition, null, _cursorDrawColor, 0, _cursorCenter,
               1, SpriteEffects.None, 0);
        }

    }
}
