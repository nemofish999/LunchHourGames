using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Drawing;
using LunchHourGames.Stage;
using LunchHourGames.Common;

namespace LunchHourGames.Screen
{
    public class GameScreen3D : GameScreen
    {
        // Camera that allows scroll, tilt, and zoom
        protected LHGCamera camera;

        // Matrix coordinates of user's perspective
        protected Matrix world, view, projection;

        // Direction the screen can scroll or pan
        public enum PanType { North, South, East, West, Idle }

        // Graphical 3D Stage to that holds all the background and floor sprites in our world
        protected LHGStage stage;

        protected MouseState mouseStateCurrent;
        protected MouseState mouseStatePrevious;

        protected bool allowManualPan = false;
        protected bool allowAutomaticPan = true;
        protected bool isCameraLocked = false;
        protected bool allowZoom = false;
        protected bool allowCameraKeyboardControl = false;
        protected bool allowCameraProperties = false;

        protected int zoomOutValue = 0;
        protected int zoomInValue = 0;
        protected int zoomOffsetValue = 10;

        protected int width = 1024;
        protected int height = 768;

        private Texture2D cameraHUD = null;
        private int hudXSize, hudYSize;

        public GameScreen3D(LunchHourGames lhg, GameScreen.Type type, LHGCamera camera)
            : base(lhg, type)
        {
            this.camera = camera;

            //Calculate the projection properties
            float aspectRatio = (float)lhg.GraphicsDevice.Viewport.Width / (float)lhg.GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, .1f, 10000f);
            world = Matrix.Identity;
        }

        /*
            XNA mouse wheel value is exactly 120 units increment/decrement 
            per notch,  starting at 0 when your game begins. We use the mouse 
            wheel to control the zooming of the camera. Dividing by 120 will give
            you the total number of wheel rotation per notch from the last  
            wheel value subtracted by current mouse wheel value.  just give some 
            offset value if you want to zoom faster or slower.
        */
        protected void handleMousePanAndZoom(GameTime gameTime)
        {
            if (isCameraLocked)
                return;
            
            mouseStateCurrent = Mouse.GetState();

            //LHGCommon.printToConsole("mouse", mouseStateCurrent);

            if (mouseStateCurrent.X < 0 || mouseStateCurrent.X > width ||
                mouseStateCurrent.Y < 0 || mouseStateCurrent.Y > height)
            {
                //LHGCommon.printToConsole("Mouse Out of Range!");
                return;
            }

            if (allowZoom)
            {
                // Zoom In
                if (mouseStateCurrent.ScrollWheelValue > mouseStatePrevious.ScrollWheelValue)
                {
                   // LHGCommon.printToConsole("Zooming In!");

                    int wheelValue = (mouseStateCurrent.ScrollWheelValue - mouseStatePrevious.ScrollWheelValue) / 120;

                    this.zoomOutValue = 0;
                    this.zoomInValue += (wheelValue * this.zoomOffsetValue);
                }

                //! Scroll-Down | Zoom Out
                if (mouseStateCurrent.ScrollWheelValue < mouseStatePrevious.ScrollWheelValue)
                {
                    //LHGCommon.printToConsole("Zooming Out!");
                    int wheelValue = (mouseStatePrevious.ScrollWheelValue - mouseStateCurrent.ScrollWheelValue) / 120;

                    this.zoomInValue = 0;
                    this.zoomOutValue += (wheelValue * this.zoomOffsetValue);
                }

                if (mouseStateCurrent.ScrollWheelValue != mouseStatePrevious.ScrollWheelValue)
                {
                    // We are zooming in or out, update the board to reflect this.
                    Zoom(this.zoomInValue, this.zoomOutValue, gameTime);
                }
            }

            // Manual Pan
            if (allowManualPan)
            {
                if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Pressed)
                {
                    // Left mouse button is being depressed.  Check if the user is dragging.
                    if (mouseStateCurrent.X != mouseStatePrevious.X)
                    {
                        // Left Right
                        ScrollLeftRight(mouseStatePrevious.X, mouseStateCurrent.X, gameTime);
                    }

                    if (mouseStateCurrent.Y != mouseStatePrevious.Y)
                    {
                        // Left Right
                        ScrollUpDown(mouseStatePrevious.Y, mouseStateCurrent.Y, gameTime);
                    }
                }
            }

