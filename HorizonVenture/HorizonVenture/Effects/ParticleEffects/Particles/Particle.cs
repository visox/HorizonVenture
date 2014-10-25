using HorizonVenture.HorizonVenture.Effects.ParticleEffects.ParticlesEmitter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Effects.ParticleEffects.Particles
{
    public class Particle
    {
        public ParticlesEmmitter Owner { get; protected set; }

        protected Vector2 _spacePosition;
        public Vector2 SpacePosition
        {
            get { return _spacePosition; }
            set
            {
                _spacePosition = value;
            }
        }
        public Texture2D Texture { get; set; }

        private Color _drawColor;
        public Color DrawColor
        {
            get { return _drawColor; }
            set { _drawColor = value; }
        }

        private Vector2 _speed;

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        
        public float ElapsedLiveTime { get; set; }
        public float TimeToLive { get; set; }
        public float TextureAngle { get; set; }
        public Vector2 TextureDrawOrigin{ get; set; }
        public float Zoom { get; set; }

        public Particle(ParticlesEmmitter owner, Vector2 position, Texture2D texture, float timeToLive)
        {
            Owner = owner;
            SpacePosition = position;
            Texture = texture;
            TimeToLive = timeToLive;

            DrawColor = Color.White;
            Speed = new Vector2(0,0);
            ElapsedLiveTime = 0;
            TextureAngle = 0;
            Zoom = 1;

            TextureDrawOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public void SetDrawColor(byte a, byte b, byte g, byte r)
        {
            _drawColor.A = a;
            _drawColor.B = b;
            _drawColor.G = g;
            _drawColor.R = r;
            
        }

        public void SetSpeed(float x, float y)
        {
            _speed.X = x;
            _speed.Y = y;
        }

        public void Update(GameTime gameTime)
        {
            if (Remove())
                return;

            UpdateLiveTime(gameTime);
            UpdatePositionBySpeed(gameTime);

        }

        protected void UpdatePositionBySpeed(GameTime gameTime)
        {
            _spacePosition.X += (gameTime.ElapsedGameTime.Milliseconds * Speed.X) / 1000.0f;
            _spacePosition.Y += (gameTime.ElapsedGameTime.Milliseconds * Speed.Y) / 1000.0f;
        }

        public void NormalizeSpeed()
        {
            _speed.Normalize();
        }


        public bool Remove()
        {
            return ElapsedLiveTime >= TimeToLive;
        }

        protected void UpdateLiveTime(GameTime gameTime)
        {
            ElapsedLiveTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            if (Remove())
                return;

            spriteBatch.Draw(Texture, GetDrawPosition(spacePositionOffset, scale), null,
                DrawColor, TextureAngle, TextureDrawOrigin, scale * Zoom, SpriteEffects.None, 0);
        }

        protected Vector2 _drawPosition = new Vector2();

        protected Vector2 GetDrawPosition(Vector2 spacePositionOffset, float scale)
        {
            _drawPosition.X = SpacePosition.X * scale;
            _drawPosition.Y = SpacePosition.Y * scale;

            _drawPosition.X += spacePositionOffset.X * scale;
            _drawPosition.Y += spacePositionOffset.Y * scale;

            return _drawPosition;
        }
    }
}
