using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture
{
    public static class Helper
    {
        static public Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }


    }

    public delegate void DrawHandler(object sender, DrawArgs e);

    public class DrawArgs : EventArgs
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public Vector2 SpacePositionOffset { get; private set; }
        public float Scale { get; private set; }
        public Vector2 OnShipPosition { get; private set; }

        public DrawArgs(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            SpriteBatch = spriteBatch;
            SpacePositionOffset = spacePositionOffset;
            Scale = scale;
            OnShipPosition = new Vector2(0, 0);
        }

        public DrawArgs(SpriteBatch spriteBatch, Vector2 spacePositionOffset, Vector2 onShipPosition, float scale)
        {
            SpriteBatch = spriteBatch;
            SpacePositionOffset = spacePositionOffset;
            Scale = scale;
            OnShipPosition = onShipPosition;
        }
    }


    public delegate void UpdateHandler(object sender, UpdateArgs e);

    public class UpdateArgs : EventArgs
    {
        public GameTime GameTime {get; private set;}

        public UpdateArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }

    }

    public static class RandomExtensions
    {
        public static float Next(this Random rand, float min, float max)
        {
            return min + (max - min) * (float)rand.NextDouble();
        }
    }
}
