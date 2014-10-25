using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.PartilesAppearance
{
    class LinearChangeParticleAppearance : IParticlesAppearance
    {

        public float BeginZoom { get; set; }
        public float EndZoom { get; set; }

        public float BeginTextureAngle { get; set; }
        public float EndTextureAngle { get; set; }

        public Color BeginDrawColor { get; set; }
        public Color EndDrawColor { get; set; }

        public LinearChangeParticleAppearance()
        {
            BeginZoom = 1;
            EndZoom = 1;

            BeginTextureAngle = 1;
            EndTextureAngle = 1;

            BeginDrawColor = Color.White;
            EndDrawColor = Color.White;
        }

        public void UpdateParticleAppearance(Particle particle)
        {
            float partLive = particle.ElapsedLiveTime / particle.TimeToLive;

            particle.Zoom = BeginZoom + ((EndZoom - BeginZoom) * partLive);

            particle.TextureAngle = BeginTextureAngle + ((EndTextureAngle - BeginTextureAngle) * partLive);

            particle.SetDrawColor
                (
                (byte)(BeginDrawColor.A + ((EndDrawColor.A - BeginDrawColor.A) * partLive)),
                (byte)(BeginDrawColor.B + ((EndDrawColor.B - BeginDrawColor.B) * partLive)),
                (byte)(BeginDrawColor.G + ((EndDrawColor.G - BeginDrawColor.G) * partLive)),
                (byte)(BeginDrawColor.R + ((EndDrawColor.R - BeginDrawColor.R) * partLive))                
                );
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
