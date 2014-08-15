///////////////////////////////////////////////////////////////////////////////
// CombatScreen.cs
//
// Handles drawing the entire combat screen.  This includes board, players, background, and HUD.
//
// Copyright (C) Lunch Hour Games. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.Sprite;
using LunchHourGames.Drawing;
using LunchHourGames.Screen;
using LunchHourGames.Players;
using LunchHourGames.Obstacles;
using LunchHourGames.Common;

using LunchHourGames.Hexagonal;

namespace LunchHourGames.Combat
{
    public class CombatScreen : GameScreen3D
    {
        private CombatSystem    combatSystem;  // The combat system associated with this screen.  This class manages all the components of the combat scene (i.e. board, this screen, turn-based play)
        private CombatBoard     combatBoard;  // The hexagonal board that is on our screen.  We could get this from the combat system, but it is being provided here for convenience and faster access.
        private CombatMenu      combatMenu;  // The player's action menu
        private CombatHUD       combatHUD;  // The heads-up-display that shows player statistics
        private BackgroundPanel background;  // 2D Background sprite. The stage is in the front of this (foreground)


        public CombatScreen(LunchHourGames lhg, LHGCamera camera)
            : base(lhg, Type.Combat, camera)
        {
            this.combatHUD = new CombatHUD(lhg, this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();           
        }

        // Load graphics content for the screen.
        public override void LoadGraphicsContent(bool loadAllContent)
        {
            width = Game.Window.ClientBounds.Width;
            height = Game.Window.ClientBounds.Height;

            Texture2D backgroundTexture = LHGGraphicsHelper.getGradientTexture(lhg.GraphicsDevice, width, height, Color.DarkBlue, Color.Chocolate);
            background = new BackgroundPanel(lhg, backgroundTexture, true);
        }

        public void startLevel(int level)
        {
            combatHUD.startLevel(level);
        }

        public CombatSystem MyCombatSystem
        {
            get { return this.combatSystem; }
            set 
            {     
                this.combatSystem = value;

                // Now that we have our combat system, create load the combat menu
                this.combatMenu = new CombatMenu(this.lhg, combatSystem.MyTurnBased.CombatMenuHandler);
                this.combatMenu.loadActionMenu();
            }
        }

        public CombatBoard MyCombatBoard
        {
            get { return this.combatBoard; }
            set { this.combatBoard = value; }
        }

        public CombatMenu MyCombatMenu
        {
            get { return this.combatMenu; }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Handle the mouse / keyboard movement for pan and scoll.  NOTE:  This needs to be put here first because handleMousePanAndZoom()
            // may update the world coordinates (view and projection matrices) of the camera and the methods that follow will need
            // to update their world coordinates to match.  If you don't, there is a "sloshing" effect that happens as you zoom in 
            // or out because the updates to the world coordinates are out of sync.            
            handleMousePanAndZoom(gameTime);
            handleKeyboardPanAndZoom(gameTime);

            // Update the combat system next so it can handle AI and other logistics about combat
            combatSystem.Update(gameTime);

            // Update the stage contents
            stage.Update(gameTime);

            // Update the board
            combatBoard.Update(gameTime);

            // Show the player menu if human controlled player is active
            if (this.combatMenu.Visible)
                combatMenu.Update(gameTime);

            combatHUD.Update(gameTime);

            // tell our base class it is time to update.  It may be handling a transition for us.
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            lhg.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            background.Draw(gameTime);
            lhg.MySpriteBatch.End();

            stage.MyView = this.view;
            stage.Draw(gameTime);

            // Draw the hexagonal combat board.  The board will all the players and obstacles that are on the board
            combatBoard.Draw(gameTime);

            if (this.combatMenu.Visible)
            {
                lhg.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                combatMenu.Draw(gameTime);
                lhg.MySpriteBatch.End();
            }

            lhg.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            combatHUD.Draw(gameTime);
            lhg.MySpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
