///////////////////////////////////////////////////////////////////////////////////////////
// GameSystem.cs
//
// This class represents the system that handles the overall story and flow of the game. 
// It is responsible for making the transistions from one scene or level to another. 
// This includes the decisions that govern overall story and game play.  The actual logic
// of all the possible game decisions and story are stored in game.xml.  This class will
// also handle custom levels (user mods) downloaded from our mod tool website.
//
// Copyright (C) 2011 Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using LunchHourGames.GamePlay;
using LunchHourGames.Screen;
using LunchHourGames.Players;
using LunchHourGames.Combat;
using LunchHourGames.Obstacles;
using LunchHourGames.Console;
using LunchHourGames.Inventory;
using LunchHourGames.Drawing;
using LunchHourGames.Stage;
using LunchHourGames.Controls;

namespace LunchHourGames
{
    public class GameFlow : DrawableGameComponent
    {
        public enum GameMode
        {
            Logo, Title, Start, PlayerRating, Story, Travel, LoadingCombat, Combat, Scavenge, Options, Pause, Credits, Design
        }

        private LunchHourGames lhg;
        private ScreenManager screenManager;
        private GameMode mode;                 // Current mode of the game
        
        private XmlDocument gameDocument = new XmlDocument();

        private CombatFactory combatFactory;
        private PlayerFactory playerFactory;
        private ObstacleFactory obstacleFactory;
        private StageFactory stageFactory;
        private InventoryFactory inventoryFactory;

        private LogoScreen logoScreen;
        private StartScreen startScreen;
        //private PlayerScreen playerScreen;
        private RatingScreen ratingScreen;
        private ControlsTestScreen controlsTestScreen;

        private LoadingScreen loadingScreen;

        private Thread combatLoadingThread;

        private Party party;

        private bool isConsoleEnabled = true;

        public GameFlow(LunchHourGames lhg)
            : base(lhg)
        {
            this.lhg = lhg;
            screenManager = lhg.MyScreenManager;
            this.party = new Party(lhg);
        }

        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            loadingScreen = new LoadingScreen(lhg);

            inventoryFactory = new InventoryFactory(lhg, loadingScreen);
            playerFactory = new PlayerFactory(lhg, loadingScreen, inventoryFactory);
            obstacleFactory = new ObstacleFactory(lhg, loadingScreen);
            stageFactory = new StageFactory(lhg, loadingScreen);
            combatFactory = new CombatFactory(lhg, loadingScreen, playerFactory, obstacleFactory, stageFactory, inventoryFactory);
            
            showCombatLoading("combat1");
            //showControlsTestScreen();
                        
                
            base.Initialize();
        }

        public void setGameScreenComplete(GameScreen gameScreen)
        {
            switch ( gameScreen.MyType )
            {
                case GameScreen.Type.Logo:
                    showStartScreen();
                    break;

                case GameScreen.Type.PlayerRating:
                    //Player mainPlayer =  playerFactory.createMainPlayer("arno", "Travis", this.ratingScreen.MyStats);
                    //party.addHuman((Human)mainPlayer);
                    showCombatLoading("combat1");
                    break;
            }

            gameScreen.ExitScreen();
        }

        public void startNewGame()
        {
            startScreen.ExitScreen();
            showPlayerRating();
        }

        public void continueGame()
        {
        }

        public void showOptions()
        {
        }

        public void quitGame()
        {
            // Make sure all threads are done before we exit
            if (combatLoadingThread != null && combatLoadingThread.IsAlive)
                combatLoadingThread.Abort();

            lhg.Exit();
        }

        private void showLogoScreen()
        {
            logoScreen = new LogoScreen(lhg);
            lhg.MyScreenManager.AddScreen(logoScreen);
            mode = GameMode.Logo;
        }

        private void showStartScreen()
        {
            startScreen = new StartScreen(lhg);
            screenManager.AddScreen(startScreen);
            mode = GameMode.Start;
        }

        private void showPlayerRating()
        {
            ratingScreen = new RatingScreen(lhg);
            screenManager.AddScreen(ratingScreen);
            mode = GameMode.PlayerRating;
        }

        private void showControlsTestScreen()
        {
            controlsTestScreen = new ControlsTestScreen(lhg);
            screenManager.AddScreen(controlsTestScreen);
            mode = GameMode.Design;
        }

        private void showCombatLoading(String combatReferenceName)
        {
            screenManager.AddScreen(loadingScreen);
            combatFactory.prepareCombatToLoad(combatReferenceName);

            combatLoadingThread = new Thread(new ThreadStart(combatFactory.loadCombat));
            combatLoadingThread.Start();

            mode = GameMode.LoadingCombat;
        }

        private void transitionToCombat()
        {
            if (this.combatLoadingThread != null)
            {
                if (!this.combatLoadingThread.IsAlive)
                {
                    if (lhg.MyCombatSystem != null)
                    {
                        screenManager.AddScreen(lhg.MyCombatSystem.MyScreen);

                        Player mainPlayer = playerFactory.createMainPlayer("arno", "Travis", PrimaryStatistics.PlayerDefaults);
                        party.addHuman((Human)mainPlayer);

                        Player soldier = playerFactory.createMainPlayer("soldier", "Sarge", PrimaryStatistics.PlayerDefaults);
                        party.addHuman((Human)soldier);

                        int level = 1;
                        lhg.MyCombatSystem.startLevel(party, level);
                        loadingScreen.ExitScreen();
                        this.mode = GameMode.Combat;

                        lhg.MyConsole.setupInterpreter(lhg.MyCombatSystem.Interpreter);
                        isConsoleEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            switch (mode)
            {
                case GameMode.Logo:
                    break;
                
                case GameMode.Title:
                    break;
                
                case GameMode.PlayerRating:
                    break;
                
                case GameMode.Story:
                    break;
                
                case GameMode.Travel:
                    break;
                
                case GameMode.LoadingCombat:
                    transitionToCombat();
                    break;

                case GameMode.Combat:
                    break;
                
                case GameMode.Scavenge:
                    break;
                
                case GameMode.Options:
                    break;
                
                case GameMode.Pause:
                    break;
                
                case GameMode.Credits:
                    break;
                
                case GameMode.Design:
                    break;
            }

            if (isConsoleEnabled)
                lhg.MyConsole.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (isConsoleEnabled)
                lhg.MyConsole.Draw(gameTime);
        }
    }
}
