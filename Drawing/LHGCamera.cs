
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

using LunchHourGames.Common;

namespace LunchHourGames.Drawing
{
    public enum LHGCameraMode
    {
        /// <summary>
        /// A totally free-look arcball that orbits relative
        /// to its orientation
        /// </summary>
        Free = 0,

        /// <summary>
        /// A camera constrained by roll so that orbits only
        /// occur on latitude and longitude
        /// </summary>
        RollConstrained = 1
    }

    public class LHGCamera
    {       
        /// <summary>
        /// Uses a pair of keys to simulate a positive or negative axis input.
        /// </summary>
        public static float ReadKeyboardAxis(KeyboardState keyState, Keys downKey, Keys upKey)
        {
            float value = 0;

            if (keyState.IsKeyDown(downKey))
                value -= 1.0f;

            if (keyState.IsKeyDown(upKey))
                value += 1.0f;

            return value;
        }

        /// <summary>
        /// The location of the look-at target
        /// </summary>
        private Vector3 targetValue;

        /// <summary>
        /// The distance between the camera and the target
        /// </summary>
        private float distanceValue;

        /// <summary>
        /// The orientation of the camera relative to the target
        /// </summary>
        private Quaternion orientation;

        private float inputDistanceRateValue = 100.0f;
        private float shiftedDistanceRate = 20.0f;
        private float InputTurnRate = 3.0f;
        private float shiftedTurnRate = 0.25f;
        private LHGCameraMode mode;
        private float yaw, pitch, upAngle, rightAngle;

        float scrollSpeed;

        public LHGCamera(LHGCameraMode controlMode)
        {
            //orientation quaternion assumes a Pi rotation so you're facing the "front"
            //of the model (looking down the +Z axis)
            orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi);

            mode = controlMode;
            yaw = MathHelper.Pi;
            pitch = 0;
            scrollSpeed = 20.0f;  // 20.0f;  // 20.0f;  // 10.0f slow  20.0f normal 40.0f fast
        }
		
		public LHGCamera(LHGCameraMode controlMode, Vector3 target, Vector3 position, float distance, float angle)
		    : this(controlMode)
		{
		    OrbitUp(angle);
            Target = target;
            Position = position;
            Distance = distance;
		}

        public Vector3 Direction
        {
            get
            {
                //R v R' where v = (0,0,-1,0)
                //The equation can be reduced because we know the following things:
                //  1.  We're using unit quaternions
                //  2.  The initial aspect does not change
                //The reduced form of the same equation follows
                Vector3 dir = Vector3.Zero;
                dir.X = -2.0f *
                    ((orientation.X * orientation.Z) + (orientation.W * orientation.Y));
                dir.Y = 2.0f *
                    ((orientation.W * orientation.X) - (orientation.Y * orientation.Z));
                dir.Z =
                    ((orientation.X * orientation.X) + (orientation.Y * orientation.Y)) -
                    ((orientation.Z * orientation.Z) + (orientation.W * orientation.W));
                Vector3.Normalize(ref dir, out dir);
                return dir;

            }
        }

        public Vector3 Right
        {
            get
            {
                //R v R' where v = (1,0,0,0)
                //The equation can be reduced because we know the following things:
                //  1.  We're using unit quaternions
                //  2.  The initial aspect does not change
                //The reduced form of the same equation follows
                Vector3 right = Vector3.Zero;
                right.X =
                    ((orientation.X * orientation.X) + (orientation.W * orientation.W)) -
                    ((orientation.Z * orientation.Z) + (orientation.Y * orientation.Y));
                right.Y = 2.0f *
                    ((orientation.X * orientation.Y) + (orientation.Z * orientation.W));
                right.Z = 2.0f *
                    ((orientation.X * orientation.Z) - (orientation.Y * orientation.W));

                return right;

            }
        }
    
        public Vector3 Up
        {
            get
            {
                //R v R' where v = (0,1,0,0)
                //The equation can be reduced because we know the following things:
                //  1.  We're using unit quaternions
                //  2.  The initial aspect does not change
                //The reduced form of the same equation follows
                Vector3 up = Vector3.Zero;
                up.X = 2.0f *
                    ((orientation.X * orientation.Y) - (orientation.Z * orientation.W));
                up.Y =
                    ((orientation.Y * orientation.Y) + (orientation.W * orientation.W)) -
                    ((orientation.Z * orientation.Z) + (orientation.X * orientation.X));
                up.Z = 2.0f *
                    ((orientation.Y * orientation.Z) + (orientation.X * orientation.W));
                return up;
            }
        }
      
