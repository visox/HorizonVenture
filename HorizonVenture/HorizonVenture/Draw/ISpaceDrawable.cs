using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Draw
{
    interface ISpaceDrawable
    {
        void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale);
    }
}
