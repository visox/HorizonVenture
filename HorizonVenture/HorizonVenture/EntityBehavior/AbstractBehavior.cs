using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.EntityBehavior
{
    public abstract class AbstractBehavior
    {
        public AbstractSpaceEntity Owner { get; protected set; }

        protected AbstractBehavior(AbstractSpaceEntity owner)
        {
            Owner = owner;

            Owner.OnPostUpdateSpaceEntity += PostUpdateOwner;
        }

        private void PostUpdateOwner(object sender, UpdateArgs e)
        {
            Update(e.GameTime);
        }

        protected virtual void Update(GameTime gameTime)
        {
 
        }
    }
}
