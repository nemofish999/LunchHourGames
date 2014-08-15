using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace LunchHourGames.Combat.Actions
{
    public class EndTurnAction : CombatAction
    {
        public EndTurnAction(LunchHourGames lhg, CombatSystem combatSystem, Handler combatActionHandler)
            : base(lhg, combatSystem, combatActionHandler, ActionType.EndTurn)
        {
        }
            
        public override void Start()
        {
            combatActionHandler.handleActionComplete(actionType);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Stop()
        {
        }
    }
}
