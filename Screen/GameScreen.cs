using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using LunchHourGames;

namespace LunchHourGames.Screen
{
    public abstract class GameScreen : DrawableGameComponent
    {
        public enum Type
        {
            Logo, Title, Start, PlayerSetup, PlayerRating, Story, Travel, Combat, Scavenge, Options, Pause, Credits, Design, Loading

        }

        public enum ScreenState
        {
            TransitionOn,
            Active,
            TransitionOff,
            Hidden,
        }

        public enum PlayState
        {
            Intialized,
            Loading,
            Playing,
            Paused,
            Finished
        }

        protected LunchHourGames lhg;
        protected Type type;
        protected PlayState playState;

        protected ScreenManager screenManager;
        protected ScreenState screenState = ScreenState.TransitionOn;

        protected bool isPopup = false;
        protected TimeSpan transitionOnTime = TimeSpan.Zero;
        protected float transitionPosition = 0;

        protected List<GameComponent> childComponents;
        protected SpriteBatch spriteBatch;
        protected ContentManager Content;
        protected bool isExiting = false;
        protected bool otherScreenHasFocus;

        public GameScreen(LunchHourGames lhg, Type type)
            :base(lhg)
        {
            this.lhg = lhg;
            this.type = type;
            playState = PlayState.Intialized;
            spriteBatch = lhg.MySpriteBatch;
            Content = lhg.Content;
            childComponents = new List<GameComponent>();
            Visible = false;
            Enabled = false;
        }

        public GameScreen.Type MyType
        {
            get { return this.type; }
        }

        // Normally when one screen is brought up over the top of another, the first screen will transition off to make room for the new
        // one. This property indicates whether the screen is only a small popup, in which case screens underneath it do not need to bother
        // transitioning off.
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }

        // Indicates how long the screen takes to transition on when it is activated
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            set { transitionOnTime = value; }
        }

        // Indicates how long the screen takes to transition off when it is deactivated.
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;

        // Gets the current position of the screen transition, ranging from zero (fully active, no transition) to 
        // one (transitioned fully off to nothing).
        public float TransitionPosition
        {
            get { return transitionPosition; }
            set { transitionPosition = value; }
        }

        // Gets the current alpha of the screen transition, ranging from 255 (fully active, no transition) to
        // 0 (transitioned fully off to nothing).
        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionPosition * 255); }
        }

        // Gets the current screen transition state.
        public ScreenState MyScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        // There are two possible reasons why a screen might be transitioning off. It could be temporarily going away to make room for another
        // screen that is on top of it, or it could be going away for good.
        // This property indicates whether the screen is exiting for real: if set, the screen will automatically remove itself as soon as the
        // transition finishes.
        public bool IsExiting
        {
            get { return isExiting; }
            protected set { isExiting = value; }
        }

        // Checks whether this screen is active and can respond to user input.
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus && (screenState == ScreenState.TransitionOn || screenState == ScreenState.Active);
            }
        }

        // Gets the manager that this screen belongs to.
        public ScreenManager MyScreenManager
        {
            get { return this.screenManager; }
            set { this.screenManager = value; }
        }
 
        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        // Load graphics content for the screen.
        public virtual void LoadGraphicsContent(bool loadAllContent)
        { 
        }

        // Unload content for the screen.
        public virtual void UnloadGraphicsContent(bool unloadAllContent)
        { 
        }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                // If the screen is going away to die, it should transition off.
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    MyScreenManager.RemoveScreen(this);

                    isExiting = false;
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Active;
                }
            }
        }

        // Updates the screen transition position.
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if ((transitionPosition <= 0) || (transitionPosition >= 1))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                MyScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            base.Draw(gameTime);
        }

        // Handles user input. Unlike Update, this method is only called when the screen is active, and not when some other
        // screen has taken the focus.
        public virtual void HandleInput(GameTime gameTime, InputState input)
        {
        }

        // Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which instantly kills the screen, this method respects
        // the transition timings and will give the screen a chance to gradually transition off.
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                MyScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }

        public void updateProgress(string developerMessage, string displayMessage, int percentComplete)
        {
        }

        public void complete()
        {
        }
    }
}