            allowAutomaticPan = false;

            // Automatic Pan
            if (allowAutomaticPan)
            {
                PanType pan = getPanType(mouseStateCurrent.X, mouseStateCurrent.Y);
                switch (pan)
                {
                    case PanType.North:
                        ScrollUpDown(30, 5, gameTime);
                        break;

                    case PanType.South:
                        ScrollUpDown(5, 30, gameTime);
                        break;

                    case PanType.East:
                        ScrollLeftRight(30, 5, gameTime);
                        break;

                    case PanType.West:
                        ScrollLeftRight(5, 30, gameTime);
                        break;

                    case PanType.Idle:
                        break;
                }
            }

            mouseStatePrevious = mouseStateCurrent;
        }

        protected PanType getPanType(int x, int y)
        {
            int margin = 50;

            if (canScrollLeft() && (x < margin))
                return PanType.West;

            if (canScrollRight() && (x > (width - margin)))
                return PanType.East;

            if (canScrollUp() && (y < margin))
                return PanType.North;

            if (canScrollDown() && (y > (height - margin)))
                return PanType.South;

            return PanType.Idle;
        }

        protected bool canScrollLeft()
        {
            return true;
        }

        protected bool canScrollRight()
        {
            return true;
        }

        protected bool canScrollUp()
        {
            return true;
        }

        protected bool canScrollDown()
        {
            return true;
        }

        public void Zoom(int zoomInValue, int zoomOutValue, GameTime gameTime)
        {
            this.camera.Zoom(zoomInValue, zoomOutValue, gameTime);
        }

        public void ScrollLeftRight(int previousX, int currentX, GameTime gameTime)
        {
            this.camera.ScrollLeftRight(previousX, currentX, gameTime);
        }

        public void ScrollUpDown(int previousY, int currentY, GameTime gameTime)
        {
            this.camera.ScrollUpDown(previousY, currentY, gameTime);
        }

        public void handleKeyboardPanAndZoom(GameTime gameTime)
        {
            if (allowCameraKeyboardControl)
            {
                KeyboardState newState = Keyboard.GetState();
                camera.HandleDefaultKeyboardControls(newState, gameTime);
            }
        }

        public LHGCamera MyCamera
        {
            get { return this.camera; }
        }

        public float CameraDistance
        {
            get { return MyCamera.Distance; }
        }

        public Matrix MyView
        {
            get { return camera.ViewMatrix; }
        }

        public Matrix MyProjection
        {
            get { return this.projection; }
        }

        public Matrix MyWorld
        {
            get { return this.world; }
        }

        public LHGStage MyStage
        {
            get { return this.stage; }
            set 
            { 
                this.stage = value;

                this.stage.MyWorld = this.world;
                this.stage.MyProjection = this.projection;
                this.stage.MyView = this.view;            
            }
        }

        public Vector3 convertScreenToWorld(int x, int y)
        {
            // Ray tracing algorithm modified from tutorial at http://blog.icubed.me/?p=5
            //
            //  Unproject the screen space mouse coordinate into model space 
            //  coordinates. Because the world space matrix is identity, this 
            //  gives the coordinates in world space.
            Viewport vp = lhg.GraphicsDevice.Viewport;

            Vector3 nearsource = new Vector3((float)x, (float)y, 0f);
            Vector3 farsource = new Vector3((float)x, (float)y, 1f);

            Vector3 nearPoint = lhg.GraphicsDevice.Viewport.Unproject(nearsource, projection, view, world);
            Vector3 farPoint = lhg.GraphicsDevice.Viewport.Unproject(farsource, projection, view, world);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);

            //Check if the ray is pointing down towards the ground  
            //(aka will it intersect the plane)  
            if (pickRay.Direction.Y < 0.0)
            {
                float xPos = 0f;
                float zPos = 0f;

                //Move the ray lower along its direction vector  
                while (pickRay.Position.Y > 0)
                {
                    pickRay.Position += pickRay.Direction;
                    xPos = pickRay.Position.X;
                    zPos = pickRay.Position.Z;
                }

                //Once it has move pass y=0, stop and record the X  
                // and Y position of the ray, return new Vector3  
                return new Vector3(xPos, 0, zPos);
            }

            return Vector3.Zero;
        }

        public Point convertWorldToScreenPoint(Vector3 sourceVector)
        {
            Viewport vp = lhg.GraphicsDevice.Viewport;
            Vector3 screen = vp.Project(sourceVector, projection, view, world);

            return new Point((int)screen.X, (int)screen.Y);
        }

        public Vector2 convertWorldToScreenVector2(Vector3 sourceVector)
        {
            Viewport vp = lhg.GraphicsDevice.Viewport;
            Vector3 screen = vp.Project(sourceVector, projection, view, world);

            return new Vector2(screen.X, screen.Y);
        }

        public Rectangle convertWorldToScreenRectangle(Rectangle3D rect3d)
        {
            view = camera.ViewMatrix;

            Point topLeft = convertWorldToScreenPoint(rect3d.topLeft);
            Point topRight = convertWorldToScreenPoint(rect3d.topRight);
            Point bottomLeft = convertWorldToScreenPoint(rect3d.bottomLeft);

            int width = topRight.X - topLeft.X;
            int height = bottomLeft.Y - topLeft.Y;

            return new Rectangle(topLeft.X, topLeft.Y, width, height);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Update our local view with what the camera has in it.
            view = camera.ViewMatrix;

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            if (allowCameraProperties)
                showCameraProperties();

            base.Draw(gameTime);
        }

        public void resetCamera()
        {
            camera.Reset();
        }

        public bool LockCamera
        {
            set { isCameraLocked = value; }
        }

        public void doCameraTest(bool start)
        {
            if (!start)
            {
                allowCameraKeyboardControl = false;
                allowCameraProperties = false;
            }
            else
            {
                allowCameraKeyboardControl = true;
                allowCameraProperties = true;

                hudXSize = 210;
                hudYSize = 100;

                if (cameraHUD == null)
                {
                    cameraHUD = new Texture2D(lhg.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    cameraHUD.SetData<Color>(new Color[1] { new Color(0, 0, 0, 128) });
                }
            }
        }

        private void showCameraProperties()
        {
            //set the offsets 
            int hudXOffset = 80;
            int hudYOffset = 10;

            SpriteBatch spriteBatch = lhg.MySpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(cameraHUD, new Rectangle(hudXOffset, hudYOffset, hudXSize, hudYSize), Color.White);

            SpriteFont font = lhg.SmallFont;

            string targetAsString = LHGCommon.formatString( "target", camera.Target);
            string distanceAsString = LHGCommon.formatString( "distance", camera.Distance);;
            string positionAsString = LHGCommon.formatString("position", camera.Position);
            string directionAsString = LHGCommon.formatString("direction", camera.Direction);
            string yawAsString = LHGCommon.formatString("yaw", camera.Yaw);
            string pitchAsString = LHGCommon.formatString("pitch", camera.Pitch);

            int j = 0;
            spriteBatch.DrawString(font, targetAsString, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
            
            j++;
            spriteBatch.DrawString(font, distanceAsString, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;
            spriteBatch.DrawString(font, positionAsString, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;
            spriteBatch.DrawString(font, directionAsString, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;
            spriteBatch.DrawString(font, yawAsString, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);
            j++;
            spriteBatch.DrawString(font, pitchAsString, new Vector2(hudXOffset + 5, hudYOffset + font.LineSpacing * (j)), Color.Yellow);

            spriteBatch.End();
        }
    }
}
