using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    public class Panel : Control
    {
        protected Texture2D _background;

        public Color DrawBackgroundColor { get; set; }

        public List<Control> Controls { get; private set; }

        public Panel(Texture2D background, Rectangle position)
        {
            _background = background;
            Position = position;
            DrawBackgroundColor = Color.White;
            Controls = new List<Control>();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Position, DrawBackgroundColor);

            foreach(Control c in Controls)
            {
                c.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Control c in Controls)
            {
                c.Update(gameTime);
            }
        }
    }
}
