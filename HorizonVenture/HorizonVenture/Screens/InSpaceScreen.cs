using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Screens
{
    class InSpaceScreen : AbstractScreen
    {

        public HorizonVentureSpace HorizonVentureSpace { get; set; }

        public InSpaceScreen(HorizonVentureGame game)
            : base(game)
        {
            _backgroundColor = Color.Black;
        }

        protected override void Init()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackgroundColor();

            if (HorizonVentureSpace != null)
                HorizonVentureSpace.Draw(spriteBatch);

           

            DrawControls(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HorizonVentureSpace != null)
                HorizonVentureSpace.Update(gameTime);
        }
    }
}
