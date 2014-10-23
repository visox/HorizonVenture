using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmittor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesGenerator
{
    class StaticParticleGenerator: IParticlesGenerator
    {
        public ParticlesEmmitor Owner { get; protected set; }
        public Texture2D ParticleTexture { get; set; }
        public Color ParticleDrawColor { get; set; }
        public float ParticleMaxTimeToLive { get; set; }
        public float ParticleMinTimeToLive { get; set; }

        public int MinParticlesCountPerSecond {get; set;}
        public int MaxParticlesCountPerSecond {get; set;}

        private int _lastElapsedMilliseconds = 0;
        private int _nextParticlesRelease = 0;
        private int _particlesReleased = 0;

        private int _currentSecond = -1;

        private Random _random;

        public StaticParticleGenerator(ParticlesEmmitor owner, Texture2D particleTexture,
            int minParticlesCountPerSecond, int maxParticlesCountPerSecond)
        {
            Owner = owner;
            ParticleTexture = particleTexture;

            MinParticlesCountPerSecond = minParticlesCountPerSecond;
            MaxParticlesCountPerSecond = maxParticlesCountPerSecond;

            ParticleDrawColor = Color.White;
            ParticleMaxTimeToLive = 1;
            ParticleMinTimeToLive = 1;

            _random = new Random();
        }

        public List<Particle> GetNextParticles()
        {
            float secondPart = (_lastElapsedMilliseconds / 1000f);
            int shouldBeReleased = (int)Math.Ceiling(_nextParticlesRelease * secondPart);


            List<Particle> result = new List<Particle>();

            while (_particlesReleased < shouldBeReleased)
            {
                Particle toAdd = new Particle(Owner, new Vector2(0, 0), ParticleTexture,
                    ParticleMinTimeToLive + ((ParticleMaxTimeToLive - ParticleMinTimeToLive) * (float)_random.NextDouble()));

                result.Add(toAdd);

                _particlesReleased++;
            }


            return result;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_currentSecond != gameTime.ElapsedGameTime.Seconds)
            {
                _currentSecond = gameTime.ElapsedGameTime.Seconds;

                _nextParticlesRelease = _random.Next(MinParticlesCountPerSecond, MaxParticlesCountPerSecond);
                _particlesReleased = 0;

                _lastElapsedMilliseconds = 0;
            }
            _lastElapsedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
