using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LunchHourGames.Controls
{
    public class PopupDrawer : BasicControl
    {
        public enum State
        {
            Closed,
            Closing,
            Open,
            Opening
        }
        private State currentState;
        private double stateStartTime;
            
        double AnimationTime = 0.2;      
        private Texture2D background;

        private int height, width;

        public PopupDrawer(LunchHourGames lhg, string referenceName, string displayName, Vector2 position,
                         Texture2D background, List<BasicControl> childControls)
            : base(lhg, referenceName, displayName, position)
        {
            this.background = background;
            currentState = State.Closed;
            stateStartTime = 0;
        }

        public PopupDrawer(LunchHourGames lhg, string referenceName, string displayName, Rectangle extents, List<BasicControl> childControls )
            : base(lhg, referenceName, displayName, new Vector2(extents.X, extents.Y))
        {
            background = new Texture2D(lhg.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            background.SetData<Color>(new Color[1] { new Color(0, 0, 0, 125) }); 
            currentState = State.Closed;
            stateStartTime = 0;
        }      

        private bool shouldOpen()
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            double now = gameTime.TotalGameTime.TotalSeconds;
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds; //time since last update call

            if (currentState == State.Closing)
            {
                if (now - stateStartTime > AnimationTime)
                {
                    currentState = State.Closed;
                    stateStartTime = now;
                }

                return;
            }

            if (currentState == State.Opening)
            {
                if (now - stateStartTime > AnimationTime)
                {
                    currentState = State.Open;
                    stateStartTime = now;
                }

                return;
            }

            if (currentState == State.Closed)
            {
                if (shouldOpen()) //this opens the console
                {
                    currentState = State.Opening;
                    stateStartTime = now;
                    this.Visible = true;
                }
                else
                {
                    return;
                }
            }

            if (currentState == State.Open)
            {
                if (shouldOpen())
                {
                    currentState = State.Closing;
                    stateStartTime = now;
                    return;
                }
            }
        }
  
        public override void Draw(GameTime gameTime)
        {
            //don't draw the console if it's closed
            if (currentState == State.Closed)
                return;

            /*
            double now = gameTime.TotalGameTime.TotalSeconds;

            //get console dimensions
            int xSize = this.Game.Window.ClientBounds.Right - this.Game.Window.ClientBounds.Left - 20;
            int ySize = this.font.LineSpacing * LinesDisplayed + 20;

            //set the offsets 
            int xOffset = 10;
            int yOffset = 10;

            //run the opening animation
            if (currentState == State.Opening)
            {
                int startPosition = 0 - yOffset - ySize;
                int endPosition = yOffset;
                yOffset = (int)MathHelper.Lerp(startPosition, endPosition, (float)(now - stateStartTime) / (float)AnimationTime);
            }
            //run the closing animation
            else if (currentState == State.Closing)
            {
                int startPosition = yOffset;
                int endPosition = 0 - yOffset - ySize;
                yOffset = (int)MathHelper.Lerp(startPosition, endPosition, (float)(now - stateStartTime) / (float)AnimationTime);
            }
            
            
            lhg.MySpriteBatch.Draw(background, new Rectangle(xOffset, yOffset, xSize, ySize), Color.White);
            
            //reset depth buffer to normal status, so as not to mess up 3d code            
            lhg.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
             * */
        }












    }
}
