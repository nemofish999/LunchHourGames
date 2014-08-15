using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LunchHourGames.Controls
{
    public class PushButton : BasicControl
    {
        public interface EventHandler
        {
            void handlePushButtonEvent(string referenceName, State state);
        }
        private EventHandler eventHandler;

        public enum State
        {
            Hover,
            Up,
            Released,
            Down
        }
        private State currentState;

        private Texture2D releasedTexture;
        private Texture2D pressedTexture;
        private Texture2D hoverTexture;
        private Texture2D currentTexture;
        private Tooltip toolTip;

        private double timer;
        private bool mousePressed;
        private bool previousMousePressed;        
        private int mouseX, mouseY;
        private double frameTime;

        public PushButton(LunchHourGames lhg, string referenceName, string displayName, State initState, Vector2 position,
                          Texture2D pressedTexture, Texture2D releasedTexture, Texture2D hoverTexture,
                          Tooltip toolTip, EventHandler eventHandler)
            : base(lhg, referenceName, displayName, position)
        {
            this.currentState = initState;
            this.releasedTexture = releasedTexture;
            this.pressedTexture = pressedTexture;
            this.toolTip = toolTip;
            this.eventHandler = eventHandler;
        }

        private Texture2D getCurrentButton(State state)
        {
            Texture2D texture = null;

            switch (state)
            {
                case State.Hover:
                    texture = hoverTexture;
                    break;

                case State.Up:
                case State.Released:
                    texture = releasedTexture;
                    break;

                case State.Down:
                    texture = pressedTexture;
                    break;
            }

            return texture;
        }

        public override void Update(GameTime gameTime)
        {
            Texture2D texture = getCurrentButton(currentState);
            MouseState mouseStateCurrent = Mouse.GetState();
            State previousState = currentState;

            if ( texture != null && hitImageAlpha(texture.Bounds, texture, mouseStateCurrent.X, mouseStateCurrent.Y) )
            {
                timer = 0.0;
                if ( mousePressed )
                {
                    // mouse is currently down
                    currentState = State.Down;
                }
                else if (!mousePressed && previousMousePressed)
                {
                    // mouse was just released
                    if (currentState == State.Down)
                    {
                        currentState = State.Released;
                    }
                }
                else
                {
                    currentState = State.Hover;
                }
            }
            else
            {
                currentState = State.Up;
            }

            if (currentState == State.Released)
            {
                eventHandler.handlePushButtonEvent(referenceName, currentState);
            }

            if ( previousState != currentState )           
                currentTexture = getCurrentButton(currentState);
                
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            lhg.MySpriteBatch.Draw(currentTexture, position, alpha);           
            base.Draw(gameTime);
        }
    }
}
