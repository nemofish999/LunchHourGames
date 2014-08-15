using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace LunchHourGames.Combat.Actions
{
    public class UseAction : CombatAction
    {
        public UseAction(LunchHourGames lhg, CombatSystem combatSystem, Handler combatActionHandler)
            :base(lhg, combatSystem, combatActionHandler, ActionType.Use)
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
