using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using LunchHourGames.Players;
using LunchHourGames.AI;
using LunchHourGames.Hexagonal;

namespace LunchHourGames.Combat.Actions
{
    public abstract class CombatAction
    {
        protected LunchHourGames lhg;
        protected CombatSystem combatSystem;

        protected bool actionHasStarted = false;
        protected bool actionIsTriggered = false;
        protected bool actionHasFinished = false;

        public interface Handler
        {
            void handleActionComplete(ActionType actionType);
        }
        protected Handler combatActionHandler;

        public enum ActionType
        {
            Choose,
            Attack,
            Move,
            Use,
            Defend,
            EndTurn
        }
        protected ActionType actionType;

        protected CombatAction(LunchHourGames lhg, CombatSystem combatSystem, Handler combatActionHandler, ActionType actionType)
        {
            this.lhg = lhg;
            this.combatSystem = combatSystem;
            this.combatActionHandler = combatActionHandler;
            this.actionType = actionType;
        }

        public bool HasStarted
        {
            get { return this.actionHasStarted; }
        }

        public virtual void Start()
        {
            actionHasStarted = true;
            actionIsTriggered = false;
            actionHasFinished = false;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Stop()
        {
            actionHasStarted = false;
            actionIsTriggered = false;
            actionHasFinished = false;
        }

        public bool allowUserInteraction()
        {
            return (CurrentPlayer.MyType == Player.Type.Human && CurrentPlayer.IsComputerControlled == false);
        }

        public ActionType MyActionType
        {
            get { return this.actionType; }
        }

        protected Player CurrentPlayer
        {
            get { return this.combatSystem.MyTurnBased.CurrentPlayer; }
            set { this.combatSystem.MyTurnBased.CurrentPlayer = value; }
        }

        protected List<Player> MyPlayers
        {
            get { return this.combatSystem.MyPlayers; }
        }

        protected List<Player> ActivePlayers
        {
            get { return this.combatSystem.ActivePlayers; }
        }

        protected CombatBoard MyBoard
        {
            get { return this.combatSystem.MyBoard; }
        }

        protected CombatScreen MyScreen
        {
            get { return this.combatSystem.MyScreen; }
        }

        protected CombatMenu MyMenu
        {
            get { return this.combatSystem.MyScreen.MyCombatMenu; }
        }     

        protected bool isPointOnBoard(Point point)
        {
            return MyBoard.IsPointOnBoard(point);
        }

        protected Hex findActiveHex(Point point)
        {
            return MyBoard.findActiveHex(point);
        }
     }
}
