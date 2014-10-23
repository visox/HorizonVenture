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

        protected AbstractSpaceEntity(HorizonVentureSpace horizonVentureSpace, Vector2 spacePosition)
        {
            SpacePosition = spacePosition;
            HorizonVentureSpace = horizonVentureSpace;
            EntityComponents = new List<AbstractEntityComponent>();
            _color = Color.White;
        }

        Vector2 _drawPosition = new Vector2(0,0);

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            this.BlocksHolder.Draw(spriteBatch, GetDrawPosition(spacePositionOffset), 
                this.BlocksHolder.GetCenter(), Angle, _color, scale);

            DrawEntityComponents(spriteBatch, spacePositionOffset, scale);
        }

        protected void DrawEntityComponents(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            foreach (AbstractEntityComponent aec in EntityComponents)
            {
                aec.Draw(spriteBatch, spacePositionOffset, scale);
            }
        }

        protected Vector2 GetDrawPosition(Vector2 spacePositionOffset)
        {
            _drawPosition.X = SpacePosition.X;
            _drawPosition.Y = SpacePosition.Y;

            _drawPosition.X += spacePositionOffset.X;
            _drawPosition.Y += spacePositionOffset.Y;

            return _drawPosition;
        }

        public virtual void Update(GameTime gameTime)
        {
 
        }



    }
}
