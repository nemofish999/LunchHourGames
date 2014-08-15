
using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames;

namespace LunchHourGames.Screen
{
    // The screen manager is a component which manages one or more GameScreen instances. It maintains a stack of screens, 
    // calls their Update and Draw methods at the appropriate times, and automatically routes input to the topmost active screen.
    public class ScreenManager : DrawableGameComponent
    {
        private LunchHourGames lhg;

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        InputState input = new InputState();

        Texture2D blankTexture;

        bool traceEnabled;

        // If true, the manager prints out a list of all the screens each time it is updated. This can be useful for making sure
        // everything is being added and removed at the right times.
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }
                
        public ScreenManager(LunchHourGames lhg)
            :base(lhg)
        {
            this.lhg = lhg;
        }

        public override void Initialize()
        {
            LoadGraphicsContent(true);          
        }

        protected void LoadGraphicsContent(bool loadAllContent)
        {
            // Load content belonging to the screen manager.
            if (loadAllContent)
            {
                blankTexture = lhg.Content.Load<Texture2D>("Backgrounds/blank");    
            }

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.LoadGraphicsContent(true);
            }
        }

        protected void UnloadGraphicsContent(bool unloadAllContent)
        {
            // Unload content belonging to the screen manager.
            if (unloadAllContent)
            {
                lhg.Content.Unload();
            }

            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.UnloadGraphicsContent(unloadAllContent);
            }
        }
    
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
            {
                screensToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = false;  // !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = screensToUpdate[0];

                screensToUpdate.RemoveAt(0);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.MyScreenState == GameScreen.ScreenState.TransitionOn || screen.MyScreenState == GameScreen.ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, input);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (screens.Count > 0)
            {
                screens[0].Draw(gameTime);
            }

            /*
            foreach (GameScreen screen in screens)
            {
                if (screen.MyScreenState != GameScreen.ScreenState.Hidden)
                {
                    screen.Draw(gameTime);
                }
            }
             * */
        }

        public void AddScreen(GameScreen screen)
        {
            screen.MyScreenManager = this;
            screen.LoadGraphicsContent(true);
            screens.Add(screen);
        }

        // Removes a screen from the screen manager. You should normally use GameScreen.ExitScreen instead of calling this directly, so
        // the screen can gradually transition off rather than just being instantly removed.
        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadGraphicsContent(true);
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }


        // Expose an array holding all the screens.  Screens should only ever be added or removed using the AddScreen and RemoveScreen methods
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }
        
        // Helper draws a translucent black fullscreen sprite, used for fading screens in and out, 
        // and for darkening the background behind popups
        public void FadeBackBufferToBlack(int alpha)
        {
            Viewport viewport = lhg.GraphicsDevice.Viewport;
            SpriteBatch spriteBatch = lhg.MySpriteBatch;

            Color tintColor = Color.Black;
            tintColor.A = (byte) alpha;

            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), tintColor);
            spriteBatch.End();
        }
    }
}