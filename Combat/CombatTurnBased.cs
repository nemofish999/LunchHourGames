 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using LunchHourGames.GamePlay;
using LunchHourGames.Drawing;
using LunchHourGames.Screen;
using LunchHourGames.Hexagonal;
using LunchHourGames.Sprite;
using LunchHourGames.Players;
using LunchHourGames.Console;
using LunchHourGames.Inventory;
using LunchHourGames.Obstacles;
using LunchHourGames.Combat.Actions;

namespace LunchHourGames.Combat
{
    public class CombatTurnBased : CombatAction.Handler
    {
        private LunchHourGames lhg;           // Our main XNA game object
        private CombatSystem combatSystem;    // The main combat system for easy reference.  Holds all combat objects and state

        private Player currentPlayer;         // The player that has control of the board
        private Player mainPlayer;            // The main player of this game.  If this player dies, the game is over.

        private int level;                    // The difficulty level of the current combat board
        private int round;                    // The current round being played

        // Player Combat Actions.  These implement the various actions that are perfomred during combat.
        private ChooseAction chooseAction;    // Allows the current player to choose a combat action to perform
        private MoveAction moveAction;        // Allows the current player to move on the board
        private AttackAction attackAction;    // Allows the current player to attack another player on the board
        private UseAction useAction;          // Allows the current player to use an item in invetory
        private DefendAction defendAction;    // Allows the current player to defend their current position
        private EndTurnAction endTurnAction;  // Allows the current player to end their turn

        private CombatAction currentAction;   // The current action being performed
        private CombatAction nextAction;
        private bool shouldStartNextRound = false;

        private static Random random = new Random();  // Random number generator to make the dice appear more random

        public CombatTurnBased(LunchHourGames lhg, CombatSystem combatSystem)
        {
            this.lhg = lhg;
            this.combatSystem = combatSystem;

            this.chooseAction = new ChooseAction(lhg, combatSystem, this);
            this.moveAction = new MoveAction(lhg, combatSystem, this);
            this.attackAction = new AttackAction(lhg, combatSystem, this);
            this.useAction = new UseAction(lhg, combatSystem, this);
            this.defendAction = new DefendAction(lhg, combatSystem, this);
            this.endTurnAction = new EndTurnAction(lhg, combatSystem, this);

            currentAction = null;
            nextAction = null;
        }

        public Player CurrentPlayer
        {
            get { return this.currentPlayer; }
            set { this.currentPlayer = value; }
        }

        public List<Player> ActivePlayers
        {
            get
            {
                List<Player> allPlayers = MyPlayers;
                List<Player> activePlayers = new List<Player>();
                foreach (Player player in allPlayers)
                {
                    if (player.IsActive)
                    {
                        activePlayers.Add(player);
                    }
                }

                return activePlayers;
            }
        }

        public List<Player> MyPlayers
        {
            get { return this.combatSystem.MyPlayers; }
        }

        public GameEntities MyGameEntities
        {
            get { return this.combatSystem.MyGameEntities; }
        }

        public CombatMenu.Handler CombatMenuHandler
        {
            get { return this.chooseAction; }
        }

        public int MyLevel
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public void startLevel(int level)
        {
            this.level = level;
            this.round = 0;
            this.mainPlayer = getMainPlayer();

            startNextRound();

            this.currentPlayer = MyGameEntities.moveToHumanControlledPlayer();
        }

        private void startNextRound()
        {
            // We have finished the round.  Roll to determine initiative.
            round++;
            rollDice(20);
            this.currentPlayer = sortPlayersByInitiative();
            moveToNextAction(CombatAction.ActionType.Choose);
        }

        private void rollDice(int dice)
        {
            foreach (Player player in MyPlayers)
            {
                player.Roll = random.Next(1, dice);
                player.recalcAllStats(level);
            }
        }

        private void rollDice(Player player, int dice)
        {
            player.Roll = random.Next(1, dice);
            player.recalcAllStats(level);
        }

