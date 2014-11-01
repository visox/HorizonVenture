using HorizonVenture.HorizonVenture.Space;
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
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(MathHelper.ToRadians(angle)), -(float)Math.Cos(MathHelper.ToRadians(angle)));
        }

        public static float VectorToAngle(Vector2 vector)
        {
            float result = MathHelper.ToDegrees((float)Math.Atan2(vector.X, -vector.Y));
            if (result < 0)
                result += 360;

            return result;
        }

        private static Vector2 _vectorFromMiddle = new Vector2();
        public static Vector2 GetVectorFromMiddle(Vector2 to, HorizonVentureGame game)
        {
            _vectorFromMiddle.X = to.X - (game.GetScreenSize().X / 2);
            _vectorFromMiddle.Y = to.Y - (game.GetScreenSize().Y / 2);

            return _vectorFromMiddle;
        }
        private static Vector2 _vectorSpacePosition = new Vector2();
        public static Vector2 GetScreenPosToSpacePosition(int toX, int toY, HorizonVentureSpace space)
        {
            _vectorSpacePosition.X = ((toX - (space.getGame().GetScreenSize().X / 2)) / space.WorldScale) + space.PlayerShip.SpacePosition.X;
            _vectorSpacePosition.Y = ((toY - (space.getGame().GetScreenSize().Y / 2)) / space.WorldScale) + space.PlayerShip.SpacePosition.Y;

            return _vectorSpacePosition;
        }


        public static float GetAngleSpeedAcceleration(float power, float weight)
        {
            return (power * 200) / weight;
        }

        public static float GetVectorSpeed(float power, float weight)
        {
            return (power * 5000) / weight;
        }

        public static Vector2 RotateAroundOrigin(Vector2 point, Vector2 origin, float angle)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(MathHelper.ToRadians(angle))) + origin;
        }

        public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
   (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                                                                    original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }
            return ret;
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
        public GameTime GameTime { get; private set; }

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
