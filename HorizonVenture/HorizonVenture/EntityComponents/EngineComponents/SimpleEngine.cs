using HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns;
using HorizonVenture.HorizonVenture.Effects.ImpEffects;
using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.EntityComponents.EngineComponents
{
    class SimpleEngine : AbstractEngine
    {

        private static readonly float POWER = 500;

         public SimpleEngine(AbstractSpaceEntity owner, Vector2 positionOnEntity)
            : base(owner, positionOnEntity)
        {

        }

         public SimpleEngine(PlayerShip ps)
             :base(ps)
        {
            
        }

         public override float GetCurrentPower()
         {
             return POWER;
         }

         public override void OnShipInit()
         {
             base.OnShipInit();

             SimpleEngineEffect engineEffect = new SimpleEngineEffect(this, 
                 new Vector2(0,
                     (3 * Blocks.BlocksHolder.SCALE_1_BLOCK_SIZE)),
                 Owner.SpacePosition, 0, Owner.HorizonVentureSpace);

             Effects.Add(engineEffect);

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
