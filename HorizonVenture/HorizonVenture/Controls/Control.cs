using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    public abstract class Control
    {
        private Rectangle _position;

        public Rectangle Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public Boolean IsPointOverControl(Point p)
        {
            return Position.Contains(p);
        }

        public void SetPosition(float x, float y, float width, float height)
        {
            _position.X = (int)x;
            _position.Y = (int)y;
            _position.Width = (int)width;
            _position.Height = (int)height;
        }
    }
}
