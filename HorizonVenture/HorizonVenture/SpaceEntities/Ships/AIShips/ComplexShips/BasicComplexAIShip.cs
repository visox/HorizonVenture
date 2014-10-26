using HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns;
using HorizonVenture.HorizonVenture.EntityComponents.EngineComponents;
using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.SpaceEntities.Ships.AIShips.ComplexShips
{
    public class BasicComplexAIShip : AbstractComplexAIShip
    {
        public BasicComplexAIShip(HorizonVentureSpace space, Vector2 spacePosition)
            : base(space, spacePosition)
        {
            BlocksHolder = BitmapBlocksHolderPatternSupplier.getPatterShip1(space.getGame());

            EntityComponents.Add(new SimpleEngine(this, new Vector2(-6, 22)));
            EntityComponents.Add(new SimpleEngine(this, new Vector2(7, 22)));
        }
    }
}
