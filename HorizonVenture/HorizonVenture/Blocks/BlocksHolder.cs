using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Blocks
{
    public class BlocksHolder
    {
        Dictionary<Vector2, AbstractBlock> _blocks;
        private HorizonVentureGame _game;
        private Texture2D [] _allTextures;
        private int _maxX, _minX, _maxY, _minY;

        public static readonly int SCALE_1_BLOCK_SIZE = 16;


        public BlocksHolder(HorizonVentureGame game)
        {
            _game = game;

            _blocks = new Dictionary<Vector2, AbstractBlock>();
            _maxX = this._maxY = this._minX = this._minY = 0;
        }

        public void ClearBlocks()
        {
            _blocks.Clear();
        }

        public Dictionary<Vector2, AbstractBlock> GetBlocks()
        {
            return _blocks;
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

            if ((_maxX - _minX) % 2 == 0)
                _maxX++;

            if ((_maxY - _minY) % 2 == 0)
                _maxY++;

            _allTextures = new Texture2D[4];

            _allTextures[0] =
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

                _allTextures[0].SetData<Color>(0, new Rectangle(
                        (int)(SCALE_1_BLOCK_SIZE * _realBlockPosition.X)
                        , (int)(SCALE_1_BLOCK_SIZE * _realBlockPosition.Y)
                        , (int)(SCALE_1_BLOCK_SIZE)
                        , (int)(SCALE_1_BLOCK_SIZE)), imageData,
                        0, imageData.Length);                    
            }

            for (int i = 1; i < _allTextures.Length; i++)
            {

                Color[] imageDataAll = new Color[(_allTextures[i - 1].Width * _allTextures[i - 1].Height)];
                _allTextures[i - 1].GetData<Color>(imageDataAll);

                Color[] imageDataAllOut = new Color[imageDataAll.Length / 4];
                for (int x = 0; x < _allTextures[i - 1].Width; x += 2)
                {
                    for (int y = 0; y < _allTextures[i - 1].Height; y += 2)
                    {
                        Color[] colors = new Color[4];

                        for (int xx = 0; xx < 2; xx++)
                        {
                            for (int yy = 0; yy < 2; yy++)
                            {
                                colors[(xx * 2) + yy] = imageDataAll[((y + yy) * _allTextures[i - 1].Width) + (x + xx)];
                            }
                        }

                        float averA = colors.Sum(c => c.A) / 4.0f;
                        float averB = colors.Sum(c => c.B) / 4.0f;
                        float averG = colors.Sum(c => c.G) / 4.0f;
                        float averR = colors.Sum(c => c.R) / 4.0f;

                        Color outputColor = new Color(averR / 255.0f, averG / 255.0f, averB / 255.0f, averA / 255.0f);

                        imageDataAllOut[((y / 2) * (_allTextures[i - 1].Width / 2)) + (x / 2)] = outputColor;
                    }
                }

                _allTextures[i] = new Texture2D(_game.GraphicsDevice, _allTextures[i - 1].Width / 2, _allTextures[i - 1].Height / 2);

                _allTextures[i].SetData<Color>(0, new Rectangle(
                            0, 0, _allTextures[i - 1].Width / 2, _allTextures[i - 1].Height / 2), imageDataAllOut,
                            0, imageDataAllOut.Length);
            }

        }


        public int GetBlocksCount()
        {
            return _blocks.Count;
        }

        private Vector2 _center = new Vector2();

        public Vector2 GetCenter()
        {
            _center.X = _maxX - _minX + 1;
            _center.Y = _maxY - _minY + 1;
            
            _center.X *= SCALE_1_BLOCK_SIZE / 2;
            _center.Y *= SCALE_1_BLOCK_SIZE / 2;

            return _center;
        }

        public float GetWidth()
        {
            return _maxX - _minX;
        }

        public float GetHeight()
        {
            return _maxY - _minY;
        }

        public int GetMinX()
        {
            return _minX;
        }

        public int GetMinY()
        {
            return _minY;
        }

        public int GetMaxX()
        {
            return _maxX;
        }

        public int GetMaxY()
        {
            return _maxY;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float angle, Color color, float scale)
        {
            int index = 0;
            float actualScale = scale;
            Texture2D actualTexture = _allTextures[index];
            Vector2 actualOrigin = origin;
            
            while (actualScale < 1 && index+1 < _allTextures.Length)
            {
                index++;
                actualScale *= 2;
                actualTexture = _allTextures[index];
                actualOrigin.X /= 2.0f;
                actualOrigin.Y /= 2.0f;
            }

            spriteBatch.Draw(actualTexture, position, null, color, MathHelper.ToRadians(angle), actualOrigin,
               actualScale, SpriteEffects.None, 0);
        }

        public Texture2D GetImage()
        {
            return _allTextures[0];
        }


        public AbstractBlock GetBlockByPosition(Vector2 position)
        {
            List<Vector2> key = _blocks.Keys.Where(k => position.X == k.X && position.Y == k.Y).ToList();
            if (key.Count() == 0)
                return null;

            return _blocks[key.ElementAt(0)];
        }
    }
}
