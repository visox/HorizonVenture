using HorizonVenture.HorizonVenture.Blocks;
using HorizonVenture.HorizonVenture.Draw;
using HorizonVenture.HorizonVenture.EntityBehavior;
using HorizonVenture.HorizonVenture.EntityComponents;
using HorizonVenture.HorizonVenture.EntityComponents.EngineComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HorizonVenture.HorizonVenture.Space.SpaceEntities
{
    public abstract class AbstractSpaceEntity : ISpaceDrawable
    {
        public BlocksHolder BlocksHolder { get; protected set; }
        protected Color _color;

        public AbstractBehavior Behavior { get; protected set; }

        public List<AbstractEntityComponent> EntityComponents { get; protected set; }

        private Vector2 _spacePosition;

        public Vector2 SpacePosition
        {
            get { return _spacePosition; }
            set { _spacePosition = value; }
        }

        public HorizonVentureSpace HorizonVentureSpace { get; protected set; }

        private float _angle;

        public float Angle
        {
            get { return _angle; }
            set { _angle = value; _targetAngle = value; }
        }
        
        public float AngleSpeed { get; set; }

        private Vector2 _speed;

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        public DrawHandler OnPreDrawSpaceEntity;
        public DrawHandler OnPostDrawSpaceEntity;

        public UpdateHandler OnPreUpdateSpaceEntity;
        public UpdateHandler OnPostUpdateSpaceEntity;

        private static readonly float MINIMAL_DIST_TO_REACT = 50F;
        private static readonly float MINIMAL_ANGLE_DIFFERENCE = 5F;

        private float _currentMinimalDistToTargetPoint;
        private float _currentMinimalAngleToTargetPoint;

        protected AbstractSpaceEntity(HorizonVentureSpace horizonVentureSpace, Vector2 spacePosition)
        {
            SpacePosition = spacePosition;
            HorizonVentureSpace = horizonVentureSpace;
            EntityComponents = new List<AbstractEntityComponent>();
            _color = Color.White;

            _currentMinimalDistToTargetPoint = MINIMAL_DIST_TO_REACT;
            _currentMinimalAngleToTargetPoint = MINIMAL_ANGLE_DIFFERENCE;
        }

        Vector2 _drawPosition = new Vector2(0, 0);

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {

            if (OnPreDrawSpaceEntity != null)
            {
                OnPreDrawSpaceEntity(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }

            InnerDraw(spriteBatch, spacePositionOffset, scale);

            if (OnPostDrawSpaceEntity != null)
            {
                OnPostDrawSpaceEntity(this, new DrawArgs(spriteBatch, spacePositionOffset, scale));
            }
        }

        protected virtual void InnerDraw(SpriteBatch spriteBatch, Vector2 spacePositionOffset, float scale)
        {
            BlocksHolder.Draw(spriteBatch, GetDrawPosition(spacePositionOffset, scale),
                           BlocksHolder.GetCenter(), Angle, _color, scale);
        }


        protected Vector2 GetDrawPosition(Vector2 spacePositionOffset, float scale)
        {
            _drawPosition.X = SpacePosition.X * scale;
            _drawPosition.Y = SpacePosition.Y * scale;

            _drawPosition.X += spacePositionOffset.X * scale;
            _drawPosition.Y += spacePositionOffset.Y * scale;

            return _drawPosition;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (OnPreUpdateSpaceEntity != null)
            {
                OnPreUpdateSpaceEntity(this, new UpdateArgs(gameTime));
            }

            InnerUpdate(gameTime);

            if (OnPostUpdateSpaceEntity != null)
            {
                OnPostUpdateSpaceEntity(this, new UpdateArgs(gameTime));
            }
        }

        protected virtual void InnerUpdate(GameTime gameTime)
        {
            UpdatePositionAngle(gameTime);
            UpdateAngleSpeed(gameTime);
        }

        protected void UpdatePositionAngle(GameTime gameTime)
        {
            _angle += (AngleSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;

            _spacePosition.X += Speed.X * gameTime.ElapsedGameTime.Milliseconds;
            _spacePosition.Y += Speed.Y * gameTime.ElapsedGameTime.Milliseconds;
        }

        List<AbstractEngine> _engines = new List<AbstractEngine>();
        public List<AbstractEngine> GetEngines()
        {
            _engines.Clear();

            _engines.AddRange(EntityComponents.Where(c => c is AbstractEngine).Cast<AbstractEngine>());

            return _engines;
        }

        public virtual float GetEntityWeight()
        {
            return BlocksHolder.GetBlocksCount();
        }


        private void UpdateAngleSpeed(GameTime gameTime)
        {
            if (_targetAngle.Equals(_angle))
                return;

            float angleDifference;

            if (Math.Abs(_targetAngle - Angle) < 180)
            {
                angleDifference = _targetAngle - Angle;
            }
            else
            {
                angleDifference = _targetAngle - (Angle - 360);
            }

            float enginesPower = AbstractEngine.GetSumEnginePower(GetEngines());

            float weigth = GetEntityWeight();

            if (Math.Abs(angleDifference) > _currentMinimalAngleToTargetPoint)
            {
                AngleSpeed = Helper.GetAngleSpeed(enginesPower, weigth);


            }
            else
            {
                AngleSpeed = Helper.GetAngleSpeed(enginesPower, weigth) *
                        _currentMinimalAngleToTargetPoint / (Math.Abs(angleDifference));

                if (Math.Abs(angleDifference) < (AngleSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f)
                {
                    AngleSpeed = 0;
                }
            }

            if (angleDifference < 0)
            {
                AngleSpeed *= -1;
            }
        }

        private float _targetAngle;

        public void TurnToSpacePosition(Vector2 to)
        {
            Vector2 direction = to - SpacePosition;

            if (direction.Length() < _currentMinimalDistToTargetPoint)
                return;

            _targetAngle = Helper.VectorToAngle(direction);

        }

    }
}
