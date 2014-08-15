using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LunchHourGames.Screen;
using LunchHourGames.Players;

namespace LunchHourGames.Combat
{
    public class CombatMenu : DrawableGameComponent, ButtonMenuEvent
    {
        public enum CombatMenuItem
        {
            None,
            Attack,
            Move,
            Defend,
            Use,
            Inventory,
            EndTurn
        }

        public interface Handler
        {
            void handleCombatMenuEvent(CombatMenuItem menuItem);
        }

        private LunchHourGames lhg;
        private Handler menuHandler;

        private ButtonMenu actionMenu;

        public CombatMenu(LunchHourGames lhg, CombatMenu.Handler menuHandler)
            :base (lhg)
        {
            this.lhg = lhg;
            this.menuHandler = menuHandler;
        }

        public void loadActionMenu()
        {
            Texture2D actionButton = lhg.Content.Load<Texture2D>("GUI/blackbutton");
            actionMenu = new ButtonMenu(lhg, this.lhg.SmallFont, actionButton, 0, this);
            string[] items = { "Attack", 
                               "Move", 
                               "Defend",
                               "Use",
                               "Inventory",
                               "End Turn" };
            actionMenu.SetMenuItems(items);
            actionMenu.HiliteColor = Color.Red;
            actionMenu.NormalColor = Color.White;
            this.actionMenu.Visible = false;
        }

        public void showActionMenu(bool visible, Player player)
        {
            this.actionMenu.resetMenu();  // Reset the menu

            Visible = visible;

            if (visible)
            {
                Rectangle rectangle = player.getCurrentRectangle();
                Position = new Vector2(rectangle.Right, rectangle.Top);
            }        
        }

        public bool Visible
        {
            get { return this.actionMenu.Visible; }
            set { this.actionMenu.Visible = value; }
        }
        
        public Vector2 Position
        {
            get { return this.actionMenu.Position; }
            set { this.actionMenu.Position = value; }
        }
        
        public void userSelectedItem(ButtonMenuItem menuItem)
        {
            switch (menuItem.name)
            {
                case "Attack":
                    menuHandler.handleCombatMenuEvent(CombatMenuItem.Attack);
                    break;

                case "Move":
                    menuHandler.handleCombatMenuEvent(CombatMenuItem.Move);
                    break;

                case "Defend":
                    menuHandler.handleCombatMenuEvent(CombatMenuItem.Defend);
                    break;

                case "Use":
                    menuHandler.handleCombatMenuEvent(CombatMenuItem.Use);
                    break;

                case "Inventory":
                    menuHandler.handleCombatMenuEvent(CombatMenuItem.Inventory);
                    break;

                case "End Turn":
                    menuHandler.handleCombatMenuEvent(CombatMenuItem.EndTurn);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            actionMenu.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            actionMenu.Draw(gameTime);
            base.Draw(gameTime);
        }

    }
}
