using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects
{
    public abstract class GameEffect
    {
        public Vector2 SpacePosition { get; set; }
        public float Angle { get; set; }
        public HorizonVentureSpace HorizonVentureSpace { get; set; }

        protected GameEffect(Vector2 spacePosition, float angle, HorizonVentureSpace horizonVentureSpace)
        {
            SpacePosition = spacePosition;
            Angle = angle;

            HorizonVentureSpace = horizonVentureSpace;
        }

        public abstract void Update(GameTime gameTime);

        protected abstract void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale);
    }
}
