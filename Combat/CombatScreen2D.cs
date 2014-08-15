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

namespace LunchHourGames.Combat
{
    class CombatScreen2D : GameScreen
    {
        private CombatSystem combatSystem;
        private BackgroundPanel background;

        private CombatHUD hud;

        public CombatScreen2D(LunchHourGames lhg, CombatSystem combatSystem)
            : base(lhg, Type.Combat)
        {
            this.combatSystem = combatSystem;
            this.lhg = lhg;
            //this.hud = new CombatHUD(lhg, combatSystem);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();           
        }

        public void setBackground(String fromContent)
        {
            Texture2D texture = lhg.Content.Load<Texture2D>(fromContent);
            //this.background = new BackgroundScreen(Game, this.spriteBatch, texture, false);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.hud.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            lhg.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            GraphicsDevice device = Game.GraphicsDevice;
            device.Clear(Color.BurlyWood);
            
            this.background.Draw(gameTime);
            spriteBatch.End();

            CombatBoard combatBoard = combatSystem.MyBoard;           
            combatBoard.DrawBoard();

            lhg.MySpriteBatch.Begin();
            if (combatSystem.areGridNumbersVisible())
            {
                int width = combatBoard.getBoardWidth();
                int height = combatBoard.getBoardHeight();

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Vector2 center = combatBoard.getCellCenter(i, j);
                        center.X -= 8;
                        center.Y -= 8;
                        String coord = i.ToString() + "," + j.ToString();
                        spriteBatch.DrawString(lhg.NormalFont, coord, center, Color.PowderBlue);
                    }
                }
            }

            // Draw the players
            List<Player> players = this.combatSystem.Players;
            foreach (Player player in players)
            {
                player.Draw(gameTime);


            // Draw all the other sprites that are on the board 
            // Obstacles

            List<Obstacle> obstacles = this.combatSystem.MyObstacles;
            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.Draw(gameTime);
            }

            //if (combatSystem.MyCombatEvent != null)
            //    combatSystem.MyCombatEvent.Draw(gameTime);

            switch (this.combatSystem.CurrentState)
            {
                case CombatSystem.State.ChooseAction:
                    this.combatSystem.ActionMenu.Draw(gameTime);
                    break;
                case CombatSystem.State.ChoosePathToMove:
                    break;
                case CombatSystem.State.ChooseTarget:
                    break;
                case CombatSystem.State.ChooseItem:
                    break;
                case CombatSystem.State.PerformingAction:
                    //handlePerformingAction(gameTime);
                    break;
                case CombatSystem.State.Pause:
                    break;
            }

            //base.Draw(gameTime);
            lhg.MySpriteBatch.End();

            //lhg.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            this.hud.Draw(gameTime);
            //spriteBatch.End();
             * */
        }

        private void handlePerformingAction(GameTime gameTime)
        {
            /*
            switch ( combatSystem.CurrentAction )
            {
                case CombatSystem.Action.Attack:
                    //if ( combatSystem.MyCombatEvent != null )
                    //    combatSystem.MyCombatEvent.Draw(gameTime);
                    break;

                case CombatSystem.Action.Move:
                    // Movement is handled by the player object when it is updated.
                    break;
            }
             * */
        }
    }
}