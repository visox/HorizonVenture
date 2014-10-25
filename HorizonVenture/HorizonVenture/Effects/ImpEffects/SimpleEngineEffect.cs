using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmitter;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesGenerator;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesMovement;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesSource;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.PartilesAppearance;
using HorizonVenture.HorizonVenture.EntityComponents.EngineComponents;
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
        public AbstractEngine Owner { get; protected set; }

        public SimpleEngineEffect(AbstractEngine owner, Vector2 offsetOwner,
            Vector2 spacePosition, float angle, HorizonVentureSpace horizonVentureSpace)
            : base(spacePosition, angle, horizonVentureSpace)
        {
            Owner = owner;

            Owner.OnPostDrawComponent += PostDrawOwner;
            Owner.OnPostUpdateSpaceComponent += PostUpdateOwner;

            Vector2 emitterPosition = new Vector2();
            emitterPosition.X = Owner.PositionOnEntity.X * BlocksHolder.SCALE_1_BLOCK_SIZE;
            emitterPosition.Y = Owner.PositionOnEntity.Y * BlocksHolder.SCALE_1_BLOCK_SIZE;

            emitterPosition.X += offsetOwner.X;
            emitterPosition.Y += offsetOwner.Y;

            StandartParticlesEmmitter spe = new StandartParticlesEmmitter(this)
            {
                OffsetOwner = emitterPosition
            };

            spe.ParticlesGenerator = new StaticParticleGenerator(spe,
                horizonVentureSpace.getGame().GetContent().Load<Texture2D>(@"Particles\particle"),
                10, 20)
                {
                    ParticleMaxTimeToLive = 10,
                    ParticleMinTimeToLive = 10
                };

            spe.ParticlesSource = new LineParticleSource(spe, new Vector2(-1.5f * BlocksHolder.SCALE_1_BLOCK_SIZE, 0)
                , new Vector2(1.5f * BlocksHolder.SCALE_1_BLOCK_SIZE, 0));

            spe.ParticlesMovement = new DirectionalAngleParticlesMovement(spe, 160, 200, 50, 20);

            spe.ParticlesAppearance = new LinearChangeParticleAppearance() 
            {
                BeginZoom = 0.9f,
                EndZoom = 0.3f,

                BeginDrawColor = Color.Yellow,
                EndDrawColor = Color.Red
                
            };

            this._emmitors.Add(spe);
        }

        private void PostUpdateOwner(object sender, UpdateArgs e)
        {
            Update(e.GameTime);
        }

        private void PostDrawOwner(object sender, DrawArgs e)
        {
            Draw(e.SpriteBatch, e.SpacePositionOffset, e.Scale);
        }
    }
}
