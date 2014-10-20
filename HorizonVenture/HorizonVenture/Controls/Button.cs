using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    class Button : Control
    {
        protected Texture2D _background;
        protected string _text;
        protected SpriteFont _font;

        public Color DrawBackgroundColor { get; set; }
        public Color DrawFontColor { get; set; }
        public float RefreshPressMilliseconds { get; set; }
        public String Tag { get; set; }
        private float _currentDelay;

        protected Vector2 _textPos;
        protected Vector2 _textCenter;

        public delegate void ButtonClickHandler(object sender, ButtonclickArgs e);

        public event ButtonClickHandler Click;

        private static readonly float DEFAULT_REFRESH_PRESS_TIME = 100;

        public Button(Texture2D background, string text, SpriteFont font, Rectangle position)
        {
            _background = background;
            _text = text;
            _font = font;
            Tag = "";

            Position = position;

            DrawBackgroundColor = Color.White;
            DrawFontColor = Color.Black;

            RefreshPressMilliseconds = DEFAULT_REFRESH_PRESS_TIME;
            _currentDelay = 0;

            _textPos = new Vector2(Position.Center.ToVector2().X, Position.Center.ToVector2().Y);
            if (_font != null)
            {
                Vector2 size = _font.MeasureString(_text);
                _textCenter = new Vector2(size.X / 2, size.Y / 2);
            }
            else
            {
                _textCenter = new Vector2(0, 0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Position, DrawBackgroundColor);

            spriteBatch.DrawString(_font, _text, _textPos, DrawFontColor, 0, 
                _textCenter , 1, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();

            UpdateDelay(gameTime);
        }

        private void UpdateDelay(GameTime gameTime)
        {
            if (_currentDelay <= 0)
            {
                return;
            }

            _currentDelay -= gameTime.ElapsedGameTime.Milliseconds;
        }

        private void HandleInput()
        {
            if (_currentDelay > 0)
            {
                return; 
            }
            if(InputManager.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if(Position.Contains(InputManager.MouseState.Position))
                {
                    if (Click != null)
                    {
                        Click(this, new ButtonclickArgs(Tag));
                        _currentDelay = RefreshPressMilliseconds;
                    }
                }
            }
        }


        public class ButtonclickArgs : EventArgs
        {
            public String Tag { get; private set; }

            public ButtonclickArgs()
            {
            }

            public ButtonclickArgs(String tag)
            {
                Tag = tag;
            }
        }


    }
}
