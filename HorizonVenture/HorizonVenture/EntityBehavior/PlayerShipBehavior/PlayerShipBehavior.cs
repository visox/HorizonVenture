using HorizonVenture.HorizonVenture.EntityComponents.EngineComponents;
using HorizonVenture.HorizonVenture.Space.SpaceEntities;
using HorizonVenture.HorizonVenture.Space.SpaceEntities.Ships;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.EntityBehavior.PlayerShipBehavior
{
    public class PlayerShipBehavior : AbstractBehavior
    {
        private bool _isActive;

        public PlayerShipBehavior(PlayerShip owner)
            :base(owner)
        {
            InputManager.OnMouseLeftKeyPress += MouseLeftKeyPress;

            _isActive = Owner.HorizonVentureSpace.getGame().InSpaceScreen.IsActive;

            Owner.HorizonVentureSpace.getGame().InSpaceScreen.OnInit += ActiveScreenChange;
            Owner.HorizonVentureSpace.getGame().InSpaceScreen.OnUnInit += ActiveScreenChange;
        }

        private void ActiveScreenChange(object sender, EventArgs e)
        {
            _isActive = Owner.HorizonVentureSpace.getGame().InSpaceScreen.IsActive;
        }



        

        private void MouseLeftKeyPress(object sender, InputManager.MouseKeyPressArgs e)
        {
            if (!_isActive)
                return;

            Vector2 toPos = Helper.GetScreenPosToSpacePosition(InputManager.MouseState.X,
                InputManager.MouseState.Y, Owner.HorizonVentureSpace);

            Owner.TurnToSpacePosition(toPos);
          


        }



        

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        
    }
}
