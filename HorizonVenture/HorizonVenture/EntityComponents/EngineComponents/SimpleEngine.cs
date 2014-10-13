using HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns;
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

         public SimpleEngine(AbstractSpaceEntity owner, Vector2 positionOnEntity)
             :base(owner, positionOnEntity)
        {

        }

         public SimpleEngine(PlayerShip ps)
             :base(ps)
        {

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
