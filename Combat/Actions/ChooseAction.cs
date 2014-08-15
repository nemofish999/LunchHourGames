using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Players;
using LunchHourGames.Inventory;

namespace LunchHourGames.Combat.Actions
{
    public class ChooseAction : CombatAction, CombatMenu.Handler
    {
        private CombatMenu.CombatMenuItem menuChoice = CombatMenu.CombatMenuItem.None;  // What the user chose in the menu.  Currently set to none.

        public ChooseAction(LunchHourGames lhg, CombatSystem combatSystem, Handler combatActionHandler)
            : base(lhg, combatSystem, combatActionHandler, ActionType.Choose)
        {
        }

        public CombatMenu.CombatMenuItem MenuChoice
        {
            get { return this.menuChoice; }
        }

        public override void Start()
        {
            if (allowUserInteraction())
            {
                 // This player is under user control, so show the action menu
                showActionMenu(true);
                highlightCurrentPlayer(true);
            }
            else
            {
                // This player is under the computer's control.  Pick an action using AI
                showActionMenu(false);
                highlightCurrentPlayer(false);

                if (CurrentPlayer.MyType == Player.Type.Zombie)
                    handleZombieTurn();
            }

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Stop()
        {
            showActionMenu(false);
            highlightCurrentPlayer(false);
            base.Stop();
        }

        private void showActionMenu(bool visible)
        {
            MyMenu.showActionMenu(visible, CurrentPlayer);
        }

        private void highlightCurrentPlayer(bool highlight)
        {
            MyBoard.highlightCurrentPlayer(highlight);
        }

        public void handleCombatMenuEvent(CombatMenu.CombatMenuItem combatMenuItem)
        {
            // The current player has picked an action.  Tell the handler this action is complete.
            menuChoice = combatMenuItem;

            if (menuChoice == CombatMenu.CombatMenuItem.Attack)
            {
                // Find the weapon that was selected
                Player player = CurrentPlayer;
                InventoryStorage inventory = player.MyInventory;
                if (inventory != null)
                {
                    player.MyWeapon = inventory.getWeapon("pistol");
                }
            }

            combatActionHandler.handleActionComplete(actionType);
        }

        private void handleZombieTurn()
        {
            Zombie zombie = (Zombie)CurrentPlayer;
            CombatMenu.CombatMenuItem menuItem = zombie.handleCombatChooseAction();
            handleCombatMenuEvent(menuItem);
        }
    }
}
