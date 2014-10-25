using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Blocks.BlocksHolderPatterns
{
    public static class BitmapBlocksHolderPatternSupplier
    {
        public static BlocksHolder getPatterByFile(HorizonVentureGame game, string bitmapFile, 
            Dictionary<Color, string> blocksDic, string defaultBlock = "")
        {
            BlocksHolder result = new BlocksHolder(game);

            Texture2D bitmap = game.GetContent().Load<Texture2D>(bitmapFile);

            Color [] data = new Color[bitmap.Height * bitmap.Width];

            bitmap.GetData<Color>(data);

            Dictionary<Vector2, AbstractBlock> blocks = new Dictionary<Vector2, AbstractBlock>();

            for (int x = 0; x < bitmap.Width; x++ )
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = data[(y * bitmap.Width) + x];

                    string blockType = "";

                    if (blocksDic.ContainsKey(color))
                    {
                        blockType = blocksDic[color];
                    }
                    else
                    {
                        blockType = defaultBlock;
                    }

                    if (!blockType.Equals(""))
                        blocks.Add(new Vector2(x, y), new Block(blockType));
                }
            }

            result.addBlocks(blocks);

            return result;
        }

        public static BlocksHolder getPatterShip1(HorizonVentureGame game)
        {
            Dictionary<Color, string> colorToBlock = new Dictionary<Color,string>();

            colorToBlock.Add(Color.White, "");

            return getPatterByFile(game, "Ships\\ship1", colorToBlock, "metal1");
        }
    }
}
