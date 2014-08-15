///////////////////////////////////////////////////////////////////////////////////////////
// LunchHourGames.cs
//
// This is the main XNA game class.  It holds all the classes that make up our game.
//
// Copyright (C) 2011 Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using LunchHourGames.GamePlay;
using LunchHourGames.Screen;
using LunchHourGames.Players;
using LunchHourGames.Combat;
using LunchHourGames.Console;
using LunchHourGames.Inventory;
using LunchHourGames.Drawing;

namespace LunchHourGames
{
    public class LunchHourGames : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;        // XNA underlying graphics system 
        private SpriteBatch spriteBatch;       // Draws 2D sprites in the game

        private UserProfile profile;           // Stores the user's profile (i.e. saved games and current state)
        private GameFlow gameFlow;             // Handles the overall story and flow of the game
        private SoundSystem soundSystem;       // Handles all the sounds (music and sound fx) in the game
        private ScreenManager screenManager;   // Manages screen transistions and dispacting the update and draw methods 
        private AssetManager assets;           // Caches the assets used most in the game (sprite sheets and player templates)

        private CombatSystem combat;           // Handles the combat scenes (i.e. hexagonal board battles)
        // private TravelSystem travel;        // Handles the travel scenes (i.e. map and distance traveled)
        // private ScavengeSystem scavenge;    // Handles the scavenging scenes (i.e. looking for supplies and resources)
        // private StorySystem story;          // Handles the story and dialog scenes (i.e plays videos and shows story text)
        private InventorySystem inventory;     // Handles the inventory in the game (i.e. weapons, food, supplies) and barter / trading

        private LHGConsole console;            // Game console that allows commands to debug and activate various features

        private bool isPaused = false;         // Is the game paused or not?
        private bool isFullScreen = false;     // Is this game running in full screen mode

        private String gameWindowTitle = "Trail of the Dead";  // Game title shown in the window

        private List<Player> players;         // Holds the list of players in the game
        
        // Keyboard states
        static KeyboardState newState;
        static KeyboardState oldState;

        // Fonts used in the game
        private SpriteFont normalFont;
        private SpriteFont smallFont;
        private SpriteFont consoleFont;

        //private List<MessageBalloon> messages;

        public LunchHourGames()
        {
            graphics = new GraphicsDeviceManager(this);

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 1024;
            
            graphics.IsFullScreen = this.isFullScreen;
            this.Window.Title = this.gameWindowTitle;

            Content.RootDirectory = "Content";

            this.assets = new AssetManager(this);
            this.screenManager = new ScreenManager(this);
            this.gameFlow = new GameFlow(this);
            this.soundSystem = new SoundSystem(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.ApplyChanges(); 

            //Make the mouse pointer visible in the game window
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            // If we’re using the Reach graphics profile, We need to do add the line
            // The reason we need to do this is to change the texture address mode to Clamp 
            // to allow using texture sizes that are not powers of two.
            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadFonts();

            this.console = new LHGConsole(this, consoleFont);
            this.screenManager.Initialize();
            this.gameFlow.Initialize();


       


            //this.mode = GameMode.LHGLogo;
            //this.activeScreen = logoScreen;
            //MediaPlayer.Play(this.introSong);
            //MediaPlayer.IsRepeating = true;

            /*
            gameFactory = new GameFactory(this);

            this.combat = gameFactory.loadCombat("level1", "combat1");

            //this.design = new DesignScreen(this);

            //gotoDesign();

            startCombatMode();
             * */
          
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        private void LoadFonts()
        {
            this.normalFont = Content.Load<SpriteFont>("Fonts/normal");
            this.smallFont = Content.Load<SpriteFont>("Fonts/smallFont");
            this.consoleFont = Content.Load<SpriteFont>("Fonts/ConsoleFont");
        }

        public SpriteFont NormalFont
        {
            get { return this.normalFont; }
        }

        public SpriteFont SmallFont
        {
            get { return this.smallFont; }
        }

        public SpriteFont ConsoleFont
        {
            get { return this.consoleFont; }
        }

        public GameFlow MyGameFlow
        {
            get { return this.gameFlow; }
        }

        public LHGConsole MyConsole
        {
            get { return console; }
        }

        public SoundSystem MySoundSystem
        {
            get { return this.soundSystem; }
        }

        public ScreenManager MyScreenManager
        {
            get { return this.screenManager; }
        }
 
        public AssetManager Assets
        {
            get { return this.assets; }
        }

        public SpriteBatch MySpriteBatch
        {
            get { return this.spriteBatch; }
        }

        public CombatSystem MyCombatSystem
        {
            get { return this.combat; }
            set { this.combat = value; }
        }

        public void showMessageBalloon(GameTime gameTime, String message)
        {
            /*
            MessageBalloon balloon = new MessageBalloon(this, message, this.normalFont,
                MessageBalloon.Effect.Pulsate, 2);
            balloon.MyStartTime = gameTime.TotalGameTime;
            messages.Add(balloon);
             * */
        }

        public static bool CheckKey(Keys theKey)
        {
            return oldState.IsKeyDown(theKey) && newState.IsKeyUp(theKey);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.gameFlow.quitGame();

            // Tell the game system that we are updating
            this.gameFlow.Update(gameTime);

            // Update all screens
            this.screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);

            // Draw all screens
            this.screenManager.Draw(gameTime);

            // Draw any information the game system wants to show.  Generally debug information or other messages
            this.gameFlow.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Attempt to set the display mode to the desired resolution.  Itterates through the display
        /// capabilities of the default graphics adapter to determine if the graphics adapter supports the
        /// requested resolution.  If so, the resolution is set and the function returns true.  If not,
        /// no change is made and the function returns false.
        /// </summary>
        /// <param name="iWidth">Desired screen width.</param>
        /// <param name="iHeight">Desired screen height.</param>
        /// <param name="bFullScreen">True if you wish to go to Full Screen, false for Windowed Mode.</param>
        private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
        {
            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (bFullScreen == false)
            {
                if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    graphics.PreferredBackBufferWidth = iWidth;
                    graphics.PreferredBackBufferHeight = iHeight;
                    graphics.IsFullScreen = bFullScreen;
                    graphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate thorugh the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == iWidth) && (dm.Height == iHeight))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        graphics.PreferredBackBufferWidth = iWidth;
                        graphics.PreferredBackBufferHeight = iHeight;
                        graphics.IsFullScreen = bFullScreen;
                        graphics.ApplyChanges();
                        return true;
                    }
                }
            }
            return false;
        }
    }  
}