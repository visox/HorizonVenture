using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships
{
    abstract class AbstractShip: AbstractSpaceEntity
    {
        public AbstractShip(HorizonVentureSpace space, Vector2 spacePosition)
            : base(space, spacePosition)
        {

        }
    }
}
