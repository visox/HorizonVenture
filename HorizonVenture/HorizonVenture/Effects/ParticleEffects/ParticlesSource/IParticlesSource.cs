using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesSource
{
    public interface IParticlesSource
    {
        void SetNewParticlePosition(Particle particle);
        void Update(GameTime gameTime);
    }
}
