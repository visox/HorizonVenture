using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Controls
{
    abstract class Control
    {
        public Rectangle Position { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public Boolean IsPointOverControl(Point p)
        {
            return Position.Contains(p);
        }
    }
}
