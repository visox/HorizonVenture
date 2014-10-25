using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Draw;
using HorizonVenture.HorizonVenture.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Space.SpaceEntities
{
    public abstract class AbstractSpaceEntity : ISpaceDrawable
    {
        public BlocksHolder BlocksHolder { get; protected set; }
        protected Color _color;

        public List<AbstractEntityComponent> EntityComponents { get; protected set; }

        public Vector2 SpacePosition { get; set; }
        public HorizonVentureSpace HorizonVentureSpace { get; protected set; }

        public float Angle { get; set; }

        public DrawHandler OnPreDrawSpaceEntity;
        public DrawHandler OnPostDrawSpaceEntity;

        public UpdateHandler OnPreUpdateSpaceEntity;
        public UpdateHandler OnPostUpdateSpaceEntity;

        protected AbstractSpaceEntity(HorizonVentureSpace horizonVentureSpace, Vector2 spacePosition)
        {
            SpacePosition = spacePosition;
            HorizonVentureSpace = horizonVentureSpace;
            EntityComponents = new List<AbstractEntityComponent>();
            _color = Color.White;
        }

        Vector2 _drawPosition = new Vector2(0, 0);

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {

            if (OnPreDrawSpaceEntity != null)
            {
                OnPreDrawSpaceEntity(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }

            InnerDraw(spriteBatch, spacePositionOffset, scale);

            if (OnPostDrawSpaceEntity != null)
            {
                OnPostDrawSpaceEntity(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }
        }

        protected virtual void InnerDraw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            BlocksHolder.Draw(spriteBatch, GetDrawPosition(spacePositionOffset, scale),
                           BlocksHolder.GetCenter(), Angle, _color, scale);
        }


        protected Vector2 GetDrawPosition(Vector2 spacePositionOffset, float scale)
        {
            _drawPosition.X = SpacePosition.X * scale;
            _drawPosition.Y = SpacePosition.Y * scale;

            _drawPosition.X += spacePositionOffset.X * scale;
            _drawPosition.Y += spacePositionOffset.Y * scale;

            return _drawPosition;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (OnPreUpdateSpaceEntity != null)
            {
                OnPreUpdateSpaceEntity(this, new UpdateArgs(gameTime));
            }

            InnerUpdate(gameTime);

            if (OnPostUpdateSpaceEntity != null)
            {
                OnPostUpdateSpaceEntity(this, new UpdateArgs(gameTime));
            }
        }

        protected virtual void InnerUpdate(GameTime gameTime)
        {

        }



    }
}
