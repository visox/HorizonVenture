using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmitter;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesSource
{
    public class LineParticleSource : IParticlesSource
    {
        public ParticlesEmmitter Owner { get; protected set; }
        public Vector2 Begin { get; set; }
        public Vector2 End { get; set; }

        private Random _random;

        public LineParticleSource(ParticlesEmmitter owner, Vector2 begin, Vector2 end)
        {
            Owner = owner;
            Begin = begin;
            End = end;

            _random = new Random();
        }

        private Vector2 _actualBegin = new Vector2();
        private Vector2 _actualEnd = new Vector2();

        public void SetNewParticlePosition(Particle particle)
        {


            float posx = _random.Next(_actualBegin.X, _actualEnd.X);
            float posy = _random.Next(_actualBegin.Y, _actualEnd.Y);

            particle.SpacePosition = new Vector2(posx, posy);
        }

        public void Update(GameTime gameTime)
        {
            _actualBegin.X = Begin.X + Owner.OffsetOwner.X + Owner.Owner.SpacePosition.X;
            _actualEnd.X = End.X + Owner.OffsetOwner.X + Owner.Owner.SpacePosition.X;

            _actualBegin.Y = Begin.Y + Owner.OffsetOwner.Y + Owner.Owner.SpacePosition.Y;
            _actualEnd.Y = End.Y + Owner.OffsetOwner.Y + Owner.Owner.SpacePosition.Y;
        }
    }
}
