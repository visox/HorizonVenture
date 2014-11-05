using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    public class RadioButtons : Control
    {
        private List<Button> _buttons;
        public String SelectedButtonTag { get; private set; }
        private Texture2D _background;

        public delegate void SelectedButtonChangedHandler(object sender, SelectedButtonChangedArgs e);

        public event SelectedButtonChangedHandler ButtonChanged;

        public Color DrawBackgroundColor { get; set; }

        public Color DrawBackgroundButtonColor { get; set; }
        public Color DrawForegroundButtonColor { get; set; }

        public Color DrawBackgroundSelectedButtonColor { get; set; }
        public Color DrawForegroundSelectedButtonColor { get; set; }

        public Vector2 Position { get; private set; }

        public Texture2D Background { get; private set; }

        public float ButtonSize { get; set; }
        public float ButtonMargin { get; set; }

        public static readonly float DEFAULT_BUTTON_SIZE = 50;
        public static readonly float DEFAULT_BUTTON_MARGIN = 20;

        public RadioButtons(Vector2 position, Texture2D background)
        {
            Position = position;

            _background = background;

            _buttons = new List<Button>();

            SelectedButtonTag = "";

            DrawBackgroundColor = Color.White;
            DrawBackgroundButtonColor = Color.White;
            DrawForegroundButtonColor = Color.White;
            DrawBackgroundSelectedButtonColor = Color.White;
            DrawForegroundSelectedButtonColor = Color.White;

            ButtonSize = DEFAULT_BUTTON_SIZE;
            ButtonMargin = DEFAULT_BUTTON_MARGIN;
        }

        public void AddButton(Button toAdd)
        {
            toAdd.DrawBackgroundColor = DrawBackgroundButtonColor;
            toAdd.DrawForegroundColor = DrawForegroundButtonColor;

            toAdd.Click += Button_Click;



            _buttons.Add(toAdd);
        }

        void Button_Click(object sender, Button.ButtonclickArgs e)
        {
            _buttons.ForEach(b => b.DrawBackgroundColor = DrawBackgroundButtonColor);
            _buttons.ForEach(b => b.DrawForegroundColor = DrawForegroundButtonColor);

            ((Button)sender).DrawBackgroundColor = DrawBackgroundSelectedButtonColor;
            ((Button)sender).DrawForegroundColor = DrawForegroundSelectedButtonColor;

            SelectedButtonTag = e.Tag;

            if (ButtonChanged != null)
            {
                ButtonChanged(this, new SelectedButtonChangedArgs());
            }
        }

        private void RefreshButtonPositionsAndSize()
        {
            _buttons.ForEach(b => 
            {
                b.SetPosition(Position.X + ButtonMargin,
                    ButtonMargin + (_buttons.IndexOf(b) * (ButtonMargin + ButtonSize)),
                    ButtonSize,
                    ButtonSize);
            });
        }

        private Rectangle _drawRectangle = new Rectangle();

        public override void Draw(SpriteBatch spriteBatch)
        {

            RefreshButtonPositionsAndSize();

            _drawRectangle.X = (int)Position.X;
            _drawRectangle.Y = (int)Position.Y;
            _drawRectangle.Width = (int)((2 * ButtonMargin) + ButtonSize);
            _drawRectangle.Height = (int)(ButtonMargin + (_buttons.Count * (ButtonSize+ ButtonMargin)));

            spriteBatch.Draw(_background, _drawRectangle, DrawBackgroundColor);

            _buttons.ForEach(b => b.Draw(spriteBatch));
        }

        public override void Update(GameTime gameTime)
        {
            _buttons.ForEach(b => b.Update(gameTime));
        }


        public class SelectedButtonChangedArgs : EventArgs
        {
            public SelectedButtonChangedArgs()
            {
            }
        }
    }
}
