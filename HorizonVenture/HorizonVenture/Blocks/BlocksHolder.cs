using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Blocks
{
    class BlocksHolder
    {
        Dictionary<Vector2, AbstractBlock> _blocks;
        private HorizonVentureGame _game;
        private Texture2D _allTextures;
        private int _maxX, _minX, _maxY, _minY;

        private static readonly int SCALE_1_BLOCK_SIZE = 16;


        public BlocksHolder(HorizonVentureGame game)
        {
            _game = game;

            _blocks = new Dictionary<Vector2, AbstractBlock>();
            _maxX = this._maxY = this._minX = this._minY = 0;
        }

        Vector2 _realBlockPosition = new Vector2();

        public void addBlocks(Dictionary<Vector2, AbstractBlock> blocksToAdd)
        {
            if (blocksToAdd.Count == 0)
                return;

            foreach (Vector2 v in blocksToAdd.Keys)
            {
                if (_blocks.ContainsKey(v))
                {
                    _blocks[v] = blocksToAdd[v];
                }
                else
                {
                    _blocks.Add(v, blocksToAdd[v]);
                }

                if (v.X > _maxX)
                    _maxX = (int)v.X;
                if (v.X < _minX)
                    _minX = (int)v.X;
                if (v.Y > _maxY)
                    _maxY = (int)v.Y;
                if (v.Y < _minY)
                    _minY = (int)v.Y;
            }


            _allTextures =
                     new Texture2D(
               _game.GraphicsDevice,
               (int)(SCALE_1_BLOCK_SIZE * 1 * ((_maxX - _minX) + 1)),
               (int)(SCALE_1_BLOCK_SIZE * 1 * ((_maxY - _minY) + 1)),
               false, SurfaceFormat.Color);



            foreach (Vector2 v in this._blocks.Keys)
            {
                _realBlockPosition.X = v.X - _minX;
                _realBlockPosition.Y = v.Y - _minY;

                Texture2D toDraw = _blocks[v].getTexture(_game);
                

                Color[] imageData = new Color[(toDraw.Width * toDraw.Height)];
                toDraw.GetData<Color>(imageData);

                _allTextures.SetData<Color>(0, new Rectangle(
                        (int)(SCALE_1_BLOCK_SIZE * _realBlockPosition.X)
                        , (int)(SCALE_1_BLOCK_SIZE * _realBlockPosition.Y)
                        , (int)(SCALE_1_BLOCK_SIZE)
                        , (int)(SCALE_1_BLOCK_SIZE)), imageData,
                        0, imageData.Length);                    
            }

        }

        private Vector2 _center = new Vector2();

        public Vector2 GetCenter()
        {
            _center.X = ((_maxX - _minX + 1f) * SCALE_1_BLOCK_SIZE) /2;
            _center.Y = ((_maxY - _minY + 1f) * SCALE_1_BLOCK_SIZE) /2;

            return _center;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float angle, Color color, float scale)
        {

            spriteBatch.Draw(this._allTextures, position, null, color, angle, origin,
               scale, SpriteEffects.None, 0);

        }

    }
}
