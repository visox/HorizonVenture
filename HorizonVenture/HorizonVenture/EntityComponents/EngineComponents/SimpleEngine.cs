using HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns;
using HorizonVenture.HorizonVenture.Effects.ImpEffects;
using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.EntityComponents.EngineComponents
{
    class SimpleEngine : AbstractEngine
    {

        SimpleEngineEffect test;

         public SimpleEngine(AbstractSpaceEntity owner, Vector2 positionOnEntity)
             :base(owner, positionOnEntity)
        {
            test = new SimpleEngineEffect(owner.SpacePosition, 0, owner.HorizonVentureSpace);
        }

         public SimpleEngine(PlayerShip ps)
             :base(ps)
        {
            test = new SimpleEngineEffect(Owner.SpacePosition, 0, Owner.HorizonVentureSpace);
        }

         public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
         {
             base.Draw(spriteBatch, spacePositionOffset, scale);
             test.Draw(spriteBatch, spacePositionOffset, scale);
         }

         public override void Update(GameTime gameTime)
         {
             test.Update(gameTime);
         } 

        public override bool CanAdd(Vector2 position)
        {
            return true;
        }

        protected override void LoadBlocksHolder()
        {
            BlocksHolder = BlocksHolderPatternSupplier.getExampleShipEnginePatter(Owner.HorizonVentureSpace.getGame());
        }
    }
}
