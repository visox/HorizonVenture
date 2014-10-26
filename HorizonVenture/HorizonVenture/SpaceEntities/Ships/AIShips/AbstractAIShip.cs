using HorizonVenture.HorizonVenture.Space;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.SpaceEntities.Ships.AIShips
{
    public class AbstractAIShip : AbstractShip
    {
        public AbstractAIShip(HorizonVentureSpace space, Vector2 spacePosition)
            : base(space, spacePosition)
        {

        }
    }
}
