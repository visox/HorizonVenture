using HorizonVenture.HorizonVenture.Effects.ParticleEffects;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmittor;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesGenerator;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesMovement;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesSource;
using HorizonVenture.HorizonVenture.Space;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ImpEffects
{
    public class SimpleEngineEffect : ParticleEffect
    {

        public SimpleEngineEffect(Vector2 spacePosition, float angle, HorizonVentureSpace horizonVentureSpace)
            : base(spacePosition, angle, horizonVentureSpace)
        {
            StandartParticlesEmmiter spe = new StandartParticlesEmmiter(this);

            spe.ParticlesGenerator = new StaticParticleGenerator(spe,
                horizonVentureSpace.getGame().GetContent().Load<Texture2D>(@"Particles\particle"),
                10, 20) 
                { ParticleMaxTimeToLive = 10,
                    ParticleMinTimeToLive = 10
            };

            spe.ParticlesSource = new RectangleParticleSource(spe, new Rectangle(-10, -10, 20, 20));

            spe.ParticlesMovement = new DirectionalAngleParticlesMovement(spe, 160, 200, 50, 20);

            this._emmitors.Add(spe);
        }
    }
}