        // The view matrix describes the camera's position, the world space point the camera is looking at, and the "up" vector 
        // for the camera. The up vector is typically the direction of the positive Y axis. The up vector in the view matrix 
        // basically ensures that the object will be drawn with the top edge of the screen facing toward the sky. 
        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(targetValue - (Direction * distanceValue), targetValue, Up);
            }
        }

        public LHGCameraMode ControlMode
        {
            get { return mode; }
            set
            {
                if (value != mode)
                {
                    mode = value;
                    SetCamera(targetValue - (Direction* distanceValue), targetValue, Vector3.Up);
                }
            }
        }
         
        public Vector3 Target
        {
            get { return targetValue; }
            set { targetValue = value; }
        }
   
        public float Distance
        {
            get
            { return distanceValue; }
            set
            { distanceValue = value; }
        }
   
        public float InputDistanceRate
        {
            get
            { return inputDistanceRateValue; }
            set
            { inputDistanceRateValue = value; }
        }
  
        public Vector3 Position
        {
            get { return targetValue - (Direction * Distance); }
            set { SetCamera(value, targetValue, Vector3.Up);   }
        }

        public float UpAngle
        {
            get { return this.upAngle; }
        }

        public float RightAngle
        {
            get { return this.rightAngle; }
        }

        public float Yaw
        {
            get { return yaw; }
        }

        public float Pitch
        {
            get { return pitch; }
        }

        public void OrbitUp(float angle)
        {
            this.upAngle = angle;

            switch (mode)
            {
                case LHGCameraMode.Free:
                    //rotate the aspect by the angle 
                    orientation = orientation *
                     Quaternion.CreateFromAxisAngle(Vector3.Right, -angle);

                    //normalize to reduce errors
                    Quaternion.Normalize(ref orientation, out orientation);
                    break;
                case LHGCameraMode.RollConstrained:
                    //update the yaw
                    pitch -= angle;

                    //constrain pitch to vertical to avoid confusion
                    pitch = MathHelper.Clamp(pitch, -(MathHelper.PiOver2) + .0001f,
                        (MathHelper.PiOver2) - .0001f);

                    //create a new aspect based on pitch and yaw
                    orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, -yaw) *
                        Quaternion.CreateFromAxisAngle(Vector3.Right, pitch);
                    break;
            }
        }
      
        public void OrbitRight(float angle)
        {
            this.rightAngle = angle;

            switch (mode)
            {
                case LHGCameraMode.Free:
                    //rotate the aspect by the angle 
                    orientation = orientation *
                        Quaternion.CreateFromAxisAngle(Vector3.Up, angle);

                    //normalize to reduce errors
                    Quaternion.Normalize(ref orientation, out orientation);
                    break;
                case LHGCameraMode.RollConstrained:
                    //update the yaw
                    yaw -= angle;

                    //float mod yaw to avoid eventual precision errors
                    //as we move away from 0
                    yaw = yaw % MathHelper.TwoPi;

                    //create a new aspect based on pitch and yaw
                    orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, -yaw) *
                        Quaternion.CreateFromAxisAngle(Vector3.Right, pitch);
                    
                    //normalize to reduce errors
                    Quaternion.Normalize(ref orientation, out orientation);
                    break;
            }
        }
    
        public void RotateClockwise(float angle)
        {
            switch (mode)
            {
                case LHGCameraMode.Free:
                    //rotate the orientation around the direction vector
                    orientation = orientation *
                        Quaternion.CreateFromAxisAngle(Vector3.Forward, angle);
                    Quaternion.Normalize(ref orientation, out orientation);
                    break;
                case LHGCameraMode.RollConstrained:
                    //Do nothing, we don't want to roll at all to stay consistent
                    break;
            }
        }
      
        public void SetCamera(Vector3 position, Vector3 target, Vector3 up)
        {
            //Create a look at matrix, to simplify matters a bit
            Matrix temp = Matrix.CreateLookAt(position, target, up);

            //invert the matrix, since we're determining the
            //orientation from the rotation matrix in RH coords
            temp = Matrix.Invert(temp);

            //set the postion
            targetValue = target;

            //create the new aspect from the look-at matrix
            orientation = Quaternion.CreateFromRotationMatrix(temp);

            //When setting a new eye-view direction 
            //in one of the gimble-locked modes, the yaw and
            //pitch gimble must be calculated.
            if (mode != LHGCameraMode.Free)
            {
                //first, get the direction projected on the x/z plne
                Vector3 dir = Direction;
                dir.Y = 0;
                if (dir.Length() == 0f)
                {
                    dir = Vector3.Forward;
                }
                dir.Normalize();

                //find the yaw of the direction on the x/z plane
                //and use the sign of the x-component since we have 360 degrees
                //of freedom
                yaw = (float)(Math.Acos(-dir.Z) * Math.Sign(dir.X));

                //Get the pitch from the angle formed by the Up vector and the 
                //the forward direction, then subtracting Pi / 2, since 
                //we pitch is zero at Forward, not Up.
                pitch = (float)-(Math.Acos(Vector3.Dot(Vector3.Up, Direction))
                    - MathHelper.PiOver2);
            }
        }
        
        public void HandleDefaultKeyboardControls(KeyboardState kbState, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float turnRate = InputTurnRate;
            float distanceRate = inputDistanceRateValue;

            if (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift))
            {
                turnRate = shiftedTurnRate;
                distanceRate = shiftedDistanceRate;
            }

            float dX = elapsedTime * ReadKeyboardAxis(kbState, Keys.A, Keys.D) * turnRate;
            float dY = elapsedTime * ReadKeyboardAxis(kbState, Keys.S, Keys.W) * turnRate;

            if (dY != 0)
                OrbitUp(dY);
            if (dX != 0) 
                OrbitRight(dX);

            distanceValue += ReadKeyboardAxis(kbState, Keys.Z, Keys.X) * inputDistanceRateValue * elapsedTime;
            if (distanceValue < .001f) 
                distanceValue = .001f;            

            if (mode != LHGCameraMode.Free)
            {
                float dR = elapsedTime * ReadKeyboardAxis(kbState, Keys.Q, Keys.E) * turnRate;
                if (dR != 0)
                    RotateClockwise(dR);
            }

            Vector3 tar = Target;
            tar.X += ReadKeyboardAxis(kbState, Keys.Left, Keys.Right) * turnRate;
            tar.Y += ReadKeyboardAxis(kbState, Keys.Down, Keys.Up) * turnRate;
            Target = tar;        
        }

        public void Zoom(int zoomInValue, int zoomOutValue, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            int value = 0;
            if (zoomInValue > zoomOutValue)
                value -= zoomInValue;

            if (zoomInValue < zoomOutValue)
                value += zoomOutValue;

            // Zoom
            distanceValue += value * inputDistanceRateValue * elapsedTime;
            if (distanceValue < .001f) 
                distanceValue = .001f;            
            
            /*
            +
            // Move across
            Vector3 currentTarget = Target;
            currentTarget.Z += value * inputDistanceRateValue * elapsedTime;
            Target = currentTarget;
             * */
        }

        public void ScrollLeftRight(int previousX, int currentX, GameTime gameTime)
        {
            if (previousX != currentX)
            {
                float elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

               // LHGCommon.printToConsole("elapsedTime", elapsedTime);

                Vector3 currentTarget = Target;

                if (previousX > currentX)
                    currentTarget.X += elapsedTime * (previousX - currentX) * scrollSpeed;
                else if (previousX < currentX)
                    currentTarget.X -= elapsedTime * (currentX - previousX) * scrollSpeed;

                Target = currentTarget;
            }
        }

        public void ScrollUpDown(int previousY, int currentY, GameTime gameTime)
        {
            if (previousY != currentY)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector3 currentTarget = Target;

                if (previousY > currentY)
                    currentTarget.Y += elapsedTime * (previousY - currentY) * scrollSpeed;
                else if (previousY < currentY)
                    currentTarget.Y -= elapsedTime * (currentY - previousY) * scrollSpeed;

                Target = currentTarget;
            }
        }
         
        public void Reset()
        {
            //orientation quaternion assumes a Pi rotation so you're facing the "front"
            //of the model (looking down the +Z axis)
            orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi);
            distanceValue = 540f;
            targetValue = Vector3.Zero;
            yaw = MathHelper.Pi;
            pitch = 0;
        }
    }
}
