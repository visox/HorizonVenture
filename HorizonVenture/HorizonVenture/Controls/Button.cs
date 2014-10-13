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
        private Texture2D _background;
        private string _text;
        private SpriteFont _font;

        public Color DrawBackgroundColor { get; set; }
        public Color DrawFontColor { get; set; }

        private Vector2 _textPos;
        private Vector2 _textCenter;

        public delegate void ButtonClickHandler(object sender, ButtonclickArgs e);

        public event ButtonClickHandler Click;



        public Button(Texture2D background, string text, SpriteFont font, Rectangle position)
        {
            _background = background;
            _text = text;
            _font = font;

            Position = position;

            DrawBackgroundColor = Color.White;
            DrawFontColor = Color.Black;

            _textPos = new Vector2(Position.Center.ToVector2().X, Position.Center.ToVector2().Y);
            Vector2 size = _font.MeasureString(_text);
            _textCenter = new Vector2(size.X / 2, size.Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Position, DrawBackgroundColor);

            spriteBatch.DrawString(_font, _text, _textPos, DrawFontColor, 0, 
                _textCenter , 1, SpriteEffects.None, 0);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if(InputManager.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if(Position.Contains(InputManager.MouseState.Position))
                {
                    if(Click != null)
                        Click(this, new ButtonclickArgs());
                }
            }
        }


        public class ButtonclickArgs : EventArgs
        {

            public ButtonclickArgs()
            {
            }
        }


    }
}
