using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Draw;
using HorizonVenture.HorizonVenture.Effects;
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
    public abstract class AbstractEntityComponent
    {
        public AbstractSpaceEntity Owner { get; protected set; }
        public Vector2 PositionOnEntity { get; set; }
        public BlocksHolder BlocksHolder { get; protected set; }
        public Color Color { get; protected set; }
        public float Angle { get; protected set; }
        public List<GameEffect> Effects { get; protected set; }

        public DrawHandler OnPreDrawComponent;
        public DrawHandler OnPostDrawComponent;

        public DrawHandler OnPreDrawEditorComponent;
        public DrawHandler OnPostDrawEditorComponent;

        public UpdateHandler OnPreUpdateSpaceComponent;
        public UpdateHandler OnPostUpdateSpaceComponent;

       // private Boolean _isOnShip;

        public AbstractEntityComponent(AbstractSpaceEntity owner, Vector2 positionOnEntity)
        {
            Owner = owner;
            PositionOnEntity = positionOnEntity;

            SameInit();
            OnShipInit();
        }

        public AbstractEntityComponent(PlayerShip ps)
        {
            Owner = ps;
            SameInit();
        }

        protected virtual void SameInit()
        {
            Color = Color.White;
            LoadBlocksHolder();            
        }        

        public virtual void OnShipInit()
        {
            Effects = new List<GameEffect>();
            Angle = 0;
            Owner.OnPostDrawSpaceEntity += PostDrawOwner;
            Owner.OnPostUpdateSpaceEntity += PostUpdateOwner;

            if (Owner is PlayerShip)
            {
                PlayerShip ps = (PlayerShip)Owner;

                ps.OnPostDrawEditorSpaceEntity += PostDrawEditorOwner;
            }
        }

        private void PostDrawEditorOwner(object sender, DrawArgs e)
        {
            DrawEditor(e.SpriteBatch, e.SpacePositionOffset, e.Scale);
        }

        private void PostUpdateOwner(object sender, UpdateArgs e)
        {
            Update(e.GameTime);
        }

        private void PostDrawOwner(object sender, DrawArgs e)
        {
            Draw(e.SpriteBatch, e.SpacePositionOffset, e.Scale);
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

        protected virtual void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {


            if (OnPreDrawComponent != null)
            {
                OnPreDrawComponent(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }

            InnerDraw(spriteBatch, spacePositionOffset, PositionOnEntity, scale);

            if (OnPostDrawComponent != null)
            {
                OnPostDrawComponent(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }
        }

        protected virtual void DrawEditor(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {


            if (OnPreDrawEditorComponent != null)
            {
                OnPreDrawEditorComponent(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }

            InnerDraw(spriteBatch, spacePositionOffset, PositionOnEntity, scale, true);

            if (OnPostDrawEditorComponent != null)
            {
                OnPostDrawEditorComponent(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }
        }

        public void UpdatePositionOnShipByCenterChange(float changeX, float changeY)
        {
           // float realChangeX
            PositionOnEntity = new Vector2(PositionOnEntity.X - changeX, PositionOnEntity.Y - changeY);
        }

        private Vector2 _realSpacePosition = new Vector2();

        public Vector2 GetRealZeroAngleSpacePosition()
        {
            _realSpacePosition.X = Owner.SpacePosition.X;
            _realSpacePosition.Y = Owner.SpacePosition.Y;

            _realSpacePosition.X += (PositionOnEntity.X) * BlocksHolder.SCALE_1_BLOCK_SIZE;
            _realSpacePosition.Y += (PositionOnEntity.Y) * BlocksHolder.SCALE_1_BLOCK_SIZE;

           // _realSpacePosition = Helper.RotateAroundOrigin(_realSpacePosition, Owner.SpacePosition, Owner.Angle);

            return _realSpacePosition;
        }

        protected Vector2 GetDrawPosition(Vector2 spacePositionOffset, float scale, Vector2 onShipPosition, bool isEditorMode = false)
        {
            _drawPosition.X = 0;
            _drawPosition.Y = 0;

            if (!isEditorMode)
            {
                _drawPosition.X = Owner.SpacePosition.X;
                _drawPosition.Y = Owner.SpacePosition.Y;
            }

            _drawPosition.X += (onShipPosition.X) * BlocksHolder.SCALE_1_BLOCK_SIZE;
            _drawPosition.Y += (onShipPosition.Y) * BlocksHolder.SCALE_1_BLOCK_SIZE;

            float actualAngle = Owner.Angle;

            if (isEditorMode)
                actualAngle = 0;

            _drawPosition = Helper.RotateAroundOrigin(_drawPosition, Owner.SpacePosition, actualAngle);

            _drawPosition.X *= scale;
            _drawPosition.Y *= scale;

            _drawPosition.X += spacePositionOffset.X * scale;
            _drawPosition.Y += spacePositionOffset.Y * scale;

            return _drawPosition;
        }

        public virtual void DrawFree(SpriteBatch spriteBatch, Vector2 spacePositionOffset, Vector2 onShipPosition, float scale)
        {
            if (OnPreDrawComponent != null)
            {
                OnPreDrawComponent(this, new DrawArgs(spriteBatch, spacePositionOffset, onShipPosition, scale));
            }

            InnerDraw(spriteBatch, spacePositionOffset, onShipPosition, scale);


            if (OnPostDrawComponent != null)
            {
                OnPostDrawComponent(this, new DrawArgs(spriteBatch, spacePositionOffset, onShipPosition, scale));
            }
        }

        protected virtual void InnerDraw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, Vector2 onShipPosition, float scale,
            bool isEditorMode = false )
        {
            float actualAngle = Owner.Angle;

            if (isEditorMode)
                actualAngle = 0;

            BlocksHolder.Draw(spriteBatch, GetDrawPosition(spacePositionOffset, scale, onShipPosition, isEditorMode),
                BlocksHolder.GetCenter(), actualAngle, Color, scale);
        }
        
        public virtual Texture2D GetImage()
        {
            return BlocksHolder.GetImage();
        }

        protected virtual void Update(GameTime gameTime)
        {
            if (OnPreUpdateSpaceComponent != null)
            {
                OnPreUpdateSpaceComponent(this, new UpdateArgs(gameTime));
            }

            InnerUpdate(gameTime);

            if (OnPostUpdateSpaceComponent != null)
            {
                OnPostUpdateSpaceComponent(this, new UpdateArgs(gameTime));
            }
        }

        private void InnerUpdate(GameTime gameTime)
        {

        }

       /* protected void UpdateEffects(GameTime gameTime)
        {
            if (Effects != null)
                Effects.ForEach(e => e.Update(gameTime));
        }*/

    }
}
