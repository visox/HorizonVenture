using HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns;
using HorizonVenture.HorizonVenture.Draw;
using HorizonVenture.HorizonVenture.EntityComponents;
using HorizonVenture.HorizonVenture.EntityComponents.EngineComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships
{
    public class PlayerShip : AbstractShip
    {

        public List<AbstractEntityComponent> OwnedComponents { get; private set; }

        public PlayerShip(HorizonVentureSpace horizonVentureSpace, Vector2 spacePosition)
            : base(horizonVentureSpace, spacePosition)
        {
            BlocksHolder = BlocksHolderPatternSupplier.getExampleShipPatter(horizonVentureSpace.getGame());

        //    EntityComponents.Add(new SimpleEngine(this, new Vector2(0,0)));

            OwnedComponents = new List<AbstractEntityComponent>();
            OwnedComponents.Add(new SimpleEngine(this));
            OwnedComponents.Add(new SimpleEngine(this));
            OwnedComponents.Add(new SimpleEngine(this));
        }




        public override void Update(GameTime gameTime)
        {
          //  userInputHandle();

          //  rotate(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        private void rotate(double delta)
        {
            this.Angle = (float)((this.Angle + (0.001f * delta)) % (2*Math.PI));
        }



        private void userInputHandle()
        {

            if (InputManager.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Point pos = InputManager.MouseState.Position;

                Vector2 realPos = new Vector2(pos.X * HorizonVentureSpace.getGame().GetScreenSize().X,
                    pos.Y * HorizonVentureSpace.getGame().GetScreenSize().Y);

                Vector2 offset = new Vector2(realPos.X - (HorizonVentureSpace.getGame().GetScreenSize().X / 2),
                    realPos.Y - (HorizonVentureSpace.getGame().GetScreenSize().Y / 2));

                if (offset.Length() > 10)
                {
                    offSetPosition(offset);
                }

            }
        }

        private void offSetPosition(Vector2 offset)
        {
            this.SpacePosition = new Vector2(this.SpacePosition.X + offset.X, this.SpacePosition.Y + offset.Y);
            //   this._world.offSetWorldPosition(offset.X, offset.Y);
        }

        public void DetailShipDraw(SpriteBatch spriteBatch, Vector2 screenCenter, float scale)
        {

            if (OnPreDrawSpaceEntity != null)
            {
                OnPreDrawSpaceEntity(this, new DrawArgs(spriteBatch, screenCenter, scale));
            }          

            this.BlocksHolder.Draw(spriteBatch, screenCenter,
                this.BlocksHolder.GetCenter(), 0, _color, scale);

            if (OnPostDrawSpaceEntity != null)
            {
                OnPostDrawSpaceEntity(this, new DrawArgs(spriteBatch, screenCenter, scale));
            }
        }

    }
}
