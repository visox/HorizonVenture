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
        public Vector2 Offset { get; set; }

        private DirectionalAngleParticlesMovement _dapm;
        private LineParticleSource _lps;

        private static readonly float HALF_ANGLE_PARTICLE_SPREAD = 20f;

        public SimpleEngineEffect(AbstractEngine owner, Vector2 offsetOwner,
            Vector2 spacePosition, float angle, HorizonVentureSpace horizonVentureSpace)
            : base(spacePosition, angle, horizonVentureSpace)
        {
            Owner = owner;

            Owner.OnPostDrawComponent += PostDrawOwner;
            Owner.OnPostUpdateSpaceComponent += PostUpdateOwner;
            Offset = offsetOwner;

            StandartParticlesEmmitter spe = new StandartParticlesEmmitter(this);

            spe.ParticlesGenerator = new StaticParticleGenerator(spe,
                horizonVentureSpace.getGame().GetContent().Load<Texture2D>(@"Particles\particle"),
                10, 20)
                {
                    ParticleMaxTimeToLive = 10,
                    ParticleMinTimeToLive = 10
                };

            _lps = new LineParticleSource(spe, new Vector2(-1.5f * BlocksHolder.SCALE_1_BLOCK_SIZE, 0)
                , new Vector2(1.5f * BlocksHolder.SCALE_1_BLOCK_SIZE, 0));

            spe.ParticlesSource = _lps;

            _dapm = new DirectionalAngleParticlesMovement(spe)
            {
                BeginSpeedPerSecond = 50,
                EndSpeedPerSecond = 20,

                MaxOffsetAngle = Owner.Owner.Angle + 180 + HALF_ANGLE_PARTICLE_SPREAD,
                MinOffsetAngle = Owner.Owner.Angle + 180 - HALF_ANGLE_PARTICLE_SPREAD
            };

            spe.ParticlesMovement = _dapm;

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            AdjustEmitterSpacePosition();
            AdjustEmitterParticleDirection();
            AndjustSourceAngle();
        }

        private void AdjustEmitterSpacePosition()
        {
            this._emmitors.ForEach(e => e.AdjustSpacePosition(Owner, Offset, Owner.Owner.Angle));
        }

        private void AndjustSourceAngle()
        {
            _lps.Angle = Owner.Owner.Angle;
        }

        private void AdjustEmitterParticleDirection()
        {
            _dapm.MaxOffsetAngle = Owner.Owner.Angle + 180 + HALF_ANGLE_PARTICLE_SPREAD;
            _dapm.MinOffsetAngle = Owner.Owner.Angle + 180 - HALF_ANGLE_PARTICLE_SPREAD;
        }
    }
}