        private Player sortPlayersByInitiative()
        {
            List<Player> players = ActivePlayers;

            players.Sort(Player.comparePlayersByInitiative);
            if (players.Count() > 0)
                return players[0];

            return null;
        }

        private Player getNextPlayer(Player player)
        {
            List<Player> players = ActivePlayers;

            int index = players.IndexOf(player);
            if (index + 1 >= players.Count)
                return null;
            else
                return players.ElementAt(index + 1);
        }

        private Player getMainPlayer()
        {
            foreach (Player player in MyPlayers)
            {
                if (player.IsMainPlayer)
                    return player;
            }

            return null;
        }

        public void Update(GameTime gameTime)
        {
            if ( nextAction != null )
            {
                // Another action is pending.  Stop the current action and move to the next action.
                if ( currentAction != null )
                    currentAction.Stop();

                currentAction = nextAction;
                nextAction = null;
            }
            else if (currentAction != null)
            {
                if (!currentAction.HasStarted)  // If we haven't started the action, do so now.
                    currentAction.Start();
                else
                    currentAction.Update(gameTime);
            } 
        }

        public void handleActionComplete(CombatAction.ActionType actionType)
        {
            if (!mainPlayer.IsAlive)
            {
                // The main player is dead.  The game is now over.
                this.combatSystem.endCombat(CombatSystem.Result.Fail);
            }
            else
            {
                // Main player is still alive, so continue with the combat scene...

                if (actionType == CombatAction.ActionType.Choose)
                {
                    // Current player has made a choice about which play they want to make.  Perform that action
                    chooseAction.Stop();
                    determineActionByMenu(chooseAction.MenuChoice);
                }
                else if (actionType == CombatAction.ActionType.EndTurn)
                {
                    moveToNextPlayer();
                }
                else
                {
                    // Determine if the player can choose another action or if their turn is finished
                    if (currentPlayer.MyAttributes.actionPoints <= 0)
                    {
                        // Player has ran out of action points, so their turn is now over.
                        // Move to the next player with the highest initiative.
                        moveToNextPlayer();
                    }

                    // Allow the player to choose the next action
                    moveToNextAction(CombatAction.ActionType.Choose);
                }
            }                     
        }

        private void moveToNextPlayer()
        {
            // Move to the next player with the highest initiative.
            currentPlayer = getNextPlayer(this.currentPlayer);
            if (currentPlayer == null)
            {
                // Every player has had a turn in this round.  Move to the next round.
                startNextRound();
            }
        }

        private void determineActionByMenu(CombatMenu.CombatMenuItem menuItem)
        {
            CombatAction.ActionType actionType = CombatAction.ActionType.Choose;

            switch (menuItem)
            {
                case CombatMenu.CombatMenuItem.Attack:
                    actionType = CombatAction.ActionType.Attack;
                    break;

                case CombatMenu.CombatMenuItem.Move:
                    actionType = CombatAction.ActionType.Move;
                    break;

                case CombatMenu.CombatMenuItem.Defend:
                    actionType = CombatAction.ActionType.Defend;
                    break;

                case CombatMenu.CombatMenuItem.Use:
                    actionType = CombatAction.ActionType.Use;
                    break;

                case CombatMenu.CombatMenuItem.Inventory:
                    break;

                case CombatMenu.CombatMenuItem.EndTurn:
                    actionType = CombatAction.ActionType.EndTurn;
                    break;
            }

            moveToNextAction(actionType);
        }

        private void moveToNextAction(CombatAction.ActionType actionType)
        {
            switch (actionType)
            {
                case CombatAction.ActionType.Choose:
                    this.nextAction = chooseAction;
                    break;

                case CombatAction.ActionType.Attack:
                    this.nextAction = attackAction;
                    break;

                case CombatAction.ActionType.Move:
                    this.nextAction = moveAction;
                    break;

                case CombatAction.ActionType.Use:
                    this.nextAction = useAction;
                    break;

                case CombatAction.ActionType.Defend:
                    this.nextAction = defendAction;
                    break;

                case CombatAction.ActionType.EndTurn:
                    this.nextAction = endTurnAction;
                    break;
            }
        }
    }
}
