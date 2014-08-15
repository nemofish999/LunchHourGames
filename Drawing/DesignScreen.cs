///////////////////////////////////////////////////////////////////////////////
// DesignScreen.cs
//
// Allows testing various designs using the XNA plateform without interrupting 
// game play.
//
// Copyright (C) Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

using LunchHourGames.Screen;
using LunchHourGames.Combat;
using LunchHourGames.Sprite;
using LunchHourGames.Common;

namespace LunchHourGames.Drawing
{
    public class DesignScreen : GameScreen
    {        
        private Vector2 safeBounds;
        private LHGCamera camera;
        private LHGGrid grid;
        private KeyboardState lastKeyboardState;
        private BackgroundPanel background;

        MouseState mouseStatePrevious;

        private StaticSprite playerHUD;

        private CombatBoard combatBoard;

        public DesignScreen(LunchHourGames lhg)
            :base(lhg, Type.Design)
        {
            LoadContent();
            mouseStatePrevious = Mouse.GetState();
        }
                  
        private Matrix world, view, projection;

        public void setBackground(String fromContent)
        {
            Texture2D texture = lhg.Content.Load<Texture2D>(fromContent);
            //this.background = new BackgroundScreen(Game, this.spriteBatch, texture, false);
        }

        public void setPlayerHUD(string spriteName, Vector2 position)
        {
            Texture2D texture = lhg.Content.Load<Texture2D>(spriteName);
            this.playerHUD = new StaticSprite(lhg, texture, position, lhg.MySpriteBatch);
        }

        protected override void LoadContent()
        {
            //setBackground("Backgrounds/Dungeon");
            setBackground("Backgrounds/outside");
            setPlayerHUD("Sprites/Players/Arno Combat HUD", new Vector2(10, 10));

            //Set up the reference grid and sample camera
            grid = new LHGGrid(lhg);
            grid.MyColor = Color.CadetBlue;           
            grid.LoadGraphicsContent(lhg.GraphicsDevice);

            camera = new LHGCamera(LHGCameraMode.RollConstrained);           

            //orbit the camera so we're looking down the z=-1 axis            
            camera.OrbitRight(MathHelper.Pi);
            camera.OrbitUp(.32f);                      
            camera.Target = new Vector3(1505f, 0.0f, 470.0f);
            camera.Position = new Vector3(1506f, 844f, 2073f);            

            camera.Distance = 2000.0f;
       
            //create the spritebatch for debug text
            spriteBatch = lhg.MySpriteBatch;

            //Calculate the projection properties first on any 
            //load callback.  That way if the window gets resized,
            //the perspective matrix is updated accordingly
            float aspectRatio = (float)lhg.GraphicsDevice.Viewport.Width / (float)lhg.GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, .1f, 10000f);

            //grid requires a projection matrix to draw correctly
            grid.ProjectionMatrix = projection;
    
            world = Matrix.Identity;

            //Set the grid to draw on the x/z plane around the origin
            grid.WorldMatrix = world;

            // calculate the safe left and top edges of the screen
            safeBounds = new Vector2(
                (float)lhg.GraphicsDevice.Viewport.X +
                (float)lhg.GraphicsDevice.Viewport.Width * 0.1f,
                (float)lhg.GraphicsDevice.Viewport.Y +
                (float)lhg.GraphicsDevice.Viewport.Height * 0.1f
                );

            //this.combatBoard = new CombatBoard(this.lhg, 50, 65, 20, 0, 0, 2, false);            
            combatBoard.DrawBoard();
        }

        /// <summary>
        /// Update the game world.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            /*
            KeyboardState keyboardState = Keyboard.GetState();         

            //Handle inputs for the sample camera
            camera.HandleDefaultKeyboardControls(keyboardState, gameTime);

            //handle inputs specific to this screen
            HandleInput(gameTime, keyboardState);
                       
            //The built-in camera class provides the view matrix
            view = camera.ViewMatrix;
            
            //additionally, the reference grid requires a view matrix to draw correctly
            grid.ViewMatrix = camera.ViewMatrix;
          
            MouseState mouseStateCurrent = Mouse.GetState();
            if (mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                mouseStatePrevious.LeftButton == ButtonState.Released)
            {
                LHGCommon.printToConsole("mouse", mouseStateCurrent);

                Vector3 worldPosition = convertScreenToWorld(mouseStateCurrent.X, mouseStateCurrent.Y);
                LHGCommon.printToConsole("world",worldPosition);

                this.combatBoard.selectCell(worldPosition);
                combatBoard.DrawBoard();

                Point screenPosition = convertWorldToScreen(worldPosition);
                LHGCommon.printToConsole("screen", screenPosition);

            }

            mouseStatePrevious = mouseStateCurrent;

            lastKeyboardState = keyboardState;
            */

            base.Update(gameTime);
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

        public Point convertWorldToScreen(Vector3 sourceVector)
        {
            Viewport vp = lhg.GraphicsDevice.Viewport;
            Vector3 screen = vp.Project(sourceVector, projection, view, world);

            return new Point((int)screen.X, (int)screen.Y);
        }


        private void HandleInput(GameTime gameTime, KeyboardState keyboardState)
        {
            float elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

     
        public override void Draw(GameTime gameTime)
        {
            lhg.GraphicsDevice.Clear(Color.Black);

            lhg.MySpriteBatch.Begin();
            background.Draw(gameTime);
            playerHUD.Draw(gameTime);
            lhg.MySpriteBatch.End();

            //the SpriteBatch added below to draw the current technique name
            //is changing some needed render states, so they are reset here.
            lhg.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //draw the reference grid so it's easier to get our bearings
            grid.Draw();
            base.Draw(gameTime);
        }
    }
}
