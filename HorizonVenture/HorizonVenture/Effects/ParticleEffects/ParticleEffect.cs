using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmittor;
using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects
{
    public class ParticleEffect : Effect
    {
        protected List<ParticlesEmmitor> _emmitors;

        public ParticleEffect(Vector2 spacePosition, float angle, HorizonVentureSpace horizonVentureSpace)
            : base(spacePosition, angle, horizonVentureSpace)
        {
            _emmitors = new List<ParticlesEmmitor>();
        }



        public override void Update(GameTime gameTime)
        {
            _emmitors.ForEach(e => e.Update(gameTime));

        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            _emmitors.ForEach(e => e.Draw(spriteBatch, spacePositionOffset, scale));
        }
    }
}
