using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns
{
    class BlocksHolderPatternSupplier
    {
        public static BlocksHolder getExampleShipPatter(HorizonVentureGame game)
        {
            BlocksHolder result = new BlocksHolder(game);

            Dictionary<Vector2, AbstractBlock> blocks = new Dictionary<Vector2, AbstractBlock>();

            for (int y = -10; y <= 10; y++ )
            {
                int xwide = 22 + y*2;
                for (int x = -xwide / 2; x <= xwide /2; x++)
                {
                    blocks.Add(new Vector2(x, y), new Block("metal1"));
                }
            }

            result.addBlocks(blocks);

            return result;
        }

        public static BlocksHolder getExampleShipEnginePatter(HorizonVentureGame game)
        {
            BlocksHolder result = new BlocksHolder(game);

            Dictionary<Vector2, AbstractBlock> blocks = new Dictionary<Vector2, AbstractBlock>();


            for (int y = -1; y <= 1; y++)
            {
                int xwide = 4 + y * 2;
                for (int x = -xwide / 2; x <= xwide / 2; x++)
                {
                    blocks.Add(new Vector2(x, y), new Block("metal2"));
                }
            }
            for (int y = 2; y <= 5; y++)
            {                
                for (int x = -3; x <= 3; x++)
                {
                    blocks.Add(new Vector2(x, y), new Block("metal2"));
                }
            }

            blocks.Add(new Vector2(-3, 6), new Block("metal2"));
            blocks.Add(new Vector2(3, 6), new Block("metal2"));

            result.addBlocks(blocks);

            return result;
        }
    }
}
