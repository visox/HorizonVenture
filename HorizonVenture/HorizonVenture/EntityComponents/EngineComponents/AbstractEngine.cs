using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.EntityComponents.EngineComponents
{
    public abstract class AbstractEngine : AbstractEntityComponent
    {
        public AbstractEngine(AbstractSpaceEntity owner, Vector2 positionOnEntity)
             :base(owner, positionOnEntity)
        {

        }

        public AbstractEngine(PlayerShip ps)
             :base(ps)
        {

        }

        protected Boolean IsPositionOnBack(Vector2 position)
        {
            throw new NotImplementedException("not implemented IsPositionOnBack");
        }


    }
}
