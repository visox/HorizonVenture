using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmittor;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesSource
{
    public class RectangleParticleSource : IParticlesSource
    {
        public ParticlesEmmitor Owner { get; protected set; }
        public Rectangle Source { get; set; }

        private Random _random;

        public RectangleParticleSource(ParticlesEmmitor owner, Rectangle source)
        {
            Owner = owner;
            Source = source;


            _random = new Random();
        }

        public void SetNewParticlePosition(Particle particle)
        {
            float posx = _random.Next(Source.Left, Source.Right) + Owner.OffsetOwner.X + Owner.Owner.SpacePosition.X;
            float posy = _random.Next(Source.Top, Source.Bottom) + Owner.OffsetOwner.Y + Owner.Owner.SpacePosition.Y;

            particle.SpacePosition = new Vector2(posx, posy);
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
