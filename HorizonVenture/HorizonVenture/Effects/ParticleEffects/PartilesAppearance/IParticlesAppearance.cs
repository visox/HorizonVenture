using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.PartilesAppearance
{
    public interface IParticlesAppearance
    {
        void UpdateParticleAppearance(Particle particle);
        void Update(GameTime gameTime);
    }
}
