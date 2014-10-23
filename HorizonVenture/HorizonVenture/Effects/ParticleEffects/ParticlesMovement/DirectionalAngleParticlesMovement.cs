using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmittor;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesMovement
{
    class DirectionalAngleParticlesMovement : IParticlesMovement
    {
        public ParticlesEmmitor Owner { get; protected set; }
        public float MinOffsetAngle { get; set; }
        public float MaxOffsetAngle { get; set; }

        public float BeginSpeedPerSecond { get; set; }
        public float EndSpeedPerSecond { get; set; }

        private Random _random;

        public DirectionalAngleParticlesMovement(ParticlesEmmitor owner, float minOffsetAngle,
        float maxOffsetAngle, float beginSpeedPerSecond, float endSpeedPerSecond)
        {
            Owner = owner;
            MinOffsetAngle = minOffsetAngle;
            MaxOffsetAngle = maxOffsetAngle;
            BeginSpeedPerSecond = beginSpeedPerSecond;
            EndSpeedPerSecond = endSpeedPerSecond;

            _random = new Random();
        }

        public void UpdateParticleMovement(Particle particle)
        {
            if (particle.Speed.Length() == 0)
            {
                float angle = _random.Next((int)MinOffsetAngle, (int)MaxOffsetAngle) + Owner.Owner.Angle;

                particle.Speed = Helper.AngleToVector(MathHelper.ToRadians(angle));                 
            }

            particle.NormalizeSpeed();

            float curSpeed = BeginSpeedPerSecond +
                ((EndSpeedPerSecond - BeginSpeedPerSecond) * (particle.ElapsedLiveTime / particle.TimeToLive));

            particle.SetSpeed(particle.Speed.X * curSpeed, particle.Speed.Y * curSpeed);
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
