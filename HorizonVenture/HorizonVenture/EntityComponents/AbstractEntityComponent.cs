using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.EntityComponents
{
    abstract class AbstractEntityComponent
    {
        public AbstractSpaceEntity Owner { get; protected set; }
        public Vector2 PositionOnEntity { get; protected set; }
        public BlocksHolder BlocksHolder { get; protected set; }
        public Color Color { get; protected set; }

        public AbstractEntityComponent(AbstractSpaceEntity owner, Vector2 positionOnEntity)
        {
            Owner = owner;
            PositionOnEntity = positionOnEntity;
            Color = Color.White;
            LoadBlocksHolder();
        }

        public AbstractEntityComponent(PlayerShip ps)
        {
            Owner = ps;
            ps.OwnedComponents.Add(this);
            Color = Color.White;
            LoadBlocksHolder();
        }

        protected abstract void LoadBlocksHolder();

        public abstract Boolean CanAdd(Vector2 position);

        protected Boolean IsFreeSpace(Vector2 position)
        {



            return true;
        }

        private static Boolean IsIntersection(BlocksHolder bh1, Vector2 pos1, BlocksHolder bh2, Vector2 pos2)
        {



            return false;
        }

        Vector2 _drawPosition = new Vector2();

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {

            this.BlocksHolder.Draw(spriteBatch, GetDrawPosition(spacePositionOffset, scale, PositionOnEntity),
                this.BlocksHolder.GetCenter(), Owner.Angle, Color, scale);
        }

        protected Vector2 GetDrawPosition(Vector2 spacePositionOffset, float scale, Vector2 onShipPosition)
        {
            _drawPosition.X = Owner.SpacePosition.X;
            _drawPosition.Y = Owner.SpacePosition.Y;

            _drawPosition.X += onShipPosition.X * scale;
            _drawPosition.Y += onShipPosition.Y * scale;

            _drawPosition.X += spacePositionOffset.X;
            _drawPosition.Y += spacePositionOffset.Y;

            return _drawPosition;
        }

        public virtual void DrawFree(SpriteBatch spriteBatch, Vector2 spacePositionOffset, Vector2 onShipPosition, float scale)
        {

            this.BlocksHolder.Draw(spriteBatch, GetDrawPosition(spacePositionOffset, scale, onShipPosition),
                this.BlocksHolder.GetCenter(), Owner.Angle, Color, scale);
        }

    }
}
