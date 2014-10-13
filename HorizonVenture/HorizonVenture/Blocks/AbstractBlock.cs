﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Blocks
{
    class AbstractBlock
    {
        protected string _id;

        protected AbstractBlock(string id)
        {
            _id = id;
        }

        public virtual Texture2D getTexture(HorizonVentureGame game)
        {
            return BlockDefinitions.getTextureById(this._id, game);
        }


    }
}
