using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesGenerator;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesMovement;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesSource;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.PartilesAppearance;
using HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HorizonVenture.HorizonVenture.EntityComponents;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmitter
{
    public abstract class ParticlesEmmitter
    {
        public IParticlesSource ParticlesSource { get; set; }
        public IParticlesGenerator ParticlesGenerator { get; set; }
        public IParticlesMovement ParticlesMovement { get; set; }
        public IParticlesAppearance ParticlesAppearance { get; set; }

        public ParticleEffect Owner { get; protected set; }
        public List<Particle> Particles { get; protected set; }
        public Vector2 SpacePosition { get; set; }

        public ParticlesEmmitter(ParticleEffect owner)
        {
            Owner = owner;
            Particles = new List<Particle>();

            SpacePosition = Owner.SpacePosition;
        }

        public void AdjustSpacePosition(AbstractEntityComponent aec, Vector2 offset, float angle)
        {
            SpacePosition = new Vector2(aec.GetRealZeroAngleSpacePosition().X + offset.X,
                aec.GetRealZeroAngleSpacePosition().Y + offset.Y);

            if (angle != 0)
            {
                SpacePosition = Helper.RotateAroundOrigin(SpacePosition, aec.Owner.SpacePosition, angle);
            }
        }


        public virtual void Update(GameTime gameTime)
        {
            UpdateParticles(gameTime);

            UpdateManagerParticlesSource(gameTime);
            UpdateManagerParticlesGenerator(gameTime);
            UpdateManagerParticlesMovement(gameTime);
            UpdateManagerParticlesAppearance(gameTime);

            AddNewParticles();
            UpdateParticlesPosition();
            UpdateParticlesAppearance();
            RemoveOldParticles();
        }

        private void UpdateParticlesAppearance()
        {
            if (ParticlesAppearance != null)
            {
                Particles.ForEach(p => ParticlesAppearance.UpdateParticleAppearance(p));
            }
        } 

        private void UpdateParticlesPosition()
        {
            if(ParticlesMovement != null)
            {
                Particles.ForEach(p => ParticlesMovement.UpdateParticleMovement(p));
            }
        }

        private void AddNewParticles()
        {
            List<Particle> toAdd = ParticlesGenerator.GetNextParticles();

            if (ParticlesSource != null)
            {
                toAdd.ForEach(p => ParticlesSource.SetNewParticlePosition(p));
            }

            Particles.AddRange(toAdd);
        }

        private void UpdateManagerParticlesGenerator(GameTime gameTime)
        {
            if (ParticlesGenerator != null)
            {
                ParticlesGenerator.Update(gameTime);
            }
        }

        private void UpdateManagerParticlesAppearance(GameTime gameTime)
        {
            if (ParticlesAppearance != null)
            {
                ParticlesAppearance.Update(gameTime);
            }
        }        

        private void UpdateManagerParticlesMovement(GameTime gameTime)
        {
            if (ParticlesMovement != null)
            {
                ParticlesMovement.Update(gameTime);
            }
        }

        

        private void UpdateManagerParticlesSource(GameTime gameTime)
        {
            if (ParticlesSource != null)
            {
                ParticlesSource.Update(gameTime);
            }
        }

        private void RemoveOldParticles()
        {
            Particles.RemoveAll(p => p.Remove());
        }

        protected void UpdateParticles(GameTime gameTime)
        {
            Particles.ForEach(p => p.Update(gameTime));

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            Particles.ForEach(p => p.Draw(spriteBatch, spacePositionOffset, scale));
        }
    }
}
