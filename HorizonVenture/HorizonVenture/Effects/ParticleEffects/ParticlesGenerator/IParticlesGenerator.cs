using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesGenerator
{
    public interface IParticlesGenerator
    {
        List<Particle> GetNextParticles();
        void Update(GameTime gameTime);
    }
}
