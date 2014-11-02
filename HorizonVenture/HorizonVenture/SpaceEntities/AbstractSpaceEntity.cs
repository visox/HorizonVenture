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
            set { _angle = value;  }
        }

        public float AngleSpeed { get; set; }

        private Vector2 _speed;

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private Vector2 _acceleration;

        public Vector2 Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }


        public DrawHandler OnPreDrawSpaceEntity;
        public DrawHandler OnPostDrawSpaceEntity;

        public UpdateHandler OnPreUpdateSpaceEntity;
        public UpdateHandler OnPostUpdateSpaceEntity;

        protected AbstractSpaceEntity(HorizonVentureSpace horizonVentureSpace, Vector2 spacePosition)
        {
            SpacePosition = spacePosition;
            HorizonVentureSpace = horizonVentureSpace;
            EntityComponents = new List<AbstractEntityComponent>();
            _color = Color.White;
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
            

           // UpdateAngleSpeedByTargetAngle(gameTime);
         //   UpdateSpeedByTargetPosition(gameTime);

            UpdateVectorSpeedDown(gameTime);
            UpdateAngleSpeedDown(gameTime);

            UpdatePositionAngle(gameTime);

            ResetAngleSpeed();
            ResetAcceleration();
        }

        protected void ResetAngleSpeed()
        {
            AngleSpeed = 0;
        }

        protected void ResetAcceleration()
        {
            _acceleration.X = 0;
            _acceleration.Y = 0;
        }

        protected void UpdatePositionAngle(GameTime gameTime)
        {
            _angle += (AngleSpeed * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
            if (_angle < 0)
                _angle += 360;
            else if (_angle >= 360)
                _angle -= 360;

         

            _speed.X += (Acceleration.X  * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
            _speed.Y += (Acceleration.Y * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;

            _spacePosition.X += (Speed.X * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
            _spacePosition.Y += (Speed.Y * gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
        }

      //  private static readonly float VECTOR_SPEED_DOWN = 100;
        private static readonly float ACCELERATION_DOWN = 1;
        private static readonly float RESET_VECTOR_SPEED = 0.05f;

        private Vector2 _speedNormalized = new Vector2();
        protected void UpdateVectorSpeedDown(GameTime gameTime)
        {

            //if (Speed.Length().Equals(0))
            //    return;

            float enginesPower = AbstractEngine.GetSumEnginePower(GetEngines());

            float weigth = GetEntityWeight();
            float acc = Helper.GetVectorSpeed(enginesPower, weigth);


            if (_acceleration.Length() == 0 && _speed.Length() <= RESET_VECTOR_SPEED * enginesPower)
            {
                _speed.X = 0;
                _speed.Y = 0;
            }

            if (Speed.Length() == 0)
            {
                return;
            }
            else
            {
                _speedNormalized = _speed;

                _speedNormalized.Normalize();                

                _acceleration.X -= (_speedNormalized.X * (float)Math.Pow(_speed.Length() / 20, 2));
                _acceleration.Y -= (_speedNormalized.Y * (float)Math.Pow(_speed.Length() / 20, 2));
            }

            if (Acceleration.Length() < ACCELERATION_DOWN)
            {
                _acceleration.X = 0;
                _acceleration.Y = 0;

                //return;
            }
        }



        private static readonly float ANGLE_SPEED_DOWN = 10;

        protected void UpdateAngleSpeedDown(GameTime gameTime)
        {
            if (AngleSpeed.Equals(0))
                return;

         /*   if (Math.Abs(AngleSpeed) < ANGLE_SPEED_DOWN)
            {
                AngleSpeed = 0;
                return;
            }*/

            AngleSpeed -= AngleSpeed > 0 ? ANGLE_SPEED_DOWN : -ANGLE_SPEED_DOWN;
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

        private void UpdateSpeedByTargetPosition(Vector2 targetSpacePosition)
        {
            if (targetSpacePosition.Equals(SpacePosition))
                return;

            Vector2 difVector = (targetSpacePosition - SpacePosition);

            float dif = difVector.Length();

            float angleDifference = GetAngleDifToTargetAngle(Helper.VectorToAngle(difVector));

            angleDifference = Math.Abs(angleDifference);

            float enginesPower = AbstractEngine.GetSumEnginePower(GetEngines());

            float weigth = GetEntityWeight();            

            difVector.Normalize();

            float addSpeedX = 0;
            float addSpeedY = 0;

            if (angleDifference < 90)
            {
                addSpeedX = difVector.X * Helper.GetVectorSpeed(enginesPower, weigth)
                    * ((90.0f - angleDifference) / 90.0f);
                addSpeedY = difVector.Y * Helper.GetVectorSpeed(enginesPower, weigth)
                    * ((90.0f - angleDifference) / 90.0f);
            }

            _acceleration.X += addSpeedX;
            _acceleration.Y += addSpeedY;
        }

        private float GetAngleDifToTargetAngle(float targetAngle)
        {
            if (targetAngle > Angle)
            {
                if (targetAngle - Angle < 180)
                {
                    return targetAngle - Angle;
                }
                else
                {
                    return (targetAngle - 360) - Angle;
                }
            }
            else
            {
                if (Angle - targetAngle < 180)
                {
                    return targetAngle - Angle;
                }
                else
                {
                    return (360 - Angle) + targetAngle;
                }
            }
        }


        private void UpdateAngleSpeedByTargetAngle(float targetAngle)
        {
            if (targetAngle.Equals(_angle))
                return;

            float angleDifference = GetAngleDifToTargetAngle(targetAngle);            

            if (Math.Abs(angleDifference) < ANGLE_SPEED_DOWN / 10.0f)
            {
                _angle = targetAngle;
                return;
            }

            float enginesPower = AbstractEngine.GetSumEnginePower(GetEngines());

            float weigth = GetEntityWeight();

            float toAddAngleSpeed = (Helper.GetAngleSpeedAcceleration(enginesPower, weigth));

            if (Math.Abs(toAddAngleSpeed / 50.0f) > Math.Abs(angleDifference))
            {
                _angle = targetAngle;
                return;
            }

            if (angleDifference < 0)
            {
                AngleSpeed += toAddAngleSpeed * -1;
            }
            else
            {
                AngleSpeed += toAddAngleSpeed;
            }

            
        }

       // private float _targetAngle;

        public void TurnToSpacePosition(Vector2 to)
        {
            Vector2 direction = to - SpacePosition;

            UpdateAngleSpeedByTargetAngle(Helper.VectorToAngle(direction));
          //  _targetAngle = Helper.VectorToAngle(direction);
        }

      //  private Vector2 _targetSpacePosition = new Vector2();

        public void GoToSpacePosition(Vector2 to)
        {
            Vector2 difference = to - SpacePosition;

            TurnToSpacePosition(to);

            UpdateSpeedByTargetPosition(to);

           // _targetSpacePosition.X = to.X;
          //  _targetSpacePosition.Y = to.Y;
        }

    }
}
