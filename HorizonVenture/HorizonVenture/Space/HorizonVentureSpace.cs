using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Space
{
    class HorizonVentureSpace
    {
        Vector2 _spacePositionOffset;
        float _worldScale;
        HorizonVentureGame _game;

        PlayerShip playerShip;
        List<AbstractSpaceEntity> _entities;

        public HorizonVentureSpace(HorizonVentureGame game) 
        {
            _game = game;

            _spacePositionOffset = new Vector2();
            _worldScale = 0.25f;

            _entities = new List<AbstractSpaceEntity>();

            playerShip = new PlayerShip(this, new Vector2(0, 0));
            _entities.Add(playerShip);
            
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
            _spacePositionOffset.X = -playerShip.SpacePosition.X + (_game.GetScreenSize().X / 2);
            _spacePositionOffset.Y = -playerShip.SpacePosition.Y + (_game.GetScreenSize().Y / 2);

            foreach (AbstractSpaceEntity entity in this._entities)
            {
                entity.Draw(spriteBatch, _spacePositionOffset, _worldScale);
            }

            
        }

        public void Update(GameTime gameTime)
        {
            foreach (AbstractSpaceEntity entity in this._entities)
            {
                entity.Update(gameTime);
            }
        }

    }
}
