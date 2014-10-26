using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using HorizonVenture.HorizonVenture.SpaceEntities.Ships.AIShips.ComplexShips;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Space
{
    public class HorizonVentureSpace
    {
        Vector2 _spacePositionOffset;
        float _worldScale;

        public float WorldScale
        {
            get { return _worldScale; }
            private set { _worldScale = value; }
        }

        HorizonVentureGame _game;

        private PlayerShip _playerShip;
        public PlayerShip PlayerShip 
        {
            get { return _playerShip; }
            set 
            {
                if (!_entities.Contains(value) || !value.Equals(_playerShip))
                {
                    _playerShip = value;
                    _entities.Add(value);
                }
            }
        }
        List<AbstractSpaceEntity> _entities;

        public HorizonVentureSpace(HorizonVentureGame game, PlayerShip playerShip) 
        {
            _game = game;

            _spacePositionOffset = new Vector2();
            _worldScale = 0.0625f;

            _entities = new List<AbstractSpaceEntity>();
            _entities.Add(playerShip);


            _entities.Add(new BasicComplexAIShip(this, new Vector2(-1200,-1200)));
            
            
        }

        public HorizonVentureGame getGame()
        {
            return this._game;
        }

        public void LoadContent()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spacePositionOffset.X = -PlayerShip.SpacePosition.X + ((_game.GetScreenSize().X / 2) / _worldScale);
            _spacePositionOffset.Y = -PlayerShip.SpacePosition.Y + ((_game.GetScreenSize().Y / 2) / _worldScale);

            removeNullEntities();

            foreach (AbstractSpaceEntity entity in _entities)
            {
                entity.Draw(spriteBatch, _spacePositionOffset, _worldScale);
            }

            
        }

        public void Update(GameTime gameTime)
        {
            removeNullEntities();

            foreach (AbstractSpaceEntity entity in _entities)
            {
                entity.Update(gameTime);
            }
        }


        private void removeNullEntities()
        {
            _entities.RemoveAll(item => item == null);
        }

    }
}
