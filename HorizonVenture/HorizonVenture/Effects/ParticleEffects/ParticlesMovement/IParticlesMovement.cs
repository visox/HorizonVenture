﻿using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesMovement
{
    public interface IParticlesMovement
    {
        void UpdateParticleMovement(Particle particle);
        void Update(GameTime gameTime);
    }
}
